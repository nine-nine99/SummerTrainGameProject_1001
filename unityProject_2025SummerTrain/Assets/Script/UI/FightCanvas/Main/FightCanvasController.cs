using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightCanvasController : Singleton<FightCanvasController>
{
    public List<GameObject> SoldierSlots; // 可召唤士兵槽位列表

    private void Start() {
        // 更新召唤槽位
        updateSummonableSlots();
    }
    // 更新召唤槽位
    public void updateSummonableSlots()
    {
        // 清空现有的槽位
        foreach (var slot in SoldierSlots)
        {
            slot.SetActive(false);
        }

        // 获取当前可召唤的士兵ID列表
        List<int> summonableSoldierIDs = PlayerAttributeDataManager.Instance.currentPlayerAttributeData.playerAttribute.PlayerSoldierID_List;

        // 激活对应的槽位
        for (int i = 0; i < summonableSoldierIDs.Count && i < SoldierSlots.Count; i++)
        {
            SoldierSlots[i].SetActive(true);
            // 这里可以添加更多逻辑来更新槽位显示的士兵信息
            if (SoldierSlots[i].transform.childCount > 0)
            {
                // Debug.Log("清除之前的图标");
                // 清除之前的图标
                SoldierSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            }
            if (SoldierDataManager.Instance.GetSoldierDetailByID(summonableSoldierIDs[i]) == null)
            {
                Debug.LogError("获取士兵详情失败，ID: " + summonableSoldierIDs[i]);
                continue; // 如果获取失败，跳过当前循环
            }
            SoldierSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = SoldierDataManager.Instance.GetSoldierDetailByID(summonableSoldierIDs[i]).soldierIcon; // 假设槽位的第一个子物体是Image组件
        }
    }
}
