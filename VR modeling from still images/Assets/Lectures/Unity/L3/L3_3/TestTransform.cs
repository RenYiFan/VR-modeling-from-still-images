using UnityEngine;
using System.Collections;
using AndrewBox.Comp;
using AndrewBox.Util;

public class TestTransform : BaseBehavior {

    protected static int m_idCreated = 0;
    protected int m_id = 0;
    protected override void OnInitFirst()
    {
        m_id = m_idCreated;
        m_idCreated++;
    }

    protected override void OnInitSecond()
    {

    }

    protected override void OnUpdate()
    {

    }
    protected void test()
    {
        Debugger.Log("hierarchyCount:"+m_transform.hierarchyCount);
        Debugger.Log("hierarchyCapacity:"+m_transform.hierarchyCapacity);
    }
    protected void OnGUI() 
    {
        float w = 150;
        float h = 100;
        float gap = 10;
        float margin = 10;
        if (GUI.Button(new Rect(margin+(w+gap)*m_id, margin, w, h), "Test_"+m_id))
        {
            test();
        }
    }
}
