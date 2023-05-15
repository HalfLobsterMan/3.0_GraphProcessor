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
using System.Collections.Generic;

namespace CZToolKit.GraphProcessor
{
    public class BaseGroup
    {
        public string groupName;
        public InternalVector2Int position;
        public InternalColor backgroundColor = new InternalColor(0.3f, 0.3f, 0.3f, 0.3f);
        public List<int> nodes = new List<int>();
    }
}