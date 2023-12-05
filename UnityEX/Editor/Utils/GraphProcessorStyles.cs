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
#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;

namespace CZToolKit.GraphProcessor.Editors
{
    public static class GraphProcessorStyles
    {
        public const string GraphWindowUXMLFile = "GraphProcessor/UXML/GraphWindow";
        public const string BassicStyleFile = "GraphProcessor/Styles/BasicStyle";
        public const string BaseGraphViewStyleFile = "GraphProcessor/Styles/BaseGraphView";
        public const string BaseNodeViewStyleFile = "GraphProcessor/Styles/BaseNodeView";
        public const string BaseSimpleNodeViewStyleFile = "GraphProcessor/Styles/BaseSimpleNodeView";
        public const string BasePortViewStyleFile = "GraphProcessor/Styles/BasePortView";
        public const string BaseEdgeStyleFile = "GraphProcessor/Styles/BaseEdgeView";
        public const string BaseGroupStyleFile = "GraphProcessor/Styles/BaseGroupView";

        public static VisualTreeAsset GraphWindowTree { get; } = Resources.Load<VisualTreeAsset>(GraphWindowUXMLFile);
        public static StyleSheet BasicStyle { get; } = Resources.Load<StyleSheet>(BassicStyleFile);
        public static StyleSheet BaseGraphViewStyle { get; } = Resources.Load<StyleSheet>(BaseGraphViewStyleFile);
        public static StyleSheet BaseNodeViewStyle { get; } = Resources.Load<StyleSheet>(BaseNodeViewStyleFile);
        public static StyleSheet SimpleNodeViewStyle { get; } = Resources.Load<StyleSheet>(BaseSimpleNodeViewStyleFile);
        public static StyleSheet BasePortViewStyle { get; } = Resources.Load<StyleSheet>(BasePortViewStyleFile);
        public static StyleSheet BaseConnectionViewStyle { get; } = Resources.Load<StyleSheet>(BaseEdgeStyleFile);
        public static StyleSheet BaseGroupViewStyle { get; } = Resources.Load<StyleSheet>(BaseGroupStyleFile);
    }
}
#endif