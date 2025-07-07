using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEState_1001 : IState
{
    private EFSM_1001 fsm;
    private Rigidbody2D rb;
    
    // 路径相关变量
    private int currentPathIndex = 0;
    private Vector2 currentTarget;
    private float arriveDistance = 0.1f; // 到达目标点的距离阈值

    public RunEState_1001(EFSM_1001 fsm)
    {
        this.fsm = fsm;
        rb = fsm.GetComponent<Rigidbody2D>();
    }
    
    public void OnEnter()
    {
        // 重置路径索引
        currentPathIndex = 0;
        
        // 检查是否有路径点
        if (fsm.pathPoints == null || fsm.pathPoints.Count == 0)
        {
            Debug.LogWarning("没有可用的路径点");
            return;
        }
        
        // 设置第一个目标点
        currentTarget = fsm.pathPoints[currentPathIndex];
        // Debug.Log($"开始沿路径移动，目标: {currentTarget}");
    }
    
    public void OnUpdate()
    {
        // 检查是否有路径点
        if (fsm.pathPoints == null || fsm.pathPoints.Count == 0)
        {
            return;
        }
        
        // 检查是否已经到达最后一个路径点
        if (currentPathIndex >= fsm.pathPoints.Count)
        {
            // 到达终点，停止移动
            rb.velocity = Vector2.zero;
            Debug.Log("已到达路径终点");
            // 这里可以切换到其他状态，比如Idle
            fsm.ChangeState(State.Idle);
            return;
        }
        
        // 计算到当前目标点的距离
        Vector2 currentPosition = rb.transform.position;
        float distanceToTarget = Vector2.Distance(currentPosition, currentTarget);
        
        // 检查是否到达当前目标点
        if (distanceToTarget <= arriveDistance)
        {
            // 移动到下一个路径点
            currentPathIndex++;
            
            if (currentPathIndex < fsm.pathPoints.Count)
            {
                currentTarget = fsm.pathPoints[currentPathIndex];
                // Debug.Log($"到达路径点 {currentPathIndex - 1}，下一个目标: {currentTarget}");
            }
            else
            {
                // 已到达最后一个点
                return;
            }
        }
        
        // 计算移动方向
        Vector2 direction = (currentTarget - currentPosition).normalized;
        
        // 设置速度
        rb.velocity = direction * fsm.Speed;
        
        // 旋转角色朝向目标
        fsm.RotateTowardsTarget(direction);
    }

    public void OnExit()
    {
        // 停止移动
        rb.velocity = Vector2.zero;
        fsm.OnReachEndOfPath();
        Debug.Log("退出Run状态");
    }
}
