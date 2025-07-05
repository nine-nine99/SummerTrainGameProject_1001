using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_1001 : IState
{
    private FSM_1001 fsm;
    private Rigidbody2D rb;
    private int attackTimes; // 攻击次数，默认为1

    public AttackState_1001(FSM_1001 fsm)
    {
        this.fsm = fsm;
        this.rb = fsm.GetComponent<Rigidbody2D>();
    }

    public void OnEnter()
    {
        // 设置攻击伤害
        fsm.transform.GetChild(3).GetComponent<IGetDamage>().SetDamageValue(fsm.AttackDamage);
        // 设置攻击范围
        fsm.transform.GetChild(3).GetComponent<CircleCollider2D>().radius = fsm.AttackRange;
        // 执行攻击（增加：根据build情况输入不同的攻击次数）
        fsm.StartAttackCoroutine(attackTimes);
        // 设置刚体的速度为0
        rb.velocity = Vector2.zero;
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        // 设置刚体的速度为0
        rb.velocity = Vector2.zero;
    }

    // 设置攻击次数
    public void SetAttackTimes(int times)
    {
        attackTimes = times;
    }
    // 获取攻击次数
    public int GetAttackTimes()
    {
        return attackTimes;
    }
}