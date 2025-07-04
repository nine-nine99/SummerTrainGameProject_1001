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
        if (fsm.currentEnemy != null){
            float distance = Vector2.Distance(fsm.transform.position, fsm.currentEnemy.position);
            if (distance <= fsm.AttackRange)
            {
                fsm.ChangeState(State.Attack);
            }
            fsm.ChangeState(State.Chase);
        }
        else if (fsm.currentEnemy == null){
            // Debug.Log("IdleState_1001");
            fsm.ChangeState(State.Patrol);
        }
    }
    public void OnExit()
    {
        
    }
}
