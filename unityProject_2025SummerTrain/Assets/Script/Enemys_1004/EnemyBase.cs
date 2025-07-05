using UnityEngine;

/// <summary>
/// 敌人基类 - 所有敌人对象的基类
/// 用于测试玩家和玩家族人的攻击
/// </summary>
public abstract class EnemyBase : MonoBehaviour
{
    [Header("敌人基础设置")]
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float currentHealth;
    
    [Header("碰撞检测设置")]
    [SerializeField] protected Collider2D enemyCollider;
    [SerializeField] protected Rigidbody2D enemyRigidbody;
    
    protected bool isDead = false;
    
    protected virtual void Awake()
    {
        // 确保物体有Enemy标签
        if (gameObject.tag != "Enemy")
        {
            gameObject.tag = "Enemy";
            Debug.LogWarning($"物体 {gameObject.name} 的标签已自动设置为 'Enemy'");
        }
        
        // 初始化碰撞体
        InitializeCollider();
        
        // 初始化生命值
        currentHealth = maxHealth;
    }
    
    /// <summary>
    /// 初始化碰撞体组件
    /// </summary>
    protected virtual void InitializeCollider()
    {
        // 检查是否已有碰撞体
        enemyCollider = GetComponent<Collider2D>();
        
        if (enemyCollider == null)
        {
            // 如果没有碰撞体，添加一个BoxCollider2D作为默认碰撞体
            enemyCollider = gameObject.AddComponent<BoxCollider2D>();
            Debug.Log($"为物体 {gameObject.name} 添加了默认的BoxCollider2D");
        }
        
        // 确保碰撞体是启用的
        enemyCollider.enabled = true;
        
        // 检查是否需要Rigidbody2D
        enemyRigidbody = GetComponent<Rigidbody2D>();
        if (enemyRigidbody == null)
        {
            // 添加Rigidbody2D用于物理交互
            enemyRigidbody = gameObject.AddComponent<Rigidbody2D>();
            enemyRigidbody.isKinematic = true; // 默认设置为运动学，避免重力影响
            Debug.Log($"为物体 {gameObject.name} 添加了Rigidbody2D组件");
        }
    }
    
    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="attacker">攻击者</param>
    public virtual void TakeDamage(float damage, GameObject attacker = null)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        
        // 触发受伤事件
        OnDamageTaken(damage, attacker);
        
        // 检查是否死亡
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    /// <summary>
    /// 受伤时的处理
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="attacker">攻击者</param>
    protected virtual void OnDamageTaken(float damage, GameObject attacker)
    {
        Debug.Log($"敌人 {gameObject.name} 受到 {damage} 点伤害，当前生命值: {currentHealth}");
        
        // 可以在这里添加受伤特效、音效等
        // 子类可以重写此方法来实现特定的受伤效果
    }
    
    /// <summary>
    /// 死亡处理
    /// </summary>
    protected virtual void Die()
    {
        if (isDead) return;
        
        isDead = true;
        Debug.Log($"敌人 {gameObject.name} 已死亡");
        
        // 可以在这里添加死亡特效、音效等
        // 子类可以重写此方法来实现特定的死亡效果
        
        // 延迟销毁物体
        Destroy(gameObject, 2f);
    }
    
    /// <summary>
    /// 获取当前生命值
    /// </summary>
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    
    /// <summary>
    /// 获取最大生命值
    /// </summary>
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    
    /// <summary>
    /// 获取生命值百分比
    /// </summary>
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
    
    /// <summary>
    /// 检查是否已死亡
    /// </summary>
    public bool IsDead()
    {
        return isDead;
    }
    
    /// <summary>
    /// 重置敌人状态（用于重新生成）
    /// </summary>
    public virtual void ResetEnemy()
    {
        isDead = false;
        currentHealth = maxHealth;
        gameObject.SetActive(true);
    }
    
    protected virtual void OnValidate()
    {
        // 在编辑器中确保生命值不为负数
        if (maxHealth < 0)
        {
            maxHealth = 0;
        }
    }
} 