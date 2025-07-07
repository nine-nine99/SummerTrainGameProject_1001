using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EParameter_1003 : MonoBehaviour, IParameterController
{
    [HideInInspector]
    public int ID;
    // 最开始的属性, 不会被修改的属性
    private Parameter baseOriginParameter;
    // 会在战斗中被修改的参数
    private Parameter currentParameter;
    [HideInInspector]
    // 角色的最大参数
    public float HPMax;
    private void Start()
    {
        ID = 1003; // 假设ID为1003
        // 初始化参数
        Init(ID);
    }

    private void LateUpdate()
    {
        // Init(ID); // 重新初始化参数
    }
    public Parameter GetCurrentParameter()
    {
        return currentParameter;
    }

    // 初始化
    public void Init(int ID)
    {
        this.ID = ID;

        // 获取当前的参数
        // baseOriginParameter = new Parameter(PlayerAttributeDataManager.Instance.currentPlayerAttributeData.GetCurrentSoldierDetailByID(ID).baseParameter); // 深拷贝，防止修改原始参数
        // currentParameter = new Parameter(baseOriginParameter); // 深拷贝，防止修改原始参数
        baseOriginParameter = EnemySoldierDataManager.Instance.GetSoldierDetailByID(ID).baseParameter; // 获取敌人士兵的原始参数
        currentParameter = new Parameter(baseOriginParameter); // 深拷贝，防止修改原始参数
        // 设置当前的执行参数
        SetHP(currentParameter.HP);
        SetAttackRange(currentParameter.AttackRange);
        SetAttackSpeed(currentParameter.AttackSpeed);
        SetAttackDamage(currentParameter.AttackDamage);
        SetSpeed(currentParameter.Speed);

        // 设置角色的最大参数
        HPMax = currentParameter.HP;
    }
    // 获取当前ID
    public int GetID()
    {
        return ID;
    }

    // 获取当前参数
    public float GetHP()
    {
        return currentParameter.HP;
    }
    // 设置当前参数
    public void SetHP(float hp)
    {
        currentParameter.HP = hp;
    }
    // 获取攻击范围
    public float GetAttackRange()
    {
        return currentParameter.AttackRange;
    }
    // 设置攻击范围
    public void SetAttackRange(float attackRange)
    {
        currentParameter.AttackRange = attackRange; // 本来想用属性，但是现在是晚上12点算了。不要在这里补充了 Copilot
    }
    // 获取攻击速度
    public float GetAttackSpeed()
    {
        return currentParameter.AttackSpeed;
    }
    // 设置攻击速度
    public void SetAttackSpeed(float attackSpeed)
    {
        currentParameter.AttackSpeed = attackSpeed;

    }
    // 获取攻击伤害
    public float GetAttackDamage()
    {
        return currentParameter.AttackDamage;
    }
    // 设置攻击伤害
    public void SetAttackDamage(float attackDamage)
    {
        currentParameter.AttackDamage = attackDamage;
    }
    // 获取移动速度
    public float GetSpeed()
    {
        return currentParameter.Speed;
    }
    // 设置移动速度
    public void SetSpeed(float speed)
    {
        currentParameter.Speed = speed;
    }
}
