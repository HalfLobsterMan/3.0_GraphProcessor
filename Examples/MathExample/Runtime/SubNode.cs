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

using System.Collections.Generic;
using CZToolKit.VM;
using CZToolKit.GraphProcessor;

[NodeMenu("Sub")]
public class SubNode : BaseNode
{
    public List<string> ports = new List<string>();
}

[ViewModel(typeof(SubNode))]
public class SubNodeVM : BaseNodeVM, IGetPortValue, IGetPortValue<float>
{
    public SubNodeVM(BaseNode model) : base(model)
    {
        AddPort(new BasePortVM("InputA", BasePort.Orientation.Horizontal, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(float)));
        AddPort(new BasePortVM("InputB", BasePort.Orientation.Horizontal, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(float)));
        AddPort(new BasePortVM("Result", BasePort.Orientation.Horizontal, BasePort.Direction.Output, BasePort.Capacity.Multi, typeof(float))
        {
            HideLabel = true
        });
    }

    public object GetValue(string port)
    {
        var inputAValue = Ports["InputA"].GetConnectionValue<float>();
        var inputBValue = Ports["InputB"].GetConnectionValue<float>();
        return inputAValue - inputBValue;
    }

    float IGetPortValue<float>.GetValue(string port)
    {
        var inputAValue = Ports["InputA"].GetConnectionValue<float>();
        var inputBValue = Ports["InputB"].GetConnectionValue<float>();
        return inputAValue - inputBValue;
    }
}
