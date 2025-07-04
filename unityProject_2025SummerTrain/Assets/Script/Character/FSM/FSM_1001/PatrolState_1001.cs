using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PatrolState_1001 : IState
{
    public FSM_1001 fsm;
    private Rigidbody2D rb;
    private Vector2 soldierPos;
    private float speed => fsm.Speed;
    private bool isWaiting = false;
    private Vector2 randomPos;
    // private Vector2 MainCharacterPosition => MCController.Instance.GetCurrentMCPosition(); // 主角位置
    public PatrolState_1001(FSM_1001 fsm)
    {
        this.fsm = fsm;
        rb = fsm.GetComponent<Rigidbody2D>();
    }

    public void OnEnter()
    {
        // 进入巡逻状态时的初始化逻辑
        isWaiting = false;
        randomPos = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        randomPos += (Vector2)fsm.transform.position; // 将随机位置偏移到当前角色位置附近
        rb.velocity = Vector2.zero; // 确保刚体速度为0
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        // 离开巡逻状态时的清理逻辑
        isWaiting = false;
        if (rb == null) return;
        rb.velocity = Vector2.zero; // 停止移动
    }

    private async void WaitAndChangeState(float waitTime)
    {
        isWaiting = true;
        await Task.Delay((int)(waitTime * 1000)); // 等待指定时间（毫秒）
        isWaiting = false;
        fsm.ChangeState(State.Idle);
    }
}
