using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : Singleton<Player>
{
    private GameObject parent;
    public GameObject entity;
    private SpriteRenderer mySprite;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private CapsuleCollider2D myCollider ;
    private BoxCollider2D footCollider;

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
        entity = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player/Player"), parent.transform);
        //初始化组件
        mySprite = entity.GetComponent<SpriteRenderer>();
        myRigidbody = entity.GetComponent<Rigidbody2D>();
        myAnimator = entity.GetComponent<Animator>();
        myCollider = entity.GetComponent<CapsuleCollider2D>();
        footCollider = entity.GetComponent<BoxCollider2D>();
        //加载摄像机
        var camera = GameObject.Find("Follow Camera").GetComponent<CinemachineVirtualCamera>();
        camera.Follow = entity.transform;
        //变更层级
        mySprite.sortingOrder = (int)OrderInLayer.Player;

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

    #region 角色控制
    //检测是否在地面
    void CheckGround()
    {
        isGround = footCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    //翻转移动方向
    void Flip()
    {
        //判断X轴有无速度
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            //如果方向往右不需要翻转
            if (myRigidbody.velocity.x > 0.1f)
            {
                entity.transform.localRotation = Quaternion.Euler(0, 0, 0);
                //顺便翻转朝向
                lookDirection = new Vector2(1, 0);
            }
            //如果往左则翻转
            if (myRigidbody.velocity.x < -0.1f)
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
        Vector2 playerVelocity = new Vector2(moveDirection * runSpeed * speedRate, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        //判断X轴有无速度
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Run", playerHasXAxisSpeed);

    }

    //跳跃
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround)
            {
                myAnimator.SetBool("Jump", true);
                Vector2 jumpVelocity = new Vector2(0.0f, jumpForce * jumpRate);
                myRigidbody.velocity = Vector2.up * jumpVelocity;
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    myAnimator.SetBool("DoubleJump", true);
                    Vector2 doubleJumpVelocity = new Vector2(0.0f, doubleJumpForce);
                    myRigidbody.velocity = Vector2.up * doubleJumpVelocity;
                    canDoubleJump = false;
                }
            }
        }
    }

    //切换
    void SwitchAnimation()
    {
        myAnimator.SetBool("Idle", false);
        if (myAnimator.GetBool("Jump"))
        {
            //当速度下降到最大值开始下落
            if (myRigidbody.velocity.y < 0.0f)
            {
                myAnimator.SetBool("Jump", false);
                myAnimator.SetBool("Fall", true);
            }
        }
        else if (isGround)
        {
            myAnimator.SetBool("Fall", false);
            myAnimator.SetBool("Idle", true);
        }
        //二段跳判断
        if (myAnimator.GetBool("DoubleJump"))
        {
            //当速度下降到最大值开始下落
            if (myRigidbody.velocity.y < 0.0f)
            {
                myAnimator.SetBool("DoubleJump", false);
                myAnimator.SetBool("DoubleFall", true);
            }
        }
        else if (isGround)
        {
            myAnimator.SetBool("DoubleFall", false);
            myAnimator.SetBool("Idle", true);
        }
    }
    #endregion

    #region 状态修改
    public void SetPos(Vector3 pos)
    {
        entity.transform.position = pos;
    }

    public void ReseState()
    {
        myRigidbody.velocity = Vector3.zero;
        myAnimator.SetBool("Idle", true);
    }
    #endregion
}
