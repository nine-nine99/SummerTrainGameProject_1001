using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_1003 : IState
{
    private FSM_1003 fsm;
    private Rigidbody2D rb;
    // 原点
    private Vector2 origin = new Vector2(0, 0);

    public IdleState_1003(FSM_1003 fsm)
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
