using UnityEngine;
using System.Collections;
using AndrewBox.Util;
using AndrewBox.Comp;

public class TestComponenets : BaseBehavior 
{
    protected override void OnInitFirst()
    {
        Debugger.LogAtFrame("OnInitFirst");
    }

    protected override void OnInitSecond()
    {
        Debugger.LogAtFrame("OnInitSecond");
    }

    protected override void OnUpdate()
    {
        Debugger.LogAtFrame("OnUpdate");
    }

    protected void OnEnable()
    {
        Debugger.LogAtFrame("OnEnable");
    }
    protected void OnDisable()
    {
        Debugger.LogAtFrame("OnDisable");
    }
    protected void OnDestroy()
    {
        Debugger.LogAtFrame("OnDestroy");
    }


}
