#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 
 *
 */
#endregion
using CZToolKit.Core.Editors;
using UnityEditor;

namespace CZToolKit.GraphProcessor.Editors
{
    [CustomObjectEditor(typeof(BaseNode))]
    public class BaseNodeObjectEditor : ObjectEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                IBaseGraphFromUnityObject inAsset = (Target as BaseNode).Owner as IBaseGraphFromUnityObject;
                if (inAsset != null)
                    EditorUtility.SetDirty(inAsset.From);
            }
        }
    }
}