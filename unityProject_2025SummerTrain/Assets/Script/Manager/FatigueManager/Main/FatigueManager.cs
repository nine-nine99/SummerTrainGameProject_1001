using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatigueManager : Singleton<FatigueManager>
{
    public FatigueData_SO fatigueData; // 疲劳数据
    public void Update()
    {
        // 获取当前玩家的数据
        PlayerAttributeData_SO currentPlayerData = PlayerAttributeDataManager.Instance.currentPlayerAttributeData;
        if (currentPlayerData == null)
        {
            Debug.LogWarning("当前玩家属性数据未设置，请先初始化");
            return;
        }
        // 遍历所有士兵，更新疲劳值
        foreach (var soldier in currentPlayerData.playerAttribute.PlayerSoldierID_List)
        {
            // 获取士兵的疲劳详情
            FatigueDetail fatigueDetail = new FatigueDetail(soldier.fatigueIncreaseSpeed, soldier.fatigueRecoverSpeed);
            if (fatigueDetail == null)
            {
                Debug.LogWarning($"士兵 {soldier.ID} 的疲劳详情未找到");
                continue;
            }

            // 更新疲劳值
            if (soldier.isInBattle)
            {
                // 在战斗中，增加疲劳值
                soldier.fatigueValue += fatigueDetail.fatigueIncreaseSpeed * Time.deltaTime;
            }
            else
            {
                // 不在战斗中，恢复疲劳值
                soldier.fatigueValue -= fatigueDetail.fatigueRecoverSpeed * Time.deltaTime;
            }

            // 确保疲劳值在0到1之间
            soldier.fatigueValue = Mathf.Clamp(soldier.fatigueValue, 0f, 100f);
        }
    }
    // 通过Vector2Int获取疲劳详情
    public FatigueDetail GetFatigueDetailByCoordinate(Vector2Int coordinate)
    {
        GridType? gridType = GripMapDataManager.Instance.GetGridTypeAtCoordinate(coordinate);
        if (gridType == null)
        {
            Debug.LogWarning($"坐标 {coordinate} 在地图数据中不存在");
            return null;
        }
        foreach (var detail in fatigueData.fatigueDetails)
        {
            if (detail.gridType == gridType)
            {
                return detail;
            }
        }
        Debug.LogWarning($"没有找到与坐标 {coordinate} 对应的疲劳详情" + gridType);
        return null;
    }

    public Parameter ChangeParameterByFatigueValue(int soldierID, float fatigueValue)
    {
        SoldierDetail soldierDetail = SoldierDataManager.Instance.GetSoldierDetailByID(soldierID);
        Parameter currentParameter = new Parameter(soldierDetail.baseParameter); // 深拷贝，防止修改原始参数
        // 根据疲劳值调整参数
        if (fatigueValue <= 30)
        {
            // 如果疲劳值小于等于0，则不调整参数
            return currentParameter;
        }
        else if (fatigueValue < 50f)
        {
            // 疲劳值在0到0.2之间，降低生命值和攻击力
            currentParameter.HP *= 0.7f;
            currentParameter.AttackDamage *= 0.7f;
        }
        else if (fatigueValue < 70f)
        {
            // 疲劳值在0.2到0.5之间，降低生命值、攻击力和速度
            currentParameter.HP *= 0.5f;
            currentParameter.AttackDamage *= 0.5f;
            currentParameter.Speed *= 0.5f;
        }
        else
        {
            // 疲劳值大于等于0.5，严重降低所有参数
            currentParameter.HP *= 0.4f;
            currentParameter.AttackDamage *= 0.4f;
            currentParameter.Speed *= 0.4f;
            currentParameter.AttackRange *= 0.4f;
            currentParameter.AttackSpeed *= 0.4f;
        }
        return currentParameter;
    }
}
