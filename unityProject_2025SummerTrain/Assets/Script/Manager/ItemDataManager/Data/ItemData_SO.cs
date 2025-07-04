using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData_SO : ScriptableObject
{
    public List<ItemDetail> itemDetails = new List<ItemDetail>();
}

[System.Serializable]
public class ItemDetail
{
    public int itemID;
    public string itemName;
    public int itemCount;
    public Sprite itemSprite;
}
