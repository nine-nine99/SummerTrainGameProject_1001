using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEState_1003 : IState
{
    private EFSM_1003 fsm;
    private Rigidbody2D rb;
    // 原点
    private Vector2 origin = new Vector2(0, 0);

    public IdleEState_1003(EFSM_1003 fsm)
    {
        this.fsm = fsm;
        rb = fsm.GetComponent<Rigidbody2D>();
    }
    public void OnEnter()
    {
        fsm.ChangeState(State.Run); // 进入空闲状态时，切换到Run状态
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        
    }
}
