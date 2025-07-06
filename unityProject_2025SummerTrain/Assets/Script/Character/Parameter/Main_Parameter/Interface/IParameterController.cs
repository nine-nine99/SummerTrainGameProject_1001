using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParameterController
{
    // 获取当前ID
    public int GetID();
    // 获取当前参数
    public float GetHP();
    // 设置当前参数
    public void SetHP(float hp);
    public float GetSpeed();
    // 设置移动速度
    public void SetSpeed(float speed);
    public float GetAttackRange();
    // 设置攻击范围
    public void SetAttackRange(float attackRange);
    public float GetAttackSpeed();
    // 设置攻击速度
    public void SetAttackSpeed(float attackSpeed);
    public float GetAttackDamage();
    // 设置攻击伤害
    public void SetAttackDamage(float attackDamage);

    public Parameter GetCurrentParameter();

    // 初始化
    public void Init(int ID);
}
