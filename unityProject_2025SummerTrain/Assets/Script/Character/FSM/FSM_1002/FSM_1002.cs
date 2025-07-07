using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_1002 : MonoBehaviour
{
    private Dictionary<State, IState> states = new Dictionary<State, IState>();
    private IState currentState;
    public Transform currentEnemy => GetTarget();
    private Transform bodySpriteTransform => transform.GetChild(0);
    // 执行参数，链接到具体参数中去
    public float AttackDamage => transform.GetComponent<IParameterController>().GetAttackDamage(); // 攻击伤害
    public float AttackRange => transform.GetComponent<IParameterController>().GetAttackRange(); // 攻击范围
    public float AttackSpeed => transform.GetComponent<IParameterController>().GetAttackRange(); // 攻击速度
    public float Speed => transform.GetComponent<IParameterController>().GetSpeed(); // 角色移动速度
    private void Start()
    {
        states.Add(State.Idle, new IdleState_1002(this));
        states.Add(State.Stop, new StopState_1002(this));

        currentState = states[State.Idle];
        currentState.OnEnter();
    }
    private void Update()
    {
        currentState.OnUpdate();
    }
    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[newState];
        currentState.OnEnter();
    }
    // 获取当前目标，这里的目标是tag为"Enemy"的物体
    public Transform GetTarget()
    {
        List<Collider2D> detectedColliders = transform.GetChild(2).GetComponent<DetectController>().detectedColliders;
        // 查找tag为"player"的物体，且离当前物体最近
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D collider in detectedColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = collider.transform;
                }
            }
        }
        return closestTarget;
    }

    // 旋转角色朝向目标
    public void RotateTowardsTarget(Vector2 direction)
    {
        if (direction.x > 0)
        {
            bodySpriteTransform.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (direction.x < 0)
        {
            bodySpriteTransform.GetComponent<SpriteRenderer>().flipX = true;
        }
    }


    // 当被释放时的回调
    public void OnRelease()
    {
        // 释放时的逻辑
        // 例如，停止所有协程，重置状态等
        StopAllCoroutines();
        currentState.OnExit();
        currentState = states[State.Idle];
        bodySpriteTransform.rotation = Quaternion.Euler(0, 0, 0);
        SpriteRenderer sr = bodySpriteTransform.GetComponent<SpriteRenderer>();
        sr.color = new Color(1f, 1f, 1f, 1f); // 恢复原色
        transform.GetComponent<FSM_1002>().enabled = false; // 禁用FSM组件
        // this.gameObject.SetActive(false);
    }

    public void PlayHitFlash(float flashTime = 0.2f)
    {
        StartCoroutine(HitFlashCoroutine(flashTime));
    }
    private IEnumerator HitFlashCoroutine(float flashTime)
    {
        SpriteRenderer sr = bodySpriteTransform.GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        sr.color = new Color(1f, 0f, 0f, 1f); // 红色
        yield return new WaitForSeconds(flashTime);
        sr.color = new Color(1f, 1f, 1f, 1f);
    }
}
