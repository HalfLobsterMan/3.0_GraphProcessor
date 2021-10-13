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

namespace CZToolKit.GraphProcessor.Editors
{
    public sealed class DefaultSimpleNodeView : SimpleNodeView<BaseNode>
    {
        public override PortView NewPortView(Slot slot)
        {
            return DefaultPortView.CreatePV(slot, typeof(object));
        }
    }
}