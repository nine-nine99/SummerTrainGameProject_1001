using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributeData", menuName = "ScriptableObjects/PlayerAttributeData")]
public class PlayerAttributeData_SO : ScriptableObject
{
    public PlayerAttribute playerAttribute;

    // 添加一个新的士兵到可召唤物列表
    public void AddSummonableSoldier(int soldierID)
    {
        // 如果playerAttribute.summonableList的长度超过最大召唤数量，则不添加
        if (playerAttribute.summonableSoldierDetail_List.Count >= playerAttribute.maxSummonableListLength)
        {
            return;
        }

        if (playerAttribute.summonableSoldierDetail_List.Find(s => s.soldierID == soldierID) == null)
        {
            summonSoldierDetail newSoldier = new summonSoldierDetail(soldierID);
            playerAttribute.summonableSoldierDetail_List.Add(newSoldier);
        }
    }

    // 输入一个士兵ID，返回对应的士兵原始数据
    public SoldierDetail GetBaseSoldierDetailByID(int soldierID)
    {
        if (playerAttribute.summonableSoldierDetail_List == null || playerAttribute.summonableSoldierDetail_List.Count == 0)
        {
            return null;
        }

        summonSoldierDetail soldier = playerAttribute.summonableSoldierDetail_List.Find(s => s.soldierID == soldierID);
        if (soldier != null)
        {
            return soldier.baseDetail;
        }
        else
        {
            return null;
        }
    }
    // 输入一个士兵ID，返回对应的士兵当前数据
    public SoldierDetail GetCurrentSoldierDetailByID(int soldierID)
    {
        if (playerAttribute.summonableSoldierDetail_List == null || playerAttribute.summonableSoldierDetail_List.Count == 0)
        {
            return null;
        }

        summonSoldierDetail soldier = playerAttribute.summonableSoldierDetail_List.Find(s => s.soldierID == soldierID);
        if (soldier != null)
        {
            return soldier.currentDetail;
        }
        else
        {
            return null;
        }
    }
}

[System.Serializable]
public class PlayerAttribute
{
    [Header("玩家等级")]
    public int level = 1;
    [Header("经验")]
    public float exp = 0;
    public float MP; // 玩家魔法值
    public float MaxMP; // 玩家最大魔法值
    public float MPRegenRate = 0.5f; // 玩家魔法值每秒恢复速度
    [Header("召唤冷却时间")]
    public float summonCooldownTime = 0.5f;
    [Header("单次召唤时同时选中的最大数量")]
    public int maxSummonSlotCount = 1;
    [Header("每次召唤的间隔时间")]
    public float singleSummonInterval = 1f;
    [Header("玩家召唤范围")]
    public float summonRange = 3f; // 玩家召唤范围  
    [Header("玩家召唤列表长度限制")]
    public int maxSummonableListLength = 12;
    [Header("玩家主角性能参数")]
    public Parameter playerParameter;
    [Header("可召唤士兵列表")]
    public List<summonSoldierDetail> summonableSoldierDetail_List = new List<summonSoldierDetail>();
    [Header("已选择的升级详情")]
    public List<SelectedUpGradeDetails> selectedUpGradeDetails = new List<SelectedUpGradeDetails>();

    // 深拷贝构造函数
    public PlayerAttribute(PlayerAttribute other)
    {
        level = other.level;
        exp = other.exp;
        MP = other.MP;
        MaxMP = other.MaxMP;
        MPRegenRate = other.MPRegenRate;
        summonCooldownTime = other.summonCooldownTime;
        maxSummonSlotCount = other.maxSummonSlotCount;
        singleSummonInterval = other.singleSummonInterval;
        summonRange = other.summonRange;
        maxSummonableListLength = other.maxSummonableListLength;
        playerParameter = new Parameter(other.playerParameter); // 深拷贝参数
        summonableSoldierDetail_List = new List<summonSoldierDetail>(other.summonableSoldierDetail_List);
        selectedUpGradeDetails = new List<SelectedUpGradeDetails>(other.selectedUpGradeDetails);
    }
}

[System.Serializable]
public class summonSoldierDetail
{
    public int soldierID; // 士兵ID
    public SoldierDetail baseDetail; // 原始数据, 不会更改
    public SoldierDetail currentDetail; // 当前数据， 会随着游戏流程而变化
    public summonSoldierDetail(int ID)
    {
        soldierID = ID;
        if (SoldierDataManager.Instance == null)
        {
            Debug.LogError("SoldierDataManager is not initialized.");
            return;
        }
        baseDetail = SoldierDataManager.Instance.GetSoldierDetailByID(ID);
        currentDetail = baseDetail;
    }
}

[System.Serializable]
public class SelectedUpGradeDetails
{
    public int upGradeID;
    public int SelectedCount;
}
