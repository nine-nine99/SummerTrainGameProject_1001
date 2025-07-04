using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtController : MonoBehaviour
{
    // 目前的攻击物体
    
    public GameObject attacker = null;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag(gameObject.tag) == false && other.GetComponent<GetDamage>() != null)
        {
            if (other.transform.tag == "Untagged")
            {
                Debug.LogError("Untagged tag is not allowed");
                return;
            }
            attacker = other.gameObject;
            // 获取伤害值
            float damage = other.GetComponent<GetDamage>().GetDamageValue();
            // 获取角色的 IHPController 接口
            IHPController hpController = transform.parent.GetComponent<IHPController>();
            // 受伤
            hpController.Hurt(damage, attacker);

            // if (other.transform.parent.GetComponent<Bullet1001_Controller>() != null)
            // {
            //     // 销毁子弹
            //     Destroy(other.transform.parent.gameObject, 0f);
            // }
            // else
            // {
            //     // 释放敌人物品池
            //     // EnemyCharacterPoolManager.Instance.OnDestroyEnemy(other.transform.parent.gameObject);
            // }
        }
    }
}
