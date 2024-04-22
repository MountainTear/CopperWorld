using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 仅作为辅助脚本，告诉敌人主角进入攻击范围
/// </summary>
public class AttackRange : MonoBehaviour
{
    //对应的飞行敌人
    public EnemyFly enemyfly;

    //告诉敌人进入攻击范围
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemyfly.AutoAttack(collision.gameObject.transform);
        }
    }
}
