using UnityEngine;

/// <summary>
/// FSM状态使用示例 - 展示如何使用Idle和Stop状态
/// </summary>
public class FSMStateExample : MonoBehaviour
{
    [Header("FSM引用")]
    [SerializeField] private FSM_1001 fsm;
    
    [Header("测试设置")]
    [SerializeField] private KeyCode stopKey = KeyCode.S;
    [SerializeField] private KeyCode resumeKey = KeyCode.R;
    [SerializeField] private KeyCode idleKey = KeyCode.I;
    [SerializeField] private KeyCode attackKey = KeyCode.A; // 攻击键
    [SerializeField] private KeyCode debugKey = KeyCode.Space; // 调试键
    
    [Header("攻击范围检测")]
    [SerializeField] private AttackRangeDetector attackRangeDetector;
    
    private Vector3 offset;
    private bool isDragging = false;

    private void Start()
    {
        // 如果没有指定FSM，尝试从当前物体获取
        if (fsm == null)
        {
            fsm = GetComponent<FSM_1001>();
        }
        
        if (fsm == null)
        {
            Debug.LogError("未找到FSM_1001组件！");
            enabled = false;
            return;
        }
        
        // 确保当前物体带有"Player"标签
        if (gameObject.tag != "Player")
        {
            gameObject.tag = "Player";
            Debug.Log("已将物体标签设置为Player");
        }
        
        // 初始化攻击范围检测器（暂时注释，等Unity编译完成后启用）
        // InitializeAttackRangeDetector();
        
        if (attackRangeDetector == null)
        {
            attackRangeDetector = GetComponentInChildren<AttackRangeDetector>();
        }
    }
    
    private void Update()
    {
        // 检查FSM是否可用
        if (fsm == null)
        {
            Debug.LogWarning("FSM组件为空，无法切换状态");
            return;
        }
        
        // 按S键停止所有行动
        if (Input.GetKeyDown(stopKey))
        {
            Debug.Log($"按下 {stopKey} 键，尝试停止角色");
            StopCharacter();
        }
        
        // 按R键恢复行动
        if (Input.GetKeyDown(resumeKey))
        {
            Debug.Log($"按下 {resumeKey} 键，尝试恢复角色");
            ResumeCharacter();
        }
        
        // 按I键切换到待机状态
        if (Input.GetKeyDown(idleKey))
        {
            Debug.Log($"按下 {idleKey} 键，尝试切换到待机状态");
            GoToIdle();
        }
        
        // 按A键切换到攻击状态
        if (Input.GetKeyDown(attackKey))
        {
            Debug.Log($"按下 {attackKey} 键，尝试切换到攻击状态");
            GoToAttack();
        }
        
        // 按空格键显示攻击范围调试信息（暂时注释，等Unity编译完成后启用）
        
        if (Input.GetKeyDown(debugKey))
        {
            ShowAttackRangeDebugInfo();
        }
        

        // 鼠标拖拽移动功能
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = transform.position.z;
            // 检查主物体和所有子物体的Collider2D
            Collider2D[] cols = GetComponentsInChildren<Collider2D>();
            foreach (var col in cols)
            {
                if (col != null && col.OverlapPoint(mouseWorldPos))
                {
                    isDragging = true;
                    offset = transform.position - mouseWorldPos;
                    break;
                }
            }
        }
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = transform.position.z;
            transform.position = mouseWorldPos + offset;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
    
    /// <summary>
    /// 停止角色所有行动
    /// </summary>
    public void StopCharacter()
    {
        if (fsm != null)
        {
            fsm.StopAllActions();
            Debug.Log("角色已停止所有行动");
        }
    }
    
    /// <summary>
    /// 恢复角色行动能力
    /// </summary>
    public void ResumeCharacter()
    {
        if (fsm != null)
        {
            fsm.ResumeActions();
            Debug.Log("角色已恢复行动能力");
        }
    }
    
