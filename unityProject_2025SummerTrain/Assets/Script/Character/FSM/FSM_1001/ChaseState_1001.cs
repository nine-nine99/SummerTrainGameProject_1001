using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState_1001 : IState
{
    private FSM_1001 fsm;
    private Rigidbody2D rb;
    private Transform bodySpriteTransform => fsm.transform.GetChild(0);
    private float speed => fsm.Speed; // 获取速度
    private float currentSpeed = 0;
    private float speedIndex = 1.5f; // 追逐速度系数

    public ChaseState_1001(FSM_1001 fsm)
    {
        this.fsm = fsm;
        rb = fsm.GetComponent<Rigidbody2D>();
    }
    public void OnEnter()
    {
        // 设置刚体的速度为0
        rb.velocity = Vector2.zero;
        // 随机速度
        currentSpeed = Random.Range(speed - 0.2f, speed + 0.2f);
    }
    public void OnUpdate()
    {
        if (fsm.currentEnemy != null)
        {
            // 如果当前目标不为空，追逐目标
            Vector2 direction = (fsm.currentEnemy.position - fsm.transform.position).normalized;
            rb.velocity = direction * currentSpeed * speedIndex; // 速度为
            fsm.RotateTowardsTarget(direction); // 旋转角色朝向目标
            fsm.PlayWalkBob(0.03f, 40f); // 播放行走动画
            float distance = Vector2.Distance(fsm.transform.position, fsm.currentEnemy.position);
            if (distance <= fsm.AttackRange)
            {
                // 如果距离小于攻击距离，切换到攻击状态
                fsm.ChangeState(State.Attack);
            }
        }
        else
        {
            // 如果当前目标为空，切换到Idle状态
            fsm.ChangeState(State.Idle);
        }
    }
    public void OnExit()
    {
        // 停止移动
        rb.velocity = Vector2.zero;
    }
}
