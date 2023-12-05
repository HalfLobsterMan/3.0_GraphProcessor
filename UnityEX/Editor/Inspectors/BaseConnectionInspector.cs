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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion
#if UNITY_EDITOR && ODIN_INSPECTOR
using CZToolKitEditor;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace CZToolKit.GraphProcessor.Editors
{
    [CustomObjectEditor(typeof(BaseConnectionView))]
    public class BaseConnectionInspector : ObjectEditor
    {
        PropertyTree propertyTree;

        public override void OnEnable()
        {
            var view = Target as BaseConnectionView;
            if (view.ViewModel != null)
                propertyTree = PropertyTree.Create(view.ViewModel.Model);
        }

        public override void OnInspectorGUI()
        {
            var view = Target as BaseConnectionView;
            if (view == null || view.ViewModel == null)
                return;
            if (propertyTree == null)
                return;
            propertyTree.BeginDraw(false);
            foreach (var property in propertyTree.EnumerateTree(false, true))
            {
                switch (property.Name)
                {
                    case nameof(BaseConnection.fromNode):
                    case nameof(BaseConnection.fromPort):
                    case nameof(BaseConnection.toNode):
                    case nameof(BaseConnection.toPort):
                        continue;
                }
                EditorGUI.BeginChangeCheck();
                property.Draw();
                if (EditorGUI.EndChangeCheck() && view.ViewModel.TryGetValue(property.Name, out var bindableProperty))
                    bindableProperty.NotifyValueChanged();
            }
            propertyTree.EndDraw();
            Editor.Repaint();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            propertyTree?.Dispose();
        }
    }
}
#endif