using UnityEngine;

/// <summary>
/// 靶子敌人 - 用于测试玩家和玩家族人的攻击
/// 带有"Enemy"标签的物体，可以被碰撞检测到
/// </summary>
public class TargetDummy : EnemyBase
{
    [Header("靶子特殊设置")]
    [SerializeField] private bool showDamageNumbers = true;
    [SerializeField] private Color damageTextColor = Color.red;
    [SerializeField] private float damageTextDuration = 2f;
    [SerializeField] private Vector3 damageTextOffset = new Vector3(0, 2f, 0);
    
    [Header("靶子视觉效果")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitColorDuration = 0.2f;
    
    private Material originalMaterial;
    private Color originalColor;
    
    protected override void Awake()
    {
        base.Awake();
        
        // 获取渲染器组件
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
        
        if (targetRenderer != null)
        {
            // 保存原始材质和颜色
            originalMaterial = targetRenderer.material;
            originalColor = originalMaterial.color;
        }
        
        // 设置靶子的特殊属性
        SetupTargetDummy();
    }
    
    /// <summary>
    /// 设置靶子的特殊属性
    /// </summary>
    private void SetupTargetDummy()
    {
        // 确保靶子有合适的碰撞体大小
        if (enemyCollider is BoxCollider2D boxCollider)
        {
            // 设置一个合适的碰撞体大小
            boxCollider.size = new Vector2(1f, 2f);
            boxCollider.offset = Vector2.zero;
        }
        
        // 设置Rigidbody2D属性
        if (enemyRigidbody != null)
        {
            enemyRigidbody.isKinematic = true; // 靶子不应该移动
            enemyRigidbody.gravityScale = 0f; // 不受重力影响
        }
        
        // 设置较高的生命值，方便测试
        maxHealth = 1000f;
        currentHealth = maxHealth;
    }
    
    /// <summary>
    /// 重写受伤处理，添加靶子特有的视觉效果
    /// </summary>
    protected override void OnDamageTaken(float damage, GameObject attacker)
    {
        base.OnDamageTaken(damage, attacker);
        
        // 显示伤害数字
        if (showDamageNumbers)
        {
            ShowDamageNumber(damage);
        }
        
        // 改变颜色表示被击中
        if (targetRenderer != null)
        {
            StartCoroutine(FlashHitColor());
        }
        
        // 可以在这里添加击中音效
        // AudioManager.Instance.PlaySound("target_hit");
    }
    
    /// <summary>
    /// 显示伤害数字
    /// </summary>
    private void ShowDamageNumber(float damage)
    {
        // 创建伤害数字文本
        GameObject damageText = new GameObject("DamageText");
        damageText.transform.position = transform.position + damageTextOffset;
        
        // 添加TextMesh组件
        TextMesh textMesh = damageText.AddComponent<TextMesh>();
        textMesh.text = damage.ToString("F0");
        textMesh.fontSize = 20;
        textMesh.color = damageTextColor;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
        
        // 添加动画效果
        StartCoroutine(AnimateDamageText(damageText));
    }
    
    /// <summary>
    /// 伤害数字动画
    /// </summary>
    private System.Collections.IEnumerator AnimateDamageText(GameObject damageText)
    {
        Vector3 startPosition = damageText.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * 1f;
        float elapsedTime = 0f;
        
        while (elapsedTime < damageTextDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / damageTextDuration;
            
            // 向上移动
            damageText.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            
            // 淡出效果
            TextMesh textMesh = damageText.GetComponent<TextMesh>();
            if (textMesh != null)
            {
                Color color = textMesh.color;
                color.a = 1f - progress;
                textMesh.color = color;
            }
            
            yield return null;
        }
        
        // 销毁伤害数字
        Destroy(damageText);
    }
    
    /// <summary>
    /// 击中时的颜色闪烁效果
    /// </summary>
    private System.Collections.IEnumerator FlashHitColor()
    {
        if (targetRenderer == null || originalMaterial == null) yield break;
        
        // 变为击中颜色
        originalMaterial.color = hitColor;
        
        yield return new WaitForSeconds(hitColorDuration);
        
        // 恢复原始颜色
        originalMaterial.color = originalColor;
    }
    
    /// <summary>
    /// 重写死亡处理，靶子死亡时可能有特殊效果
    /// </summary>
    protected override void Die()
    {
        // 靶子死亡时的特殊效果
        if (targetRenderer != null)
        {
            originalMaterial.color = Color.gray; // 变为灰色表示死亡
        }
        
        // 可以播放死亡音效
        // AudioManager.Instance.PlaySound("target_destroyed");
        
        base.Die();
    }
    
    /// <summary>
    /// 重置靶子状态
    /// </summary>
    public override void ResetEnemy()
    {
        base.ResetEnemy();
        
        // 恢复原始颜色
        if (targetRenderer != null && originalMaterial != null)
        {
            originalMaterial.color = originalColor;
        }
    }
    
    /// <summary>
    /// 在编辑器中验证设置
    /// </summary>
    protected override void OnValidate()
    {
        base.OnValidate();
        
        // 确保伤害文本持续时间不为负数
        if (damageTextDuration < 0)
        {
            damageTextDuration = 0;
        }
        
        // 确保击中颜色持续时间不为负数
        if (hitColorDuration < 0)
        {
            hitColorDuration = 0;
        }
    }
    
    /// <summary>
    /// 用于调试的方法 - 在编辑器中测试伤害
    /// </summary>
    [ContextMenu("测试伤害 (10点)")]
    private void TestDamage()
    {
        TakeDamage(10f);
    }
    
    /// <summary>
    /// 用于调试的方法 - 在编辑器中测试死亡
    /// </summary>
    [ContextMenu("测试死亡")]
    private void TestDeath()
    {
        TakeDamage(currentHealth);
    }
} 