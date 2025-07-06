using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPController_1002 : MonoBehaviour, IHPController
{
    private IParameterController parameterController => GetComponent<IParameterController>();
    private FSM_1002 fsm => GetComponent<FSM_1002>();
    // 受伤
    public void Hurt(float damage, GameObject attacker = null)
    {
        // Debug.Log("角色受伤");
        float hp = parameterController.GetHP();
        hp -= (int)damage;
        parameterController.SetHP(hp);

        if (hp <= 0)
        {
            Dead();
            return;
        }
        if (transform.GetComponent<FSM_1002>() != null)
        {
            // fsm.PlayHitAndKnockback(attacker.transform);
        }
    }

    // 死亡
    public void Dead()
    {
        // Debug.Log("角色死亡");
        // Destroy(gameObject);
        if (gameObject.CompareTag("Player"))
        {
            transform.GetComponent<FSM_1002>().OnRelease();
            // 玩家死亡
            // PlayerCharacterPoolManager.Instance.OnDestroySoldier(gameObject);

        }
        else if (gameObject.CompareTag("Enemy"))
        {
            // 敌人死亡
            // EnemyCharacterPoolManager.Instance.OnDestroyEnemy(gameObject);
            transform.GetChild(4).GetComponent<DropController_1001>().OnDeath();
        }
    }
}
