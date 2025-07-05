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
            playerAttribute.PlayerSoldierID_List.Add(soldierID);
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
    public List<int> PlayerSoldierID_List = new List<int>(); // 可召唤物列表

    // 深拷贝构造函数
    public PlayerAttribute(PlayerAttribute other)
    {
        maxSummonableListLength = other.maxSummonableListLength;
        PlayerSoldierID_List = new List<int>(other.PlayerSoldierID_List);
    }
}


