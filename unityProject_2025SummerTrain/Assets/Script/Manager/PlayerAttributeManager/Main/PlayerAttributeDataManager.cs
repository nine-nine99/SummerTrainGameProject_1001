using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PlayerAttributeDataManager : Singleton<PlayerAttributeDataManager>
{
    [Header("玩家属性数据管理器")]
    public PlayerAttributeData_SO standerdPlayerAttributeData; // 标准玩家属性数据
    public PlayerAttributeData_SO currentPlayerAttributeData;

    // 通过 standerdPlayerAttributeData 重置 currentPlayerAttributeData
    public void ResetCurrentPlayerAttributeData()
    {
        if (standerdPlayerAttributeData == null)
        {
            Debug.LogError("标准玩家属性数据未设置，请在编辑器中设置");
            return;
        }

        currentPlayerAttributeData.playerAttribute = new PlayerAttribute(standerdPlayerAttributeData.playerAttribute);
        Debug.Log("当前玩家属性数据已重置为标准数据");
    }

    // 清空玩家属性中所有士兵的疲劳值，把疲劳恢复速度设置为1并把是否在战斗中设置为false
    public void ClearAllSoldierFatigueValues()
    {
        if (currentPlayerAttributeData == null)
        {
            Debug.LogError("当前玩家属性数据未设置，请先初始化");
            return;
        }

        foreach (var soldier in currentPlayerAttributeData.playerAttribute.PlayerSoldierID_List)
        {
            soldier.fatigueValue = 0f; // 清空疲劳值
            soldier.fatigueIncreaseSpeed = 0f; // 清空疲劳增加速度
            soldier.fatigueRecoverSpeed = 1f; // 设置疲劳恢复速度为1
            soldier.isInBattle = false; // 设置是否在战斗中为false
        }

        Debug.Log("所有士兵的疲劳值已清空，疲劳恢复速度已重置为1，且不在战斗中");
    }
    // 通过士兵ID获取士兵详情
    public BattleSoldierDetail GetSoldierDetailByID(int soldierID)
    {
        if (currentPlayerAttributeData == null)
        {
            Debug.LogError("当前玩家属性数据未设置，请先初始化");
            return null;
        }

        foreach (var soldier in currentPlayerAttributeData.playerAttribute.PlayerSoldierID_List)
        {
            if (soldier.ID == soldierID)
            {
                return soldier;
            }
        }

        Debug.LogWarning($"士兵ID {soldierID} 不存在于当前玩家属性数据中");
        return null;
    }

    // 获取当前士兵的疲劳值
    public float GetFatigueValueByID(int soldierID)
    {
        BattleSoldierDetail soldierDetail = GetSoldierDetailByID(soldierID);
        if (soldierDetail != null)
        {
            return soldierDetail.fatigueValue;
        }
        else
        {
            Debug.LogWarning($"无法获取士兵ID {soldierID} 的疲劳值，因为该士兵不存在");
            return 0f; // 如果士兵不存在，返回0
        }
    }
    // 设置当前士兵的疲劳值
    public void SetFatigueValueByID(int soldierID, float fatigueValue)
    {
        BattleSoldierDetail soldierDetail = GetSoldierDetailByID(soldierID);
        if (soldierDetail != null)
        {
            soldierDetail.fatigueValue = fatigueValue;
            Debug.Log($"士兵ID {soldierID} 的疲劳值已设置为 {fatigueValue}");
        }
        else
        {
            Debug.LogWarning($"无法设置士兵ID {soldierID} 的疲劳值，因为该士兵不存在");
        }
    }
    // 设置当前士兵的疲劳增加速度和疲劳恢复速度
    public void SetFatigueSpeedByID(int soldierID, float fatigueIncreaseSpeed, float fatigueRecoverSpeed)
    {
        BattleSoldierDetail soldierDetail = GetSoldierDetailByID(soldierID);
        if (soldierDetail != null)
        {
            soldierDetail.fatigueIncreaseSpeed = fatigueIncreaseSpeed;
            soldierDetail.fatigueRecoverSpeed = fatigueRecoverSpeed;
            Debug.Log($"士兵ID {soldierID} 的疲劳增加速度已设置为 {fatigueIncreaseSpeed}，疲劳恢复速度已设置为 {fatigueRecoverSpeed}");
        }
        else
        {
            Debug.LogWarning($"无法设置士兵ID {soldierID} 的疲劳速度，因为该士兵不存在");
        }
    }

}

