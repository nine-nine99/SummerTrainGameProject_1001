using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGeneratorManager : Singleton<SoldierGeneratorManager>
{
    [HideInInspector]
    private int currentSoldierID = 0; // 当前士兵ID
    public GameObject SoldierFather; // 士兵父物体

    private void OnEnable() {
        EventHandler.GameStartEvent += DestroyAllSoldiers; // 游戏开始时销毁所有士兵
        EventHandler.GameOverEvent += DestroyAllSoldiers; // 游戏结束时销毁所有士兵
    }
    private void OnDisable() {
        EventHandler.GameStartEvent -= DestroyAllSoldiers; // 取消订阅游戏开始事件
        EventHandler.GameOverEvent -= DestroyAllSoldiers; // 取消订阅游戏结束事件
    }
    

    // 销毁所有 SoldierFather 下的士兵
    public void DestroyAllSoldiers()
    {
        if (SoldierFather == null)
        {
            SoldierFather = GameObject.Find("SoldierFather");
        }
        if (SoldierFather != null)
        {
            foreach (Transform child in SoldierFather.transform)
            {
                Destroy(child.gameObject);
            }
            Debug.Log("已销毁所有士兵");
        }
        else
        {
            Debug.LogError("SoldierFather 物体未找到，无法销毁士兵");
        }
    }

    // 输入坐标，通过 currentSoldierID 生成士兵
    public bool GenerateSoldier(Vector2Int coordinate)
    {
        // 通过 currentSoldierID 获取 BattleSoldierDetail 数据
        BattleSoldierDetail battleSoldierDetail = PlayerAttributeDataManager.Instance.GetSoldierDetailByID(currentSoldierID);
        SoldierDetail detail = SoldierDataManager.Instance.GetSoldierDetailByID(currentSoldierID);
        // 实例化士兵预制体
        if (detail == null || battleSoldierDetail == null)
        {
            Debug.LogError($"无法生成士兵，士兵ID {currentSoldierID} 的数据！！！不存在！！");
            return false;
        }
        if (battleSoldierDetail.isInBattle)
        {
            Debug.LogError($"无法生成士兵，士兵ID {currentSoldierID} 的数据正在战斗中！！！");
            return false;
        }

        GameObject thePrefab = detail.soldierPrefab;
        if (thePrefab == null)
        {
            Debug.LogError($"无法生成士兵，士兵ID {currentSoldierID} 的预制体不存在");
            return false;
        }
        // 你好
        GameObject obj = Instantiate(thePrefab, new Vector3(coordinate.x + 0.5f, coordinate.y + 0.5f, 0), Quaternion.identity);
        obj.transform.SetParent(SoldierFather.transform); // 设置父物体
        Debug.Log("成功生成士兵，ID: " + currentSoldierID + " 在坐标: " + coordinate);
        battleSoldierDetail.isInBattle = true; // 设置士兵为在战斗中
        // 通过地块情况，设置士兵的疲劳回复速度
        return true;
    }

    public int GetCurrentSoldierID()
    {
        return currentSoldierID;
    }
    public void SetCurrentSoldierID(int soldierID)
    {
        if (SoldierDataManager.Instance.GetSoldierDetailByID(soldierID) == null)
        {
            currentSoldierID = 0; // 如果士兵ID不存在，则重置为0
            Debug.LogError($"士兵ID {soldierID} 的数据不存在");
            return;
        }
        currentSoldierID = soldierID;
    }
}
