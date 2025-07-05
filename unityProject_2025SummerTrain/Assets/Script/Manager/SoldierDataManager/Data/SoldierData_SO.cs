using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoldierData", menuName = "ScriptableObjects/SoldierData")]
public class SoldierData_SO : ScriptableObject
{
    public List<SoldierDetail> soldierDetailsList;
}

[System.Serializable]
public class SoldierDetail
{
    public string soldierName;
    public int soldierID;
    public Sprite soldierIcon;
    public GameObject soldierPrefab;
    public Parameter baseParameter; // 原始参数
}

[System.Serializable]
public class Parameter
{
    public float HP; // 生命值
    public float Speed; // 移动速度
    public float AttackDamage; // 攻击力
    public float AttackSpeed; // 攻击速度
    public float AttackRange; // 攻击范围
    public float CoolDownTime; // 冷却时间
    public float Cost; // 召唤消耗


    // 拷贝构造函数
    public Parameter(Parameter other)
    {
        HP = other.HP;
        AttackRange = other.AttackRange;
        AttackSpeed = other.AttackSpeed;
        AttackDamage = other.AttackDamage;
        Speed = other.Speed;
        CoolDownTime = other.CoolDownTime;
        Cost = other.Cost;
    }
}