    /// <summary>
    /// 切换到待机状态
    /// </summary>
    public void GoToIdle()
    {
        if (fsm != null)
        {
            try
            {
                fsm.ChangeState(State.Idle);
                Debug.Log("角色已切换到待机状态");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"切换到待机状态失败: {e.Message}");
            }
        }
        else
        {
            Debug.LogError("FSM组件为空，无法切换状态");
        }
    }
    
    /// <summary>
    /// 切换到攻击状态
    /// </summary>
    public void GoToAttack()
    {
        if (fsm != null)
        {
            try
            {
                fsm.ChangeState(State.Attack);
                Debug.Log("角色已切换到攻击状态");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"切换到攻击状态失败: {e.Message}");
            }
        }
        else
        {
            Debug.LogError("FSM组件为空，无法切换状态");
        }
    }
    
    /// <summary>
    /// 检查角色是否处于停止状态
    /// </summary>
    public bool IsCharacterStopped()
    {
        return fsm != null && fsm.IsStopped();
    }
    
    /// <summary>
    /// 获取当前状态名称
    /// </summary>
    public string GetCurrentStateName()
    {
        if (fsm == null) return "FSM为空";
        
        try
        {
            if (fsm.IsStopped()) return "停止";
            
            // 通过反射获取当前状态名称
            var currentStateField = typeof(FSM_1001).GetField("currentState", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (currentStateField != null)
            {
                var currentState = currentStateField.GetValue(fsm);
                if (currentState != null)
                {
                    return currentState.GetType().Name;
                }
            }
            
            return "未知状态";
        }
        catch
        {
            return "状态获取失败";
        }
    }
    
    /// <summary>
    /// 在Inspector中测试停止功能
    /// </summary>
    [ContextMenu("测试停止角色")]
    public void TestStopCharacter()
    {
        StopCharacter();
    }
    
    /// <summary>
    /// 在Inspector中测试恢复功能
    /// </summary>
    [ContextMenu("测试恢复角色")]
    public void TestResumeCharacter()
    {
        ResumeCharacter();
    }
    
    /// <summary>
    /// 在Inspector中测试待机功能
    /// </summary>
    [ContextMenu("测试待机状态")]
    public void TestIdleState()
    {
        GoToIdle();
    }
    
    /// <summary>
    /// 在Inspector中测试攻击功能
    /// </summary>
    [ContextMenu("测试攻击状态")]
    public void TestAttackState()
    {
        GoToAttack();
    }
    
    /// <summary>
    /// 刷新FSM引用
    /// </summary>
    [ContextMenu("刷新FSM引用")]
    public void RefreshFSMReference()
    {
        fsm = GetComponent<FSM_1001>();
        if (fsm != null)
        {
            Debug.Log("FSM引用刷新成功");
        }
        else
        {
            Debug.LogError("未找到FSM_1001组件，请确保物体上有FSM_1001脚本");
        }
    }
    
    private void OnGUI()
    {
        // 显示当前状态信息
        if (fsm != null)
        {
            string currentState = GetCurrentStateName();
            GUI.Label(new Rect(10, 10, 300, 20), $"当前状态: {currentState}");
            GUI.Label(new Rect(10, 30, 300, 20), $"按 {stopKey} 停止角色");
            GUI.Label(new Rect(10, 50, 300, 20), $"按 {resumeKey} 恢复角色");
            GUI.Label(new Rect(10, 70, 300, 20), $"按 {idleKey} 切换到待机");
            GUI.Label(new Rect(10, 90, 300, 20), $"按 {attackKey} 切换到攻击");
            GUI.Label(new Rect(10, 110, 300, 20), "状态切换调试信息请查看Console");
            
            // 显示攻击范围信息（暂时注释，等Unity编译完成后启用）
            
            if (attackRangeDetector != null)
            {
                GUI.Label(new Rect(10, 130, 300, 20), $"攻击范围内敌人: {attackRangeDetector.GetEnemyCount()} 个");
                GUI.Label(new Rect(10, 150, 300, 20), $"按 {debugKey} 查看详细信息");
            }
            
        }
        else
        {
            GUI.Label(new Rect(10, 10, 300, 20), "FSM组件未找到！");
        }
    }
    
    /// <summary>
    /// 初始化攻击范围检测器（暂时注释，等Unity编译完成后启用）
    /// </summary>
    
    private void InitializeAttackRangeDetector()
    {
        // 如果没有指定攻击范围检测器，尝试从子物体中查找
        if (attackRangeDetector == null)
        {
            attackRangeDetector = GetComponentInChildren<AttackRangeDetector>();
        }
        
        if (attackRangeDetector == null)
        {
            Debug.LogWarning("未找到AttackRangeDetector组件！请确保子物体Attach上有AttackRangeDetector脚本");
        }
        else
        {
            Debug.Log("攻击范围检测器初始化成功");
        }
    }
    
    /// <summary>
    /// 显示攻击范围调试信息
    /// </summary>
    private void ShowAttackRangeDebugInfo()
    {
        if (attackRangeDetector == null)
        {
            Debug.LogWarning("攻击范围检测器未找到！");
            return;
        }
        
        var enemies = attackRangeDetector.GetEnemiesInRange();
        Debug.Log($"=== 攻击范围调试信息 ===");
        Debug.Log($"攻击范围内敌人数量: {enemies.Count}");
        
        if (enemies.Count > 0)
        {
            Debug.Log("攻击范围内的敌人:");
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null)
                {
                    float distance = Vector2.Distance(transform.position, enemies[i].transform.position);
                    Debug.Log($"  {i + 1}. {enemies[i].name} (距离: {distance:F2})");
                }
            }
            
            GameObject nearestEnemy = attackRangeDetector.GetNearestEnemy();
            if (nearestEnemy != null)
            {
                Debug.Log($"最近的敌人: {nearestEnemy.name}");
            }
        }
        else
        {
            Debug.Log("攻击范围内没有敌人");
        }
        Debug.Log("========================");
    }
    
    /// <summary>
    /// 获取攻击范围检测器
    /// </summary>
    /// <returns>攻击范围检测器组件</returns>
    public AttackRangeDetector GetAttackRangeDetector()
    {
        return attackRangeDetector;
    }
    
    /// <summary>
    /// 检查是否有敌人在攻击范围内
    /// </summary>
    /// <returns>是否有敌人</returns>
    public bool HasEnemiesInAttackRange()
    {
        return attackRangeDetector != null && attackRangeDetector.HasEnemiesInRange();
    }
    
    /// <summary>
    /// 获取攻击范围内的敌人数量
    /// </summary>
    /// <returns>敌人数量</returns>
    public int GetEnemyCountInAttackRange()
    {
        return attackRangeDetector != null ? attackRangeDetector.GetEnemyCount() : 0;
    }
    
    /// <summary>
    /// 获取最近的敌人
    /// </summary>
    /// <returns>最近的敌人GameObject</returns>
    public GameObject GetNearestEnemy()
    {
        return attackRangeDetector != null ? attackRangeDetector.GetNearestEnemy() : null;
    }
    
} 