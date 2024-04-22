using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int damage = 1;  
    public int health = 1;
    public static float DAMAGE_INTERVAL = 0.3f;    //判断受伤间隔
    private float lastCollisionTime;

    public SkeletonAnimationHandle animationHandle;
    private SkeletonAnimation skeletonAnimation;

    public void Start()
    {
        skeletonAnimation = animationHandle.skeletonAnimation;
        skeletonAnimation.transform.GetComponent<MeshRenderer>().sortingOrder = (int)OrderInLayer.Player;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private bool IsCanBeDamage(Collider2D collision)
    {
        if (collision.CompareTag(SpecialTag.Weapon.ToString()) && PlayerMgr.Instance.mode == PlayerMode.Attack)
            return true;
        return false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(SpecialTag.Player.ToString()))
        {
            PlayerMgr.Instance.TakeDamage(damage);
        }
        if (IsCanBeDamage(collision))
        {
            lastCollisionTime = Time.time;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (IsCanBeDamage(collision))
        {
            if (Time.time - lastCollisionTime >= DAMAGE_INTERVAL)
            {
                TakeDamage(PlayerMgr.Instance.damage);
                lastCollisionTime = Time.time;
            }
        }
    }
}
