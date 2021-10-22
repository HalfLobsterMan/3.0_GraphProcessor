#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
using CZToolKit.Core.SharedVariable;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CZToolKit.GraphProcessor
{
    public abstract partial class BaseGraph : IntegratedViewModel
    {
        public const string POSITION_NAME = nameof(panOffset);
        public const string SCALE_NAME = nameof(scale);

        #region 字段
        public event Action<BaseNode> onNodeAdded;
        public event Action<BaseNode> onNodeRemoved;

        public event Action<BaseConnection> onConnected;
        public event Action<BaseConnection> onDisconnected;

        [NonSerialized] public List<SharedVariable> variables = new List<SharedVariable>();
        #endregion

        #region 属性
        public Vector3 Position
        {
            get { return GetPropertyValue<Vector3>(POSITION_NAME); }
            set { SetPropertyValue(POSITION_NAME, value); }
        }
        public Vector3 Scale
        {
            get { return GetPropertyValue<Vector3>(SCALE_NAME); }
            set { SetPropertyValue(SCALE_NAME, value); }
        }
        public IReadOnlyDictionary<string, BaseNode> Nodes { get { return nodes; } }
        public IReadOnlyList<BaseConnection> Connections { get { return connections; } }
        public IVariableOwner VarialbeOwner { get; private set; }
        public IReadOnlyList<SharedVariable> Variables
        {
            get
            {
                if (variables == null) CollectionVariables();
                return variables;
            }
        }
        #endregion

        public void Enable()
        {
            foreach (var node in nodes.Values)
            {
                node.Enable(this);
            }
            foreach (var edge in connections)
            {
                edge.Enable(this);
            }
        }

        protected override void InitializeBindableProperties()
        {
            this[POSITION_NAME] = new BindableProperty<Vector3>(panOffset, v => panOffset = v);
            this[SCALE_NAME] = new BindableProperty<Vector3>(scale, v => scale = v);
        }

        #region API
        public virtual void Initialize(IGraphOwner graphOwner)
        {
            InitializePropertyMapping(graphOwner);
        }

        private void CollectionVariables()
        {
            if (variables == null)
                variables = new List<SharedVariable>();
            else
                variables.Clear();
            foreach (var node in nodes.Values)
            {
                variables.AddRange(SharedVariableUtility.CollectionObjectSharedVariables(node));
            }
        }

        public void InitializePropertyMapping(IVariableOwner variableOwner)
        {
            if (variables == null)
                CollectionVariables();
            VarialbeOwner = variableOwner;
            foreach (var variable in variables)
            {
                variable.InitializePropertyMapping(VarialbeOwner);
            }

            foreach (var node in Nodes.Values)
            {
                node.OnInitializedPropertyMapping(variableOwner);
            }
        }

        public string GenerateNodeGUID()
        {
            while (true)
            {
                string guid = Guid.NewGuid().ToString();
                if (!nodes.ContainsKey(guid)) return guid;
            }
        }

        public void AddNode(BaseNode node)
        {
            if (node.ContainsKey(node.GUID))
                return;
            node.Enable(this);
            nodes[node.GUID] = node;
            if (variables == null)
                CollectionVariables();
            IEnumerable<SharedVariable> nodeVariables = SharedVariableUtility.CollectionObjectSharedVariables(node);
            variables.AddRange(nodeVariables);
            if (VarialbeOwner != null)
            {
                foreach (var variable in nodeVariables)
                {
                    variable.InitializePropertyMapping(VarialbeOwner);
                }
            }
            onNodeAdded?.Invoke(node);
        }

        public void RemoveNode(BaseNode node)
        {
            if (node == null) return;
            Disconnect(node);
            nodes.Remove(node.GUID);
            onNodeRemoved?.Invoke(node);
        }

        public void Connect(BaseConnection connection)
        {
            BaseConnection tempConnection = connections.Find(item =>
            item.FromNodeGUID == connection.FromNodeGUID
            && item.FromPortName == connection.FromPortName
            && item.ToNodeGUID == connection.ToNodeGUID
            && item.ToPortName == connection.ToPortName
            );
            if (tempConnection != null)
                return;

            connection.Enable(this);

            BasePort fromPort = connection.FromNode.GetPorts().FirstOrDefault(port => port.name == connection.FromPortName);
            if (fromPort.capacity == BasePort.Capacity.Single)
                Disconnect(connection.FromNode, fromPort);

            BasePort toPort = connection.ToNode.GetPorts().FirstOrDefault(port => port.name == connection.ToPortName);
            if (toPort.capacity == BasePort.Capacity.Single)
                Disconnect(connection.ToNode, toPort);

            connection.Enable(this);
            connections.Add(connection);
            onConnected?.Invoke(connection);
        }

        public BaseConnection Connect(BaseNode from, string fromPortName, BaseNode to, string toPortName)
        {
            BaseConnection connection = connections.Find(edge => edge.FromNode == from && edge.FromPortName == fromPortName && edge.ToNode == to && edge.ToPortName == toPortName);
            if (connection != null)
                return connection;

            BasePort fromPort = from.GetPorts().FirstOrDefault(port => port.name == fromPortName);
            if (fromPort.capacity == BasePort.Capacity.Single)
                Disconnect(from, fromPort);

            BasePort toPort = to.GetPorts().FirstOrDefault(port => port.name == toPortName);
            if (toPort.capacity == BasePort.Capacity.Single)
                Disconnect(to, toPort);

            connection = NewConnection(from, fromPortName, to, toPortName);
            connection.Enable(this);
            connections.Add(connection);
            onConnected?.Invoke(connection);
            return connection;
        }

        public void Disconnect(BaseNode node)
        {
            // 断开节点所有连接
            foreach (var connection in Connections.ToArray())
            {
                if (connection.FromNodeGUID == node.GUID || connection.ToNodeGUID == node.GUID)
                    Disconnect(connection);
            }
        }

        public void Disconnect(BaseConnection edge)
        {
            if (!connections.Contains(edge)) return;
            connections.Remove(edge);
            onDisconnected?.Invoke(edge);
        }

        public void Disconnect(BaseNode node, BasePort port)
        {
            Disconnect(node, port.name);
        }

        public void Disconnect(BaseNode node, string portName)
        {
            foreach (var edge in connections.ToArray())
            {
                if ((edge.FromNode == node && edge.FromPortName == portName) || (edge.ToNode == node && edge.ToPortName == portName))
                    Disconnect(edge);
            }
        }
        #endregion

        #region Overrides
        public T NewNode<T>(Vector2 position) where T : BaseNode { return NewNode(typeof(T), position) as T; }
        public virtual BaseNode NewNode(Type type, Vector2 position)
        {
            return BaseNode.CreateNew(type, this, position);
        }
        public virtual BaseConnection NewConnection(BaseNode from, string fromPortName, BaseNode to, string toPortName)
        {
            return BaseConnection.CreateNew<BaseConnection>(from, fromPortName, to, toPortName);
        }
        #endregion
    }
}