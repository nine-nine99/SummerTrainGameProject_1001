using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_1004 : IState
{
    private FSM_1004 fsm;
    public IdleState_1004(FSM_1004 fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        // 进入Idle状态时的逻辑
    }
    public void OnUpdate()
    {
        // Idle状态下的逻辑
    }
    public void OnExit()
    {
        // 离开Idle状态时的逻辑
    }
} 