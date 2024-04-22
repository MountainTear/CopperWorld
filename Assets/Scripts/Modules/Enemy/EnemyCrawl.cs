using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrawl : Enemy
{
    //定义变量
    public float speed = 2;
    public float startWaitTime = 1;
    private float waitTime;

    //定义移动范围
    public float moveX;
    private float leftX = -2f;
    private float rightX = 2f;
    private Vector3 posTarget;
    private Vector3 posOrigin;

    public new void Start()
    {
        base.Start();
        waitTime = startWaitTime;
        moveX = GetRandomX();
        posOrigin = transform.position;
        posTarget = posOrigin;
    }
   
    public void Update()
    {
        posTarget.x = posOrigin.x + moveX;
        transform.position = Vector3.MoveTowards(transform.position, posTarget, speed * Time.deltaTime);
        if (Math.Abs(posTarget.x - transform.position.x) < 0.1f)
        {
            if (waitTime <= 0)
            {
                moveX = GetRandomX();
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    float GetRandomX()
    {
        return UnityEngine.Random.Range(leftX, rightX);
    }

}