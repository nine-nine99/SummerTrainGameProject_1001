using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击范围检测器 - 检测范围内带有Enemy标签的物体
/// </summary>
public class AttackRangeDetector : MonoBehaviour
{
    [Header("检测设置")]
    [SerializeField] private string targetTag = "Enemy";
    [SerializeField] private bool showDebugInfo = true;
    
    [Header("检测结果")]
    [SerializeField] private List<GameObject> enemiesInRange = new List<GameObject>();
    
    private Collider2D attackCollider;
    
    private void Start()
    {
        // 获取当前物体的Collider2D组件
        attackCollider = GetComponent<Collider2D>();
        
        if (attackCollider == null)
        {
            Debug.LogError("AttackRangeDetector: 未找到Collider2D组件！请确保此脚本附加的物体有Collider2D组件");
            enabled = false;
            return;
        }
        
        // 确保Collider2D是触发器
        if (!attackCollider.isTrigger)
        {
            attackCollider.isTrigger = true;
            Debug.Log("AttackRangeDetector: 已将Collider2D设置为触发器");
        }
        
        if (showDebugInfo)
        {
            Debug.Log($"AttackRangeDetector: 攻击范围检测器已初始化，目标标签: {targetTag}");
        }
    }
    
    /// <summary>
    /// 当物体进入触发器范围时调用
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查进入的物体是否带有目标标签
        if (other.CompareTag(targetTag))
        {
            // 避免重复添加
            if (!enemiesInRange.Contains(other.gameObject))
            {
                enemiesInRange.Add(other.gameObject);
                
                if (showDebugInfo)
                {
                    Debug.Log($"AttackRangeDetector: 检测到敌人进入攻击范围: {other.name}");
                }
            }
        }
    }
    
    /// <summary>
    /// 当物体离开触发器范围时调用
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        // 检查离开的物体是否带有目标标签
        if (other.CompareTag(targetTag))
        {
            // 从列表中移除
            if (enemiesInRange.Contains(other.gameObject))
            {
                enemiesInRange.Remove(other.gameObject);
                
                if (showDebugInfo)
                {
                    Debug.Log($"AttackRangeDetector: 敌人离开攻击范围: {other.name}");
                }
            }
        }
    }
    
    /// <summary>
    /// 获取攻击范围内的所有敌人
    /// </summary>
    /// <returns>敌人GameObject列表</returns>
    public List<GameObject> GetEnemiesInRange()
    {
        // 清理已销毁的物体引用
        enemiesInRange.RemoveAll(enemy => enemy == null);
        return new List<GameObject>(enemiesInRange);
    }
    
    /// <summary>
    /// 获取攻击范围内的敌人数量
    /// </summary>
    /// <returns>敌人数量</returns>
    public int GetEnemyCount()
    {
        // 清理已销毁的物体引用
        enemiesInRange.RemoveAll(enemy => enemy == null);
        return enemiesInRange.Count;
    }
    
    /// <summary>
    /// 检查是否有敌人在攻击范围内
    /// </summary>
    /// <returns>是否有敌人</returns>
    public bool HasEnemiesInRange()
    {
        // 清理已销毁的物体引用
        enemiesInRange.RemoveAll(enemy => enemy == null);
        return enemiesInRange.Count > 0;
    }
    
    /// <summary>
    /// 获取最近的敌人
    /// </summary>
    /// <returns>最近的敌人GameObject，如果没有则返回null</returns>
    public GameObject GetNearestEnemy()
    {
        // 清理已销毁的物体引用
        enemiesInRange.RemoveAll(enemy => enemy == null);
        
        if (enemiesInRange.Count == 0)
            return null;
        
        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;
        
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }
        
        return nearestEnemy;
    }
    
    /// <summary>
    /// 获取指定范围内的所有敌人
    /// </summary>
    /// <param name="maxDistance">最大距离</param>
    /// <returns>指定范围内的敌人列表</returns>
    public List<GameObject> GetEnemiesWithinDistance(float maxDistance)
    {
        List<GameObject> enemiesWithinRange = new List<GameObject>();
        
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance <= maxDistance)
                {
                    enemiesWithinRange.Add(enemy);
                }
            }
        }
        
        return enemiesWithinRange;
    }
    
    /// <summary>
    /// 清空检测列表
    /// </summary>
    public void ClearDetectionList()
    {
        enemiesInRange.Clear();
        if (showDebugInfo)
        {
            Debug.Log("AttackRangeDetector: 已清空检测列表");
        }
    }
    
    /// <summary>
    /// 设置目标标签
    /// </summary>
    /// <param name="newTag">新的目标标签</param>
    public void SetTargetTag(string newTag)
    {
        targetTag = newTag;
        if (showDebugInfo)
        {
            Debug.Log($"AttackRangeDetector: 目标标签已更改为: {targetTag}");
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (attackCollider != null)
        {
            // 绘制攻击范围
            Gizmos.color = Color.red;
            
            if (attackCollider is BoxCollider2D boxCollider)
            {
                Vector3 size = new Vector3(boxCollider.size.x, boxCollider.size.y, 0.1f);
                Gizmos.DrawWireCube(transform.position, size);
            }
            else if (attackCollider is CircleCollider2D circleCollider)
            {
                Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
            }
        }
        
        // 绘制检测到的敌人
        Gizmos.color = Color.yellow;
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                Gizmos.DrawLine(transform.position, enemy.transform.position);
                Gizmos.DrawWireSphere(enemy.transform.position, 0.5f);
            }
        }
    }
    
    private void OnGUI()
    {
        if (showDebugInfo)
        {
            GUI.Label(new Rect(10, 130, 300, 20), $"攻击范围内敌人数量: {GetEnemyCount()}");
            if (HasEnemiesInRange())
            {
                GUI.Label(new Rect(10, 150, 300, 20), "按空格键查看详细信息");
            }
        }
    }
} 