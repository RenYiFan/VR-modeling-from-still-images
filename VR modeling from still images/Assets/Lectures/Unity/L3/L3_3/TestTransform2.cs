using UnityEngine;
using System.Collections;
using AndrewBox.Comp;
using AndrewBox.Util;

public class TestTransform2 : BaseBehavior {


    protected override void OnInitFirst()
    {

    }

    protected override void OnInitSecond()
    {

    }

    protected override void OnUpdate()
    {

    }
    protected void test0()
    {
        if (m_transform.parent != null)
        {
            Debugger.Log("parent:" + m_transform.parent.name);
        }
        else
        {
           Debugger.Log("I'm top root");
        }
        Debugger.Log("root:"+m_transform.root.name);
    }
    protected void OnGUI() 
    {
        if (drawBtn(0,"Parent And Root"))
        {
            test0();
        }
        if (drawBtn(1,"DetachChildren"))
        {
            m_transform.DetachChildren();
        }
    }
    protected bool drawBtn(int id,string text)
    {
         float w = 150;
        float h = 100;
        float gap = 10;
        float margin = 10;
        return GUI.Button(new Rect(margin+(w+gap)*id, margin, w, h),text);
    }
}
