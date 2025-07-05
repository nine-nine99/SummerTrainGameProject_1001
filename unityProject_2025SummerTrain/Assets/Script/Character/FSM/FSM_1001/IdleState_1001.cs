using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 待机状态 - 角色静止不动，等待指令或检测到敌人
/// </summary>
public class IdleState_1001 : IState
{
    private FSM_1001 fsm;
    private Rigidbody2D rb;
    private float idleTimer = 0f; // 待机计时器
    private const float IDLE_DURATION = 2f; // 待机持续时间

    public IdleState_1001(FSM_1001 fsm)
    {
        this.fsm = fsm;
        rb = fsm.GetComponent<Rigidbody2D>();
    }

    public void OnEnter()
    {
        Debug.Log("进入待机状态");
        idleTimer = 0f;
        
        // 停止移动
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        
        // 重置角色旋转
        fsm.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
    }

    public void OnUpdate()
    {
        // 检测是否有敌人
        if (fsm.currentEnemy != null)
        {
            float distance = Vector2.Distance(fsm.transform.position, fsm.currentEnemy.position);
            
            // 如果在攻击范围内，直接攻击
            if (distance <= fsm.AttackRange)
            {
                fsm.ChangeState(State.Attack);
                return;
            }
            
            // 如果敌人不在攻击范围内，追逐敌人
            fsm.ChangeState(State.Chase);
            return;
        }
        
        // 如果没有敌人，增加待机时间
        idleTimer += Time.deltaTime;
        
        // 待机一段时间后，如果没有敌人，开始巡逻
        if (idleTimer >= IDLE_DURATION)
        {
            fsm.ChangeState(State.Patrol);
        }
    }

    public void OnExit()
    {
        Debug.Log("退出待机状态");
        idleTimer = 0f;
    }
}
