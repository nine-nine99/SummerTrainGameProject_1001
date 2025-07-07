using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_1001 : IState
{
    private FSM_1001 fsm;
    private Rigidbody2D rb;
    // 原点
    private Vector2 origin = new Vector2(0, 0);

    public IdleState_1001(FSM_1001 fsm)
    {
        this.fsm = fsm;
        rb = fsm.GetComponent<Rigidbody2D>();
    }
    public void OnEnter()
    {
        
    }
    public void OnUpdate()
    {
        Transform obj = fsm.GetTarget();
        if (obj != null)
        {
            float distance = Vector2.Distance(fsm.transform.position, obj.position);
            if (distance <= fsm.AttackRange)
            {
                fsm.currentTarget = obj; // 更新当前目标
                fsm.ChangeState(State.Attack);
            }
        }
    }
    public void OnExit()
    {
        
    }
}
