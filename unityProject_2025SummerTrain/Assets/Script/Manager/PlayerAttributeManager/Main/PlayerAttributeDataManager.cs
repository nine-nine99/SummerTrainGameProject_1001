using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PlayerAttributeDataManager : Singleton<PlayerAttributeDataManager>
{
    [Header("玩家属性数据管理器")]
    public PlayerAttributeData_SO standerdPlayerAttributeData; // 标准玩家属性数据
    public PlayerAttributeData_SO currentPlayerAttributeData;
    private void OnEnable() {
        EventHandler.GameStartEvent += ResetCurrentPlayerAttributeData; // 游戏开始时重置玩家属性数据
        EventHandler.GameOverEvent += ResetCurrentPlayerAttributeData; // 游戏结束时重置   
    }
    private void OnDisable()
    {
        EventHandler.GameStartEvent -= ResetCurrentPlayerAttributeData; // 取消订阅游戏开始事件
        EventHandler.GameOverEvent -= ResetCurrentPlayerAttributeData; // 取消订阅游戏结束事件
    }

    // 获取当前玩家HP
    public int GetCurrentPlayerHP()
    {
        if (currentPlayerAttributeData == null)
        {
            Debug.LogError("当前玩家属性数据未设置，请先初始化");
            return 0;
        }

        return currentPlayerAttributeData.playerAttribute.HP;
    }
    // 设置当前玩家HP
    public void SetCurrentPlayerHP(int hp)
    {
        if (currentPlayerAttributeData == null)
        {
            Debug.LogError("当前玩家属性数据未设置，请先初始化");
            return;
        }

        currentPlayerAttributeData.playerAttribute.HP = hp;
        Debug.Log($"当前玩家HP已设置为: {hp}");
    }

    /// <summary>
    /// 通过标准玩家属性数据重置当前玩家属性数据
    /// </summary>
    public void ResetCurrentPlayerAttributeData()
    {
        // 检查标准数据是否存在
        if (standerdPlayerAttributeData == null)
        {
            Debug.LogError("标准玩家属性数据未设置，请在编辑器中设置 standerdPlayerAttributeData");
            return;
        }

        // 检查标准数据的玩家属性是否存在
        if (standerdPlayerAttributeData.playerAttribute == null)
        {
            Debug.LogError("标准玩家属性数据中的 playerAttribute 为空，无法重置");
            return;
        }

        // 深拷贝标准数据到当前数据
        currentPlayerAttributeData.playerAttribute = new PlayerAttribute(standerdPlayerAttributeData.playerAttribute);

        Debug.Log($"当前玩家属性数据已重置为标准数据 - 最大可召唤数量: {currentPlayerAttributeData.playerAttribute.maxSummonableListLength}, 士兵数量: {currentPlayerAttributeData.playerAttribute.PlayerSoldierID_List.Count}");
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

    /// <summary>
    /// 验证当前玩家属性数据是否有效
    /// </summary>
    /// <returns>如果数据有效返回 true，否则返回 false</returns>
    public bool IsCurrentPlayerAttributeDataValid()
    {
        if (currentPlayerAttributeData == null)
        {
            Debug.LogWarning("当前玩家属性数据为空");
            return false;
        }

        if (currentPlayerAttributeData.playerAttribute == null)
        {
            Debug.LogWarning("当前玩家属性数据中的 playerAttribute 为空");
            return false;
        }

        if (currentPlayerAttributeData.playerAttribute.PlayerSoldierID_List == null)
        {
            Debug.LogWarning("当前玩家属性数据中的士兵列表为空");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 强制重置当前玩家属性数据（带验证）
    /// </summary>
    public void ForceResetCurrentPlayerAttributeData()
    {
        ResetCurrentPlayerAttributeData();
        
        // 验证重置是否成功
        if (!IsCurrentPlayerAttributeDataValid())
        {
            Debug.LogError("玩家属性数据重置失败，请检查标准数据配置");
        }
        else
        {
            Debug.Log("玩家属性数据重置成功并已验证");
        }
    }
}

