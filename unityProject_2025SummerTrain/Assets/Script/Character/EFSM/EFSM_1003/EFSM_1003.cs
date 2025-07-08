using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFSM_1003 : MonoBehaviour
{
    private Dictionary<State, IState> states = new Dictionary<State, IState>();
    private IState currentState;
    private Transform bodySpriteTransform => transform.GetChild(0);
    public List<Vector2> pathPoints => transform.GetComponent<PathController>().pathPoints; // 路径点列表
    // 执行参数，链接到具体参数中去
    public float AttackDamage => transform.GetComponent<IParameterController>().GetAttackDamage(); // 攻击伤害
    public float AttackRange => transform.GetComponent<IParameterController>().GetAttackRange(); // 攻击范围
    public float AttackSpeed => transform.GetComponent<IParameterController>().GetAttackRange(); // 攻击速度
    public float Speed => transform.GetComponent<IParameterController>().GetSpeed(); // 角色移动速度
    private void Start()
    {
        states.Add(State.Idle, new IdleEState_1003(this));
        states.Add(State.Run, new RunEState_1003(this));
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
    // 旋转角色朝向目标
    public void RotateTowardsTarget(Vector2 direction)
    {
        if (direction.x > 0)
        {
            bodySpriteTransform.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (direction.x < 0)
        {
            bodySpriteTransform.GetComponent<SpriteRenderer>().flipX = false;
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
        transform.GetComponent<EFSM_1001>().enabled = false; // 禁用FSM组件
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

    // 达到最后一个路径点后，执行的逻辑
    public void OnReachEndOfPath()
    {
        // 可以在这里添加到达路径终点后的逻辑
        Debug.Log("已到达路径终点");
        PlayerAttributeDataManager.Instance.SetCurrentPlayerHP(PlayerAttributeDataManager.Instance.GetCurrentPlayerHP() - (int)AttackDamage);
        Destroy(gameObject); // 销毁当前对象
    }
}
