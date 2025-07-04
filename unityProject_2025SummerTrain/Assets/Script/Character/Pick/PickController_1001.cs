using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickController_1001 : MonoBehaviour
{
    public float detectionRadius;
    private float duration = 0.3f;

    private void Update()
    {
        DetectTarget();
    }

    private void DetectTarget()
    {
        // 使用OverlapCircleAll检测范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.transform == null)
            {
                continue;
            }
            // if (collider.transform.parent != null && collider.transform.parent.GetComponent<ItemController>() != null)
            // {
            //     // 获取item的数据
            //     ItemController itemController = collider.transform.parent.GetComponent<ItemController>();

            //     ItemDetail itemDetail = ItemDataManager.Instance.GetItemDataByID(itemController.GetItemID());
            //     // 触发拾取效果
            //     // EventHandler.CallPickEffect(collider.transform.parent, itemDetail, duration);
            //     // 开始协程添加到背包
            //     StartCoroutine(AddToInventory(itemDetail));
            //     // 物品销毁
            //     ItemPoolManager.Instance.OnDestroyItem(collider.transform.parent.gameObject);
            // }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    
    private IEnumerator AddToInventory(ItemDetail itemDetail)
    {
        // 等待指定的持续时间
        yield return new WaitForSeconds(duration);

        // 物品添加到背包
        // 获取当前储存的硬币数量
        // float currentCount = InventoryDataManager.Instance.GetInventoryCount();
        // currentCount += itemDetail.itemCount;
        // // 设置新的储存数量
        // InventoryDataManager.Instance.SetInventoryCount(currentCount);
    }
}
