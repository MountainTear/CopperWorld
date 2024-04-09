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
    //�ƶ��ٶ�
    public float runSpeed = 10.0f;
    //��Ծ����
    public float jumpForce = 7.0f;
    public float doubleJumpForce = 3.0f;
    //bool ����
    //�Ƿ��ڵ���
    private bool isGround;
    //�ܷ������
    public bool canDoubleJump;
    //����
    public Vector2 lookDirection;
    //Ӱ���ƶ�����
    public float speedRate = 1;
    public float jumpRate = 1;

    public Player()
    {
        parent = GameObject.Find("Player");
        isInit = false;
    }

    public void Init()
    {
        //ʵ����ʵ��
        entity = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Scene/Player"), parent.transform);
        //��ʼ�����
        uRigidbody = entity.GetComponent<Rigidbody2D>();
        uAnimator = entity.GetComponent<Animator>();
        uBox = entity.GetComponent<BoxCollider2D>();
        //���������
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

    //����Ƿ��ڵ���
    void CheckGround()
    {
        isGround = uBox.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    //��ת�ƶ�����
    void Flip()
    {
        //�ж�X�������ٶ�
        bool playerHasXAxisSpeed = Mathf.Abs(uRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            //����������Ҳ���Ҫ��ת
            if (uRigidbody.velocity.x > 0.1f)
            {
                entity.transform.localRotation = Quaternion.Euler(0, 0, 0);
                //˳�㷭ת����
                lookDirection = new Vector2(1, 0);
            }
            //���������ת
            if (uRigidbody.velocity.x < -0.1f)
            {
                entity.transform.localRotation = Quaternion.Euler(0, 180, 0);
                //˳�㷭ת����
                lookDirection = new Vector2(-1, 0);
            }
        }
    }

    //�����ƶ�
    void Run()
    {
        //��ȡ�ƶ�����
        float moveDirection = Input.GetAxis("Horizontal");

        //�ƶ��ٶ�
        Vector2 playerVelocity = new Vector2(moveDirection * runSpeed * speedRate, uRigidbody.velocity.y);
        uRigidbody.velocity = playerVelocity;

        //�ж�X�������ٶ�
        bool playerHasXAxisSpeed = Mathf.Abs(uRigidbody.velocity.x) > Mathf.Epsilon;
        uAnimator.SetBool("Run", playerHasXAxisSpeed);

    }

    //��Ծ
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

    //�л�
    void SwitchAnimation()
    {
        uAnimator.SetBool("Idle", false);
        if (uAnimator.GetBool("Jump"))
        {
            //���ٶ��½������ֵ��ʼ����
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
        //�������ж�
        if (uAnimator.GetBool("DoubleJump"))
        {
            //���ٶ��½������ֵ��ʼ����
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
