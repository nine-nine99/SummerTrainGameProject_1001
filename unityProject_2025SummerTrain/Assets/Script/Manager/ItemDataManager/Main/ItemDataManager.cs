using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : Singleton<ItemDataManager>
{
    public ItemData_SO itemData_SO;

    // 通过ID获取物品数据
    public ItemDetail GetItemDataByID(int id)
    {
        foreach (var item in itemData_SO.itemDetails)
        {
            if (item.itemID == id)
            {
                return item;
            }
        }
        return null;
    }
}
