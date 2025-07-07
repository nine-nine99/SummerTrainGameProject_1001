using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHPController_1001 : MonoBehaviour, IHPController
{
    private IParameterController parameterController => GetComponent<IParameterController>();
    private EFSM_1001 fsm => GetComponent<EFSM_1001>();
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
        if (transform.GetComponent<EFSM_1001>() != null)
        {
            // fsm.PlayHitAndKnockback(attacker.transform);
        }
    }

    // 死亡
    public void Dead()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            // 敌人死亡
            // EnemyCharacterPoolManager.Instance.OnDestroyEnemy(gameObject);
            transform.GetChild(4).GetComponent<DropController_1001>().OnDeath();
        }
    }
}
