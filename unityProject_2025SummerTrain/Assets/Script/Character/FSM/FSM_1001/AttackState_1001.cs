using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_1001 : IState
{
    private FSM_1001 fsm;
    private Rigidbody2D rb;
    private int attackTimes; // 攻击次数，默认为1
    
    [Header("子弹设置")]
    [SerializeField] private GameObject bulletPrefab; // 子弹预制体
    [SerializeField] private Transform firePoint; // 发射点
    [SerializeField] private float bulletSpeed = 10f; // 子弹速度
    [SerializeField] private float bulletDamage = 20f; // 子弹伤害
    [SerializeField] private bool useBulletAttack = false; // 是否使用子弹攻击
    
    private float lastFireTime = 0f; // 上次发射时间
    private float fireRate = 0.5f; // 发射频率

    public AttackState_1001(FSM_1001 fsm)
    {
        this.fsm = fsm;
        this.rb = fsm.GetComponent<Rigidbody2D>();
        this.attackTimes = fsm.AttackTimes; // 默认攻击次数为1
    }

    public void OnEnter()
    {
        // 设置攻击伤害
        fsm.transform.GetChild(3).GetComponent<IGetDamage>().SetDamageValue(fsm.AttackDamage);
        // 设置攻击范围
        fsm.transform.GetChild(3).GetComponent<CircleCollider2D>().radius = fsm.AttackRange;
        
        // 根据攻击类型选择攻击方式
        if (useBulletAttack)
        {
            // 使用子弹攻击
            FireBullet();
        }
        else
        {
            // 使用近战攻击
            fsm.StartAttackCoroutine(attackTimes);
        }
        
        // 设置刚体的速度为0
        rb.velocity = Vector2.zero;
    }

    public void OnUpdate()
    {
        // 如果使用子弹攻击，在Update中持续发射
        if (useBulletAttack && fsm.currentEnemy != null)
        {
            // 检查发射冷却
            if (Time.time - lastFireTime >= fireRate)
            {
                FireBullet();
                lastFireTime = Time.time;
            }
        }
    }

    public void OnExit()
    {
        // 设置刚体的速度为0
        rb.velocity = Vector2.zero;
    }

    // 设置攻击次数
    public void SetAttackTimes(int times)
    {
        attackTimes = times;
    }
    // 获取攻击次数
    public int GetAttackTimes()
    {
        return attackTimes;
    }
    
    /// <summary>
    /// 发射子弹
    /// </summary>
    private void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("子弹预制体未设置！");
            return;
        }
        
        // 确定发射点
        Transform firePosition = firePoint != null ? firePoint : fsm.transform;
        
        // 确定发射方向
        Vector2 fireDirection = GetFireDirection();
        
        // 创建子弹
        GameObject bullet = UnityEngine.Object.Instantiate(bulletPrefab, firePosition.position, Quaternion.identity);
        
        // 设置子弹属性
        SetupBullet(bullet, fireDirection);
        
        // 播放发射音效
        PlayFireSound();
        
        Debug.Log($"发射子弹，方向: {fireDirection}");
    }
    
    /// <summary>
    /// 获取发射方向
    /// </summary>
    private Vector2 GetFireDirection()
    {
        // 如果有目标，朝目标方向发射
        if (fsm.currentEnemy != null)
        {
            return (fsm.currentEnemy.position - fsm.transform.position).normalized;
        }
        
        // 否则朝角色朝向发射
        SpriteRenderer spriteRenderer = fsm.transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.flipX)
        {
            return Vector2.left; // 朝左
        }
        else
        {
            return Vector2.right; // 朝右
        }
    }
    
    /// <summary>
    /// 设置子弹属性
    /// </summary>
    private void SetupBullet(GameObject bullet, Vector2 direction)
    {
        // 获取子弹的Rigidbody2D组件
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            // 设置子弹速度
            bulletRb.velocity = direction * bulletSpeed;
        }
        
        // 设置子弹伤害
        IGetDamage bulletDamageComponent = bullet.GetComponent<IGetDamage>();
        if (bulletDamageComponent != null)
        {
            bulletDamageComponent.SetDamageValue(bulletDamage);
        }
        
        // 设置子弹标签
        bullet.tag = fsm.gameObject.tag;
        
        // 设置子弹生命周期
        UnityEngine.Object.Destroy(bullet, 5f); // 5秒后销毁子弹
    }
    
    /// <summary>
    /// 播放发射音效
    /// </summary>
    private void PlayFireSound()
    {
        // 这里可以添加发射音效
        // AudioManager.Instance.PlaySound("fire_sound");
    }
    
    /// <summary>
    /// 设置子弹预制体
    /// </summary>
    public void SetBulletPrefab(GameObject prefab)
    {
        bulletPrefab = prefab;
    }
    
    /// <summary>
    /// 设置发射点
    /// </summary>
    public void SetFirePoint(Transform point)
    {
        firePoint = point;
    }
    
    /// <summary>
    /// 设置子弹速度
    /// </summary>
    public void SetBulletSpeed(float speed)
    {
        bulletSpeed = speed;
    }
    
    /// <summary>
    /// 设置子弹伤害
    /// </summary>
    public void SetBulletDamage(float damage)
    {
        bulletDamage = damage;
    }
    
    /// <summary>
    /// 设置发射频率
    /// </summary>
    public void SetFireRate(float rate)
    {
        fireRate = rate;
    }
    
    /// <summary>
    /// 切换攻击模式
    /// </summary>
    public void SetUseBulletAttack(bool useBullet)
    {
        useBulletAttack = useBullet;
    }
}