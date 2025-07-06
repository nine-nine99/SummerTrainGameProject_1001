using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot_1001_Controller : MonoBehaviour, IPointerClickHandler 
{
    public int id; // 槽位ID
    public void OnPointerClick(PointerEventData eventData)
    {
        // 检查是否点击了召唤槽位
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 获取当前槽位的索引
            int slotIndex = transform.GetSiblingIndex();
            Debug.Log("Clicked on slot index: " + slotIndex);
            SoldierGeneratorManager.Instance.SetCurrentSoldierID(id); // 设置当前士兵ID
        }
    }
}
