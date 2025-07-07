using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopState_1004 : IState
{
    private FSM_1004 fsm;
    public StopState_1004(FSM_1004 fsm)
    {
        this.fsm = fsm;
    }
    public void OnEnter()
    {
        // 进入Stop状态时的逻辑
    }
    public void OnUpdate()
    {
        // Stop状态下的逻辑
    }
    public void OnExit()
    {
        // 离开Stop状态时的逻辑
    }
} 