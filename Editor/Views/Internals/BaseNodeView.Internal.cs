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
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

using NodeView = UnityEditor.Experimental.GraphView.Node;

namespace CZToolKit.GraphProcessor.Editors
{
    public partial class BaseNodeView : NodeView, IBindableView<BaseNode>
    {
        #region 字段
        Label titleLabel;
        public readonly VisualElement nodeBorder;
        public readonly VisualElement topPortContainer;
        public readonly VisualElement bottomPortContainer;
        public readonly VisualElement controlsContainer;
        public readonly VisualElement contentsHorizontalDivider;
        public readonly VisualElement portsVerticalDivider;
        public readonly VisualElement controlsHorizontalDivider;
        public readonly Dictionary<string, BasePortView> portViews = new Dictionary<string, BasePortView>();
        [NonSerialized]
        List<IconBadge> badges = new List<IconBadge>();
        #endregion

        #region 属性
        public Label TitleLabel
        {
            get
            {
                if (titleLabel == null)
                    titleLabel = titleContainer.Q<Label>("title-label");
                return titleLabel;
            }
        }
        public BaseGraphView Owner { get; private set; }
        public BaseNode Model { get; protected set; }
        #endregion

        public BaseNodeView()
        {
            styleSheets.Add(GraphProcessorStyles.BaseNodeViewStyle);

            nodeBorder = this.Q(name: "node-border");

            contentsHorizontalDivider = contentContainer.Q(name: "divider", className: "horizontal");
            contentsHorizontalDivider.AddToClassList("contents-horizontal-divider");

            portsVerticalDivider = topContainer.Q(name: "divider", className: "vertical");
            portsVerticalDivider.AddToClassList("ports-vertical-divider");

            controlsContainer = new VisualElement { name = "controls" };
            controlsContainer.AddToClassList("node-controls");
            mainContainer.Add(controlsContainer);

            topPortContainer = new VisualElement { name = "top-port-container" };
            topPortContainer.style.justifyContent = Justify.Center;
            topPortContainer.style.alignItems = Align.Center;
            topPortContainer.style.flexDirection = FlexDirection.Row;
            Insert(0, topPortContainer);

            bottomPortContainer = new VisualElement { name = "bottom-port-container" };
            bottomPortContainer.style.justifyContent = Justify.Center;
            bottomPortContainer.style.alignItems = Align.Center;
            bottomPortContainer.style.flexDirection = FlexDirection.Row;
            Add(bottomPortContainer);

            TitleLabel.style.flexWrap = Wrap.Wrap;
        }

        #region Initialize
        public void SetUp(BaseNode node, BaseGraphView graphView)
        {
            Model = node;
            Owner = graphView;

            // 初始化
            base.SetPosition(new Rect(Model.Position == default ? Vector2.zero : Model.Position, GetPosition().size));
            title = Model.Title;
            tooltip = Model.Tooltip;
            titleContainer.style.backgroundColor = Model.TitleColor;
            TitleLabel.style.color = Model.TitleColor.GetLuminance() > 0.5f && Model.TitleColor.a > 0.5f ? Color.black : Color.white * 0.9f;

            foreach (var port in Model.Ports.Values)
            {
                BasePortView portView = NewPortView(port);
                portView.SetUp(port, Owner);
                portViews[port.name] = portView;

                if (portView.orientation == Orientation.Horizontal)
                {
                    if (portView.direction == Direction.Input)
                        inputContainer.Add(portView);
                    else
                        outputContainer.Add(portView);
                }
                else
                {
                    if (portView.direction == Direction.Input)
                        topPortContainer.Add(portView);
                    else
                        bottomPortContainer.Add(portView);
                }
            }
            RefreshPorts();
            RefreshContentsHorizontalDivider();
            OnInitialized();
        }

        public void BindingProperties()
        {
            Model.BindingProperty<Vector2>(BaseNode.POSITION_NAME, OnPositionChanged);
            Model.BindingProperty<string>(BaseNode.TITLE_NAME, OnTitleChanged);
            Model.BindingProperty<Color>(BaseNode.TITLE_COLOR_NAME, OnTitleColorChanged);
            Model.BindingProperty<string>(BaseNode.TOOLTIP_NAME, OnTooltipChanged);

            Model.onPortAdded += OnPortAdded;
            Model.onPortRemoved += OnPortRemoved;

            foreach (var portView in portViews.Values)
            {
                portView.BindingProperties();
            }

            OnBindingProperties();
        }

        public void UnBindingProperties()
        {
            Model.UnBindingProperty<string>(BaseNode.TITLE_NAME, OnTitleChanged);
            Model.UnBindingProperty<Color>(BaseNode.TITLE_COLOR_NAME, OnTitleColorChanged);
            Model.UnBindingProperty<string>(BaseNode.TOOLTIP_NAME, OnTooltipChanged);
            Model.UnBindingProperty<Vector2>(BaseNode.POSITION_NAME, OnPositionChanged);

            Model.onPortAdded -= OnPortAdded;
            Model.onPortRemoved -= OnPortRemoved;

            foreach (var portView in portViews.Values)
            {
                portView.UnBindingProperties();
            }

            OnUnBindingProperties();
        }

        void RefreshContentsHorizontalDivider()
        {
            if (portViews.Values.FirstOrDefault(port => port.orientation == Orientation.Horizontal) != null)
                contentsHorizontalDivider.RemoveFromClassList("hidden");
            else
                contentsHorizontalDivider.AddToClassList("hidden");
        }
        #endregion

        #region Callbacks
        void OnPortAdded(BasePort port)
        {
            BasePortView portView = NewPortView(port);
            portView.SetUp(port, Owner);
            portView.BindingProperties();
            portViews[port.name] = portView;

            if (portView.orientation == Orientation.Horizontal)
            {
                if (portView.direction == Direction.Input)
                    inputContainer.Add(portView);
                else
                    outputContainer.Add(portView);
            }
            else
            {
                if (portView.direction == Direction.Input)
                    topPortContainer.Add(portView);
                else
                    bottomPortContainer.Add(portView);
            }
            RefreshPorts();
            RefreshContentsHorizontalDivider();
        }

        void OnPortRemoved(BasePort port)
        {
            portViews[port.name].RemoveFromHierarchy();
            portViews[port.name].UnBindingProperties();
            portViews.Remove(port.name);
            RefreshPorts();
            RefreshContentsHorizontalDivider();
        }

        void OnTitleChanged(string title)
        {
            base.title = title;
        }
        void OnTooltipChanged(string tooltip)
        {
            base.tooltip = tooltip;
        }
        void OnPositionChanged(Vector2 position)
        {
            base.SetPosition(new Rect(position, GetPosition().size));
            Owner.SetDirty();
        }
        void OnTitleColorChanged(Color color)
        {
            titleContainer.style.backgroundColor = color;
            TitleLabel.style.color = color.GetLuminance() > 0.5f && color.a > 0.5f ? Color.black : Color.white * 0.9f;
        }
        #endregion

        public void SetDeletable(bool deletable)
        {
            if (deletable)
                capabilities |= Capabilities.Deletable;
            else
                capabilities &= ~Capabilities.Deletable;
        }

        public void SetMovable(bool movable)
        {
            if (movable)
                capabilities = capabilities | Capabilities.Movable;
            else
                capabilities = capabilities & (~Capabilities.Movable);
        }

        public void SetSelectable(bool selectable)
        {
            if (selectable)
                capabilities |= Capabilities.Selectable;
            else
                capabilities &= ~Capabilities.Selectable;
        }
    }
}
#endif