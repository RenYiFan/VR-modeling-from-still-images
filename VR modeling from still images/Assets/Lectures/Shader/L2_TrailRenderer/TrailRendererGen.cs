using UnityEngine;
using System.Collections;
using AndrewBox.Comp;

public class TrailRendererGen : BaseBehavior 
{
    [SerializeField][Tooltip("旋转角速度")]
    protected float m_angle=360;
    protected override void OnInitFirst()
    {
    }

    protected override void OnInitSecond()
    {
    }

    protected override void OnUpdate()
    {
        m_transform.Rotate(Vector3.forward, m_angle*Time.deltaTime);
    }
}
