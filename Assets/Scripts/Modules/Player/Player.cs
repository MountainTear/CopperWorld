using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Spine.Unity;
using Spine;

public class Player : Singleton<Player>
{
    private GameObject parent;
    public GameObject entity;
    private Rigidbody2D myRigidbody;
    private CapsuleCollider2D myCollider;
    private BoxCollider2D footCollider;
    public CinemachineVirtualCamera camera;
    private SkeletonAnimationHandle animationHandle;
    private SkeletonAnimation skeletonAnimation;

    //输入
    private string xAxis = "Horizontal";
    private string yAxis = "Vertical";
    private string jumpButton = "Jump";
    private string useWeaponButton = "UseWeapon";
    private string floatButton = "Float";
    private List<string> buttonNames;
    private Dictionary<string, bool[]> buttonStateList;     //0是cache，1是stop，2是start
    //缓存
    private Vector2 input;
    private Vector2 velocity;
    //移动速度
    private float walkToRun = 0.6f; //输入从走变为跑的突变值
    private float walkSpeed = 9f;   //暂时屏蔽走动
    private float runSpeed = 9f;
    //跳跃
    private float minimumJumpEndTime;   //跳跃停止时间戳
    private float jumpSpeed = 7.0f; //跳跃力
    private float minimumJumpDuration = 0.5f;    //最长跳跃时间
    private float jumpInterruptFactor = 0.5f;    //跳跃力衰减系数
    private float gravityScale = 1f;   //重力缩放
    //浮空
    private float floatSpeed = 7.0f; //浮空力
    //地面判断
    private bool isGrounded;
    private bool wasGrounded;
    //状态
    private CharacterState previousState, currentState;
    public bool isInFloat;   //浮空
    public bool isInUseWeapon;  //使用武器

    public Player()
    {
        parent = GameObject.Find("Player");
    }

    public void Init()
    {
        //实例化实体
        entity = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player/Player"), parent.transform);
        //初始化组件
        myRigidbody = entity.GetComponent<Rigidbody2D>();
        myCollider = entity.GetComponent<CapsuleCollider2D>();
        footCollider = entity.GetComponent<BoxCollider2D>();
        animationHandle = entity.GetComponent<SkeletonAnimationHandle>();
        skeletonAnimation = animationHandle.skeletonAnimation;
        //加载摄像机
        camera = GameObject.Find("Follow Camera").GetComponent<CinemachineVirtualCamera>();
        camera.Follow = entity.transform;
        //变更层级
        skeletonAnimation.transform.GetComponent<MeshRenderer>().sortingOrder = (int)OrderInLayer.Player;
        //初始化变量
        wasGrounded = false;
        input = Vector2.zero;
        velocity = Vector2.zero;
        minimumJumpEndTime = 0;
        buttonNames = new List<string>() { jumpButton, useWeaponButton, floatButton };
        buttonStateList = new Dictionary<string, bool[]>
        {
            { buttonNames[0], new bool[]{ false, false, false} },
            { buttonNames[1], new bool[]{ false, false, false} },
            { buttonNames[2], new bool[]{ false, false, false} },
        };
        isInFloat = false;
        isInUseWeapon = false;
    }

    public void FixedUpdate()
    {
        CheckGround();
        UpdateCharacter();
    }

