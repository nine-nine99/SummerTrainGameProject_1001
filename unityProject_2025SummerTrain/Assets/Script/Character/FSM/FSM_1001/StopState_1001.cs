using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 停止状态 - 停止该角色的所有行动、动画、速度，停止状态机中的所有协程
/// </summary>
public class StopState_1001 : IState
{
    private FSM_1001 fsm;
    private Rigidbody2D rb;
    private Vector2 originalVelocity; // 保存原始速度
    private bool wasKinematic; // 保存原始运动学状态
    private bool wasGravityEnabled; // 保存原始重力状态

    public StopState_1001(FSM_1001 fsm)
    {
        this.fsm = fsm;
        rb = fsm.GetComponent<Rigidbody2D>();
    }

    public void OnEnter()
    {
        Debug.Log("进入停止状态 - 停止所有行动");
        
        // 停止所有协程
        fsm.StopAllCoroutines();
        
        // 保存并停止物理运动
        if (rb != null)
        {
            originalVelocity = rb.velocity;
            wasKinematic = rb.isKinematic;
            wasGravityEnabled = rb.gravityScale > 0;
            
            // 停止移动
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            
            // 设置为运动学模式，完全停止物理模拟
            rb.isKinematic = true;
            rb.gravityScale = 0f;
        }
        
        // 停止动画（如果有Animator组件）
        Animator animator = fsm.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }
        
        // 停止行走动画效果
        StopWalkBob();
        
        // 重置角色旋转
        fsm.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
    }

    public void OnUpdate()
    {
        // 停止状态下不执行任何更新逻辑
        // 角色完全静止，等待外部指令恢复
    }

    public void OnExit()
    {
        Debug.Log("退出停止状态 - 恢复行动能力");
        
        // 恢复物理运动
        if (rb != null)
        {
            // 恢复原始设置
            rb.isKinematic = wasKinematic;
            rb.gravityScale = wasGravityEnabled ? 1f : 0f;
            
            // 可以选择是否恢复原始速度
            // rb.velocity = originalVelocity;
        }
        
        // 恢复动画
        Animator animator = fsm.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
        }
        
        // 恢复行走动画效果
        ResumeWalkBob();
    }
    
    /// <summary>
    /// 停止行走动画效果
    /// </summary>
    private void StopWalkBob()
    {
        // 重置角色精灵位置到原始位置
        Transform bodySpriteTransform = fsm.transform.GetChild(0);
        Vector3 pos = bodySpriteTransform.localPosition;
        pos.y = 0f; // 重置到原始Y位置
        bodySpriteTransform.localPosition = pos;
    }
    
    /// <summary>
    /// 恢复行走动画效果
    /// </summary>
    private void ResumeWalkBob()
    {
        // 这里可以重新初始化行走动画的计时器
        // 具体实现取决于FSM_1001中walkBobTimer的管理方式
    }
} 