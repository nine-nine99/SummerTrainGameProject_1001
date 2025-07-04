using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PlayerAttributeDataManager : Singleton<PlayerAttributeDataManager>
{
    [Header("玩家属性数据管理器")]
    public PlayerAttributeData_SO standerdPlayerAttributeData; // 标准玩家属性数据
    public PlayerAttributeData_SO currentPlayerAttributeData;
    // private void Start()
    // {
    //     if (currentPlayerAttributeData != null)
    //     {
    //         currentPlayerAttributeData.AddSummonableSoldier(1001); // 添加默认士兵ID
    //         currentPlayerAttributeData.AddSummonableSoldier(1002); // 添加默认士兵ID
    //         currentPlayerAttributeData.AddSummonableSoldier(1003);

    //         standerdPlayerAttributeData.AddSummonableSoldier(1001); // 添加默认士兵ID
    //         standerdPlayerAttributeData.AddSummonableSoldier(1002); // 添加默认士兵ID
    //         standerdPlayerAttributeData.AddSummonableSoldier(1003);
    //         EditorUtility.SetDirty(standerdPlayerAttributeData); // 标记为已修改
    //         EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
    //     }
    // }

    // 获取当前玩家属性数据
    public PlayerAttributeData_SO GetCurrentPlayerAttributeData()
    {
        return currentPlayerAttributeData;
    }

    // 通过标准玩家属性数据初始化currentPlayerAttributeData
    public void InitializeCurrentPlayerAttributeData()
    {
        if (standerdPlayerAttributeData != null)
        {
            currentPlayerAttributeData = Instantiate(standerdPlayerAttributeData);


            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
        else
        {
            Debug.LogError("Standard Player Attribute Data is not set!");
        }
    }
    // 获取当前魔法值
    public float GetCurrentMP()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.MP;
        }
        return 0;
    }
    // 获取当前最大魔法值
    public float GetCurrentMaxMP()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.MaxMP;
        }
        return 0;
    }
    // 获取当前魔法值恢复速率
    public float GetCurrentMPRegenRate()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.MPRegenRate;
        }
        return 0f;
    }

    // 获取当前经验值
    public float GetCurrentExp()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.exp;
        }
        return 0;
    }
    // 获取当前等级
    public int GetCurrentLevel()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.level;
        }
        return 0;
    }
    // 获取当前召唤冷却时间
    public float GetCurrentSummonCooldownTime()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.summonCooldownTime;
        }
        return 0f;
    }
    // 获取单次召唤时同时选中的最大数量
    public int GetCurrentMaxSummonSlotCount()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.maxSummonSlotCount;
        }
        return 0;
    }
    // 获取每次召唤的间隔时间
    public float GetCurrentSummonInterval()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.singleSummonInterval;
        }
        return 0f;
    }
    // 获取玩家召唤范围
    public float GetCurrentSummonRange()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.summonRange;
        }
        return 0f;
    }
    // 获取当前最大召唤数量
    public int GetCurrentMaxSummonableListLength()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.maxSummonableListLength;
        }
        return 0;
    }
    // 获取当前召唤物列表
    public List<summonSoldierDetail> GetCurrentSummonableSoldierDetailList()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.summonableSoldierDetail_List;
        }
        return new List<summonSoldierDetail>();
    }
    // 获取当前召唤物列表的数量
    public int GetCurrentSummonableSoldierCount()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.summonableSoldierDetail_List.Count;
        }
        return 0;
    }
    // 获取当前召唤物列表的最大长度
    public int GetCurrentSummonableListMaxLength()
    {
        if (currentPlayerAttributeData != null)
        {
            return currentPlayerAttributeData.playerAttribute.maxSummonableListLength;
        }
        return 0;
    }

    // 设置当前魔法值
    public void SetCurrentMP(float mp)
    {
        if (currentPlayerAttributeData != null)
        {
            currentPlayerAttributeData.playerAttribute.MP = mp;
            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
    }
    // 设置当前最大魔法值
    public void SetCurrentMaxMP(float maxMP)
    {
        if (currentPlayerAttributeData != null)
        {
            currentPlayerAttributeData.playerAttribute.MaxMP = maxMP;
            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
    }
    // 设置当前魔法值恢复速率
    public void SetCurrentMPRegenRate(float regenRate)
    {
        if (currentPlayerAttributeData != null)
        {
            currentPlayerAttributeData.playerAttribute.MPRegenRate = regenRate;
            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
    }

    // 设置当前经验值
    public void SetCurrentExp(float exp)
    {
        if (currentPlayerAttributeData != null)
        {
            currentPlayerAttributeData.playerAttribute.exp = exp;
            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
    }
    // 设置当前等级
    public void SetCurrentLevel(int level)
    {
        if (currentPlayerAttributeData != null)
        {
            currentPlayerAttributeData.playerAttribute.level = level;
            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
    }
    // 设置当前召唤冷却时间
    public void SetCurrentSummonCooldownTime(float cooldownTime)
    {
        if (currentPlayerAttributeData != null)
        {
            currentPlayerAttributeData.playerAttribute.summonCooldownTime = cooldownTime;
            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
    }
    // 设置召唤范围
    public void SetCurrentSummonRange(float summonRange)
    {
        if (currentPlayerAttributeData != null)
        {
            currentPlayerAttributeData.playerAttribute.summonRange = summonRange;
            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
    }
    // 设置单次召唤时同时选中的最大数量
    public void SetCurrentMaxSummonSlotCount(int maxCount)
    {
        if (currentPlayerAttributeData != null)
        {
            currentPlayerAttributeData.playerAttribute.maxSummonSlotCount = maxCount;
            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
    }
    // 设置每次召唤的间隔时间
    public void SetCurrentSummonInterval(float interval)
    {
        if (currentPlayerAttributeData != null)
        {
            currentPlayerAttributeData.playerAttribute.singleSummonInterval = interval;
            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
    }
    // 设置当前召唤物列表的最大召唤物数量
    public void SetCurrentMaxSummonableListLength(int maxLength)
    {
        if (currentPlayerAttributeData != null)
        {
            currentPlayerAttributeData.playerAttribute.maxSummonableListLength = maxLength;
            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
    }
    // 添加可召唤的士兵
    public void AddSummonableSoldier(int soldierID)
    {
        if (currentPlayerAttributeData != null)
        {
            currentPlayerAttributeData.AddSummonableSoldier(soldierID);
            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
    }
    // 输入UpGradeID, 添加新的SelectedUpGradeDetails到已选择的升级详情列表
    public void AddSelectedUpGradeDetails(int upGradeID)
    {
        if (currentPlayerAttributeData != null)
        {
            // 检查是否已存在相同的升级ID
            SelectedUpGradeDetails existingDetail = currentPlayerAttributeData.playerAttribute.selectedUpGradeDetails.Find(detail => detail.upGradeID == upGradeID);
            if (existingDetail != null)
            {
                // 如果已存在，增加选择次数
                existingDetail.SelectedCount++;
            }
            else
            {
                // 如果不存在，创建新的SelectedUpGradeDetails并添加到列表
                SelectedUpGradeDetails newDetail = new SelectedUpGradeDetails { upGradeID = upGradeID, SelectedCount = 1 };
                currentPlayerAttributeData.playerAttribute.selectedUpGradeDetails.Add(newDetail);
            }
            EditorUtility.SetDirty(currentPlayerAttributeData); // 标记为已修改
        }
    }

    // 输入UpGradeID, 返回已选择的升级详情数量
    public int GetSelectedUpGradeDetailsCount(int upGradeID)
    {
        if (currentPlayerAttributeData != null)
        {
            SelectedUpGradeDetails detail = currentPlayerAttributeData.playerAttribute.selectedUpGradeDetails.Find(d => d.upGradeID == upGradeID);
            if (detail != null)
            {
                return detail.SelectedCount;
            }
        }
        return 0;
    }
}