    #region 角色控制
    private void CheckGround()
    {
        isGrounded = footCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
    private bool IsInSpecialState()
    {
        return isInUseWeapon || isInFloat;
    }

    private void UpdateCharacter()
    {
        float dt = Time.deltaTime;

        input.x = Input.GetAxis(xAxis);
        input.y = Input.GetAxis(yAxis);
        
        //避免FixedUpdate
        foreach(string name in buttonNames)
        {
            bool buttonNew = Input.GetButton(name);
            bool buttonCache = buttonStateList[name][0];
            if (buttonNew != buttonCache)
            {
                if (buttonCache)
                {
                    buttonStateList[name][1] = true;
                    buttonStateList[name][2] = false;
                }
                else
                {
                    buttonStateList[name][2] = true;
                    buttonStateList[name][1] = false;
                }
                buttonStateList[name][0] = buttonNew;
            }
        }

        bool doJumpInterrupt = false;   //打断跳跃
        bool doJump = false;    //跳跃

        //逻辑处理
        if (isGrounded)
        {
            if (IsInSpecialState())
            {
                //结束使用武器
                if (buttonStateList[useWeaponButton][1] && isInUseWeapon)
                {
                    isInUseWeapon = false;
                }
            }
            else
            {
                //跳跃>浮空>使用武器
                //开始跳跃
                if (buttonStateList[jumpButton][2])
                {
                    doJump = true;
                }
                //开始浮空
                if ((!doJump) && buttonStateList[floatButton][2] && !isInFloat)
                {
                    //这里需要判断是否能浮空
                    isInFloat = true;
                }
                //开始使用武器
                if ((!doJump && !isInFloat) && buttonStateList[useWeaponButton][2] && !isInFloat)
                {
                    isInUseWeapon = true;
                }
            }
        }
        else
        {
            //开始打断跳跃
            doJumpInterrupt = buttonStateList[jumpButton][1] && Time.time < minimumJumpEndTime;

            //结束浮空，视做打断跳跃
            if (buttonStateList[floatButton][1] && isInFloat)
            {
                isInFloat = false;
                doJumpInterrupt = true;
                //这里需要调用结束浮空函数
            }

        }

        //受力处理
        Vector2 gravityDeltaVelocity = Physics2D.gravity * gravityScale * dt;
        if (doJump)
        {
            velocity.y = jumpSpeed;
            minimumJumpEndTime = Time.time + minimumJumpDuration;
        }else if (doJumpInterrupt)
        {
            if (velocity.y > 0)
                velocity.y *= jumpInterruptFactor;
        }else if (isInFloat)
        {
            velocity.y = floatSpeed;
        }

        velocity.x = 0;
        if (!isInUseWeapon)    //使用武器时不允许移动
        {
            if (input.x != 0)
            {
                velocity.x = Mathf.Abs(input.x) > walkToRun ? runSpeed : walkSpeed;
                velocity.x *= Mathf.Sign(input.x);
            }
        }
        if (!isGrounded)
        {
            if (wasGrounded)
            {
                if (velocity.y < 0)
                    velocity.y = 0;
            }
            else
            {
                velocity += gravityDeltaVelocity;
            }
        }

        //移动处理
        if (velocity.x != 0 || velocity.y != 0)
        {
            myRigidbody.MovePosition(myRigidbody.position + velocity * dt);
        }
        wasGrounded = isGrounded;

        //动画状态处理
        if (IsInSpecialState())
        {
            if (isInUseWeapon)
            {
                currentState = PlayerMgr.Instance.ModeToCharacterState();
            }
            else if (isInFloat)
            {
                currentState = CharacterState.Float;
            }
        }
        else
        {
            if (isGrounded)
            {
                if (input.x == 0)
                    currentState = CharacterState.Idle;
                else
                    currentState = Mathf.Abs(input.x) > walkToRun ? CharacterState.Run : CharacterState.Walk;
            }
            else
            {
                currentState = velocity.y > 0 ? CharacterState.Rise : CharacterState.Fall;
            }
        }
        bool stateChanged = previousState != currentState;
        previousState = currentState;
        if (stateChanged)
            HandleStateChanged();

        //处理朝向
        if (input.x != 0)
            animationHandle.SetFlip(input.x);
    }

    private void HandleStateChanged()
    {
        string stateName = null;
        switch (currentState)
        {
            case CharacterState.Idle:
                stateName = "idle";
                break;
            case CharacterState.Walk:   //暂时屏蔽走动动画
                stateName = "run";
                break;
            case CharacterState.Run:
                stateName = "run";
                break;
            case CharacterState.Rise:
                stateName = "rise";
                break;
            case CharacterState.Fall:
                stateName = "fall";
                break;
            case CharacterState.Attack:
                stateName = "attack";
                break;
            case CharacterState.Mine:
                stateName = "attack";   //暂时用攻击代替
                break;
            case CharacterState.Float:
                stateName = "float";
                break;
            default:
                break;
        }

        animationHandle.PlayAnimationForState(stateName, 0);
    }
    #endregion

    #region 状态获取和修改
    public void SetPos(Vector3 pos)
    {
        entity.transform.position = pos;
    }

    public FlipType GetFlipType()
    {
        return skeletonAnimation.Skeleton.ScaleX == 1.0f ? FlipType.Right : FlipType.Left;
    }
    #endregion
}