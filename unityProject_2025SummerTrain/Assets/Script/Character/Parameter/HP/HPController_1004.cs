using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPController_1004 : MonoBehaviour, IHPController
{
    private IParameterController parameterController => GetComponent<IParameterController>();
    private FSM_1004 fsm => GetComponent<FSM_1004>();
    // 受伤
    public void Hurt(float damage, GameObject attacker = null)
    {
        // Debug.Log("角色受伤");
        float hp = parameterController.GetHP();
        hp -= (int)damage;
        parameterController.SetHP(hp);

        if (hp <= 0)
        {
            return;
        }
        if (transform.GetComponent<FSM_1004>() != null)
        {
        }
    }
}
