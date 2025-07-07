using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_1004 : MonoBehaviour
{
    private Dictionary<State, IState> states = new Dictionary<State, IState>();
    private IState currentState;
    public Transform currentEnemy => GetTarget();
    public Transform currentTarget; // 当前目标，通常是敌人
    private Transform bodySpriteTransform => transform.GetChild(0);
    // 执行参数，链接到具体参数中去
    public float AttackDamage => transform.GetComponent<IParameterController>().GetAttackDamage(); // 攻击伤害
    public float AttackRange => transform.GetComponent<IParameterController>().GetAttackRange(); // 攻击范围
    public float AttackSpeed => transform.GetComponent<IParameterController>().GetAttackSpeed(); // 攻击速度
    public float Speed => transform.GetComponent<IParameterController>().GetSpeed(); // 角色移动速度
    private void Start()
    {
        states.Add(State.Idle, new IdleState_1004(this));
        states.Add(State.Attack, new AttackState_1004(this));
        // states.Add(State.Stop, new StopState_1004(this));

        currentState = states[State.Idle];
        currentState.OnEnter();
    }
    private void Update()
    {
        currentState.OnUpdate();

        // 每3秒恢复1点生命值
        IParameterController parameterController = GetComponent<IParameterController>();
        if (parameterController != null)
        {
            float hp = parameterController.GetHP();
            // 每3秒恢复1点生命值
            if (Time.time % 3 < 0.1f) // 每3秒触发一次
            {
                hp += 1f; // 恢复1点生命值
            }
            parameterController.SetHP(hp);
        }
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
        // 查找tag为"Enemy"的物体，且离当前物体最近
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D collider in detectedColliders)
        {
            if (collider == null) continue; // 检查是否为null
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
        transform.GetComponent<FSM_1001>().enabled = false; // 禁用FSM组件
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
    // 连击攻击，attackTimes为攻击次数，默认为1
    public void StartAttackCoroutine(int attackTimes = 1, Transform target = null)
    {
        StartCoroutine(AttackCoroutine(attackTimes, target.parent));
    }

    private IEnumerator AttackCoroutine(int attackTimes = 1, Transform target = null)
    {
        Debug.Log($"开始攻击，攻击次数: {attackTimes}, 目标: {target}");
        for (int i = 0; i < attackTimes; i++)
        {
            // 记录初始角度
            float startAngle = bodySpriteTransform.eulerAngles.z;
            // 如果当前目标不为空，追逐目标
            if (target == null)
            {
                ChangeState(State.Idle);
                yield break; // 如果没有目标，直接退出
            }
            Debug.LogWarning("攻击目标为空，无法执行攻击");

            Vector2 direction = (target.position - transform.position).normalized;
            float firstAngle = 0;
            float secondAngle = 0;
            if (direction.x >= 0)
            {
                firstAngle = startAngle + 60f;
                secondAngle = startAngle - 60f;
                // 控制攻击方向
                transform.GetChild(3).transform.localPosition = new Vector3(0.32f, 0.39f, 0);
            }
            else if (direction.x < 0)
            {
                firstAngle = startAngle - 60f;
                secondAngle = startAngle + 60f;
                // 控制攻击方向
                transform.GetChild(3).transform.localPosition = new Vector3(-0.32f, 0.39f, 0);
            }

            float duration1 = 0.4f * AttackSpeed; // 向右缓慢旋转时间
            float duration2 = 0.1f * AttackSpeed; // 向左快速旋转时间
            float duration3 = 0.5f * AttackSpeed; // 缓慢复原时间

            // 1. 缓慢向右旋转
            float t = 0;
            while (t < duration1)
            {
                t += Time.deltaTime;
                float angle = Mathf.LerpAngle(startAngle, firstAngle, t / duration1);
                bodySpriteTransform.rotation = Quaternion.Euler(0, 0, angle);
                yield return null;
            }

            // 2. 快速向左旋转
            t = 0;
            while (t < duration2)
            {
                t += Time.deltaTime;
                float angle = Mathf.LerpAngle(firstAngle, secondAngle, t / duration2);
                bodySpriteTransform.rotation = Quaternion.Euler(0, 0, angle);
                yield return null;
            }
            // 造成伤害
            target.GetChild(1).GetComponent<HurtController>().GetHurt(AttackDamage, transform.gameObject);
            // 3. 缓慢复原
            // 如果是最后一下
            if (i == attackTimes - 1)
            {
                t = 0;
                while (t < duration3)
                {
                    t += Time.deltaTime;
                    float angle = Mathf.LerpAngle(secondAngle, startAngle, t / duration3);
                    bodySpriteTransform.rotation = Quaternion.Euler(0, 0, angle);
                    yield return null;
                }
            }
            else
            {
                bodySpriteTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
            // 如果不是最后一下，稍作间隔
            if (i < attackTimes - 1)
                yield return new WaitForSeconds(0.01f);
        }
        // 确保最终角度精确复原
        bodySpriteTransform.rotation = Quaternion.Euler(0, 0, 0);
        ChangeState(State.Idle);
    }
}