#region ע ��
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: ��ֻ��Ϻ��
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace CZToolKit.GraphProcessor.Editors
{
    public class NodeEditorAssetModProcessor : UnityEditor.AssetModificationProcessor
    {
        [MenuItem("Assets/Graph Processor/Generate Node Script")]
        public static void GenerateNodeScript()
        {

        }

//        /// <summary> ɾ���ڵ�ű�֮ǰ�Զ�ɾ���ڵ� </summary> 
//        private static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
//        {
//            // ������ɾ������Դ·��
//            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

//            if (obj is MonoScript)
//            {
//                // ���ű����ͣ�������ǽڵ����򷵻�
//                MonoScript script = obj as MonoScript;
//                System.Type scriptType = script.GetClass();
//                if (scriptType != null && (scriptType == typeof(BaseNode) || scriptType.IsSubclassOf(typeof(BaseNode))))
//                {
//                    string[] graphGUIDs = AssetDatabase.FindAssets("t:" + typeof(BaseGraphAsset));
//                    foreach (string graphGUID in graphGUIDs)
//                    {
//                        string graphPath = AssetDatabase.GUIDToAssetPath(graphGUID);
//                        BaseGraphAsset graphAsset = AssetDatabase.LoadAssetAtPath<BaseGraphAsset>(graphPath);
//                        foreach (var item in graphAsset.Graph.Nodes.Values.ToArray())
//                        {
//                            if (item != null && scriptType == item.GetType())
//                                graphAsset.Graph.RemoveNode(item);
//                        }
//                    }

//                    AssetDatabase.SaveAssets();
//                    AssetDatabase.Refresh();
//                }
//            }
//            else if (obj is BaseGraphAsset)
//            {
//                if (obj != null)
//                {
//                    foreach (var graphWindow in Resources.FindObjectsOfTypeAll<BaseGraphWindow>().Where(w => w.GraphAsset == obj))
//                        graphWindow.Clear();
//                }
//            }

//            // ������unityɾ��Ӧ��ɾ���Ľű�
//            return AssetDeleteResult.DidNotDelete;
//        }
    }
}