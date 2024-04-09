using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : Singleton<Player>
{
    private GameObject parent;
    private GameObject entity;
    private Rigidbody2D uRigidbody;
    private Animator uAnimator;
    private BoxCollider2D uBox;

    private bool isInit;
    //移动速度
    public float runSpeed = 10.0f;
    //跳跃能力
    public float jumpForce = 7.0f;
    public float doubleJumpForce = 3.0f;
    //bool 条件
    //是否在地面
    private bool isGround;
    //能否二段跳
    public bool canDoubleJump;
    //朝向
    public Vector2 lookDirection;
    //影响移动比例
    public float speedRate = 1;
    public float jumpRate = 1;

    public Player()
    {
        parent = GameObject.Find("Player");
        isInit = false;
    }

    public void Init()
    {
        //实例化实体
        entity = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Scene/Player"), parent.transform);
        //初始化组件
        uRigidbody = entity.GetComponent<Rigidbody2D>();
        uAnimator = entity.GetComponent<Animator>();
        uBox = entity.GetComponent<BoxCollider2D>();
        //加载摄像机
        var camera = GameObject.Find("Follow Camera").GetComponent<CinemachineVirtualCamera>();
        camera.Follow = entity.transform;

        isInit = true;
    }

    public void Update()
    {
        if (!isInit)
        {
            return;
        }
        Flip();
        Run();
        Jump();
        CheckGround();
        SwitchAnimation();
    }

    //检测是否在地面
    void CheckGround()
    {
        isGround = uBox.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    //翻转移动方向
    void Flip()
    {
        //判断X轴有无速度
        bool playerHasXAxisSpeed = Mathf.Abs(uRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            //如果方向往右不需要翻转
            if (uRigidbody.velocity.x > 0.1f)
            {
                entity.transform.localRotation = Quaternion.Euler(0, 0, 0);
                //顺便翻转朝向
                lookDirection = new Vector2(1, 0);
            }
            //如果往左则翻转
            if (uRigidbody.velocity.x < -0.1f)
            {
                entity.transform.localRotation = Quaternion.Euler(0, 180, 0);
                //顺便翻转朝向
                lookDirection = new Vector2(-1, 0);
            }
        }
    }

    //左右移动
    void Run()
    {
        //获取移动方向
        float moveDirection = Input.GetAxis("Horizontal");

        //移动速度
        Vector2 playerVelocity = new Vector2(moveDirection * runSpeed * speedRate, uRigidbody.velocity.y);
        uRigidbody.velocity = playerVelocity;

        //判断X轴有无速度
        bool playerHasXAxisSpeed = Mathf.Abs(uRigidbody.velocity.x) > Mathf.Epsilon;
        uAnimator.SetBool("Run", playerHasXAxisSpeed);

    }

    //跳跃
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround)
            {
                uAnimator.SetBool("Jump", true);
                Vector2 jumpVelocity = new Vector2(0.0f, jumpForce * jumpRate);
                uRigidbody.velocity = Vector2.up * jumpVelocity;
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    uAnimator.SetBool("DoubleJump", true);
                    Vector2 doubleJumpVelocity = new Vector2(0.0f, doubleJumpForce);
                    uRigidbody.velocity = Vector2.up * doubleJumpVelocity;
                    canDoubleJump = false;
                }
            }
        }
    }

    //切换
    void SwitchAnimation()
    {
        uAnimator.SetBool("Idle", false);
        if (uAnimator.GetBool("Jump"))
        {
            //当速度下降到最大值开始下落
            if (uRigidbody.velocity.y < 0.0f)
            {
                uAnimator.SetBool("Jump", false);
                uAnimator.SetBool("Fall", true);
            }
        }
        else if (isGround)
        {
            uAnimator.SetBool("Fall", false);
            uAnimator.SetBool("Idle", true);
        }
        //二段跳判断
        if (uAnimator.GetBool("DoubleJump"))
        {
            //当速度下降到最大值开始下落
            if (uRigidbody.velocity.y < 0.0f)
            {
                uAnimator.SetBool("DoubleJump", false);
                uAnimator.SetBool("DoubleFall", true);
            }
        }
        else if (isGround)
        {
            uAnimator.SetBool("DoubleFall", false);
            uAnimator.SetBool("Idle", true);
        }
    }
}
