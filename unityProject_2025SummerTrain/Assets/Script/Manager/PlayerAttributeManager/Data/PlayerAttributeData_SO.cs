using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributeData", menuName = "ScriptableObjects/PlayerAttributeData")]
public class PlayerAttributeData_SO : ScriptableObject
{
    public PlayerAttribute playerAttribute;
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
    public int maxSummonableListLength = 5; // 最大可召唤数量\
    public List<BattleSoldierDetail> PlayerSoldierID_List = new List<BattleSoldierDetail>(); // 可召唤物列表

    // 深拷贝构造函数
    public PlayerAttribute(PlayerAttribute other)
    {
        maxSummonableListLength = other.maxSummonableListLength;
        PlayerSoldierID_List = new List<BattleSoldierDetail>(other.PlayerSoldierID_List);
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
}