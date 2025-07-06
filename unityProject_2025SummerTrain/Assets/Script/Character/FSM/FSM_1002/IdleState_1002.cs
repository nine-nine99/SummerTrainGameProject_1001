using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_1002 : IState
{
    private FSM_1002 fsm;
    private Rigidbody2D rb;
    // 原点
    private Vector2 origin = new Vector2(0, 0);

    public IdleState_1002(FSM_1002 fsm)
    {
        this.fsm = fsm;
        rb = fsm.GetComponent<Rigidbody2D>();
    }
    public void OnEnter()
    {
        
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        
    }
}
