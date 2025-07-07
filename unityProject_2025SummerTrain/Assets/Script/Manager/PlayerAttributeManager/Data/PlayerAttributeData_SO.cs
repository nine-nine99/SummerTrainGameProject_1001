using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributeData", menuName = "ScriptableObjects/PlayerAttributeData")]
public class PlayerAttributeData_SO : ScriptableObject
{
    public PlayerAttribute playerAttribute;

    // 深拷贝构造函数
    public PlayerAttributeData_SO(PlayerAttributeData_SO other)
    {
        playerAttribute = new PlayerAttribute(other.playerAttribute);
    }
    // 添加一个int类型的ID 到 PlayerSoldierID_List
    public void AddSoldierID(int soldierID)
    {
        if (playerAttribute.PlayerSoldierID_List.Count < playerAttribute.maxSummonableListLength)
        {
            // 检查是否已经存在相同的士兵ID
            foreach (var soldier in playerAttribute.PlayerSoldierID_List)
            {
                if (soldier.ID == soldierID)
                {
                    Debug.LogWarning("士兵ID已存在，无法添加重复的士兵ID: " + soldierID);
                    return; // 如果已经存在，则不添加
                }
            }
            // 如果没有达到最大召唤数量且没有重复的士兵ID，则添加新的士兵ID
            BattleSoldierDetail soldierDetail = new BattleSoldierDetail(soldierID, false, 0f, 0f, 1f); // 假设疲劳值初始为0
            playerAttribute.PlayerSoldierID_List.Add(soldierDetail);
            if (FightCanvasController.Instance != null)
            {
                FightCanvasController.Instance.updateSummonableSlots(); // 更新召唤槽位
            }
        }
        else
        {
            Debug.LogWarning("已达到最大可召唤数量，无法添加新的士兵ID。");
        }
    }
}

[System.Serializable]
public class PlayerAttribute
{
    public int HP; // 生命值
    public int maxSummonableListLength = 5; // 最大可召唤数量\
    public List<BattleSoldierDetail> PlayerSoldierID_List = new List<BattleSoldierDetail>(); // 可召唤物列表

    // 深拷贝构造函数
    public PlayerAttribute(PlayerAttribute other)
    {
        HP = other.HP;
        // 深拷贝最大可召唤数量
        maxSummonableListLength = other.maxSummonableListLength;
        
        // 真正的深拷贝：为每个 BattleSoldierDetail 创建新的实例
        PlayerSoldierID_List = new List<BattleSoldierDetail>();
        foreach (var soldier in other.PlayerSoldierID_List)
        {
            PlayerSoldierID_List.Add(new BattleSoldierDetail(soldier));
        }
    }
    
    /// <summary>
    /// 创建当前 PlayerAttribute 的深拷贝
    /// </summary>
    /// <returns>深拷贝的新实例</returns>
    public PlayerAttribute Clone()
    {
        return new PlayerAttribute(this);
    }
    
    /// <summary>
    /// 使用 LINQ 的更高效深拷贝方法（可选）
    /// </summary>
    /// <param name="other">要拷贝的源对象</param>
    public void DeepCopyFrom(PlayerAttribute other)
    {
        HP = other.HP;
        maxSummonableListLength = other.maxSummonableListLength;
        
        // 使用 LINQ 的 Select 方法进行深拷贝
        PlayerSoldierID_List = other.PlayerSoldierID_List.Select(soldier => new BattleSoldierDetail(soldier)).ToList();
    }
}
[System.Serializable]
public class BattleSoldierDetail
{
    public int ID;
    public bool isInBattle = false; // 是否在战斗中
    public float fatigueValue; // 疲劳值
    public float fatigueIncreaseSpeed; // 疲劳增加速度
    public float fatigueRecoverSpeed; // 疲劳恢复速度
    
    public BattleSoldierDetail(int id, bool isInBattle, float fatigueValue, float fatigueIncreaseSpeed, float fatigueRecoverSpeed)
    {
        ID = id;
        this.isInBattle = isInBattle;
        this.fatigueValue = fatigueValue;
        this.fatigueIncreaseSpeed = fatigueIncreaseSpeed;
        this.fatigueRecoverSpeed = fatigueRecoverSpeed;
    }
    
    // 深拷贝构造函数
    public BattleSoldierDetail(BattleSoldierDetail other)
    {
        ID = other.ID;
        isInBattle = other.isInBattle;
        fatigueValue = other.fatigueValue;
        fatigueIncreaseSpeed = other.fatigueIncreaseSpeed;
        fatigueRecoverSpeed = other.fatigueRecoverSpeed;
    }
    
    /// <summary>
    /// 创建当前 BattleSoldierDetail 的深拷贝
    /// </summary>
    /// <returns>深拷贝的新实例</returns>
    public BattleSoldierDetail Clone()
    {
        return new BattleSoldierDetail(this);
    }
    
    /// <summary>
    /// 重置士兵状态到初始状态
    /// </summary>
    public void ResetToDefault()
    {
        isInBattle = false;
        fatigueValue = 0f;
        fatigueIncreaseSpeed = 0f;
        fatigueRecoverSpeed = 1f;
    }
}