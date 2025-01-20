﻿#if UNITY_EDITOR
using Moyo.GraphProcessor.Editors;
using MoyoEditor;
using UnityEditor;
using UnityEngine.UIElements;

[CustomView(typeof(SVNUpdateNode))]
public class SVNUpdateNodeView : BaseNodeView
{
    protected override void OnInitialized()
    {
        var editor = ObjectInspectorEditor.CreateEditor(this);
        controls.Add(new IMGUIContainer(() =>
        {
            var wideMode = EditorGUIUtility.wideMode;
            EditorGUIUtility.wideMode = true;
            EditorGUI.BeginChangeCheck();
            editor.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
                this.MarkDirtyRepaint();
            EditorGUIUtility.wideMode = wideMode;
        }));
    }
}
#endif