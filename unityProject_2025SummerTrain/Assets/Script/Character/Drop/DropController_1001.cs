using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController_1001 : MonoBehaviour
{
    public int itemID = 1002; // 物品ID
    public void OnDeath()
    {
        // Debug.Log("掉落物被销毁了" + transform.name);
        // 通过物品ID创建掉落物
        Vector3 pos = transform.position;
        EventHandler.CallCreaterNewItem(itemID, pos);
    }
}
