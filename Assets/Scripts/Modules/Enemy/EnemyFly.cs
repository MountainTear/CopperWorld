using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 飞行类敌人，不随机移动，当主角出现时会朝他移动
/// </summary>
public class EnemyFly : Enemy
{
    //获取刚体
    private Rigidbody2D rbody;
    //等待冲刺时间
    public float startWaitTime = 1;
    private float waitTime;
    //定义攻击跟随范围
    public GameObject attckRange;

    public new void Start()
    {
        base.Start();
        waitTime = startWaitTime;
        rbody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        waitTime = waitTime - Time.deltaTime;
    }

    //朝主角自动攻击
    public void AutoAttack(Transform playerT)
    {
        if (waitTime < 0)
        {
            rbody.MovePosition((playerT.position - rbody.transform.position) / 3 + rbody.transform.position);
            waitTime = startWaitTime;
        }
    }

}
