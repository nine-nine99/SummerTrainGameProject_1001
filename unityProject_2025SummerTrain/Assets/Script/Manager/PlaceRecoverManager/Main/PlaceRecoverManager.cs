using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceRecoverManager : Singleton<PlaceRecoverManager>
{
    // 已经被放置了的坐标列表
    private List<Vector2Int> placedCoordinates = new List<Vector2Int>();
    void Update()
    {
        Place();
        Recover();
    }

    private void Recover()
    {
        // 这里可以添加恢复逻辑
        // 比如检测是否有需要恢复的坐标，或者其他恢复操作
        if (Input.GetMouseButtonDown(1))
        {
            // 检测鼠标点击位置是否在可放置区域内
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 从鼠标位置发射射线检测，检测是否鼠标位置处是否有parent上挂载了 IParameterController 的物体
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider.transform.parent != null)
            {
                // 检测到有物体被点击
                IParameterController parentController = hit.collider.transform.parent.GetComponent<IParameterController>();
                if (parentController != null)
                {
                    Vector2Int coordinate = new Vector2Int((int)Mathf.Floor(mousePosition.x), (int)Mathf.Floor(mousePosition.y));
                    RemoveFromPlacedCoordinates(coordinate); // 从已放置列表中移除该坐标

                    int ID = parentController.GetID();
                    // 执行恢复逻辑
                    // 销毁士兵Object
                    Destroy(hit.collider.transform.parent.gameObject);

                    BattleSoldierDetail battleSoldierDetail = PlayerAttributeDataManager.Instance.GetSoldierDetailByID(ID);
                    if (battleSoldierDetail != null)
                    {
                        // 设置士兵不在战斗中
                        battleSoldierDetail.isInBattle = false;
                        // Debug.Log($"成功恢复士兵，ID: {ID} 在坐标: {coordinate}");
                    }
                    else
                    {
                        // Debug.LogWarning($"无法恢复士兵，ID: {ID} 的数据不存在");
                    }
                }
            }

        }
    }

    private void Place()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUIObject())
            {
                // 如果鼠标点击在UI上，则不处理放置逻辑
                return;
            }
            // 检测鼠标点击位置是否在可放置区域内
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPosition = new Vector2Int((int)Mathf.Floor(mousePosition.x), (int)Mathf.Floor(mousePosition.y));

            // 检查该位置是否可以放置
            if (CanPlaceAt(gridPosition))
            {
                // 执行放置逻辑
                PlaceAt(gridPosition);
            }
        }
    }

    /// <summary>
    /// 检查指定坐标是否可以放置
    /// </summary>
    /// <param name="coordinate">要检查的坐标</param>
    /// <returns>如果坐标存在且 GridType 为 CanPlace 则返回 true
    /// 否则返回 false</returns>
    public bool CanPlaceAt(Vector2Int coordinate)
    {
        // 检查当前地图数据是否存在
        if (GripMapDataManager.Instance.currentGripMapData == null)
        {
            Debug.LogWarning("当前地图数据为空，无法检查坐标");
            return false;
        }
        // 检查是否已经被放置
        if (placedCoordinates.Contains(coordinate))
        {
            Debug.LogWarning($"坐标 {coordinate} 已经被放置过了");
            return false;
        }

        // 在 tileProperties 中查找匹配的坐标
        foreach (var tileProperty in GripMapDataManager.Instance.currentGripMapData.tileProperties)
        {
            if (tileProperty.tileCoordinate == coordinate)
            {
                // 检查 GridType 是否为 CanPlace
                return tileProperty.gridType == GridType.CanPlace;
            }
        }

        // 如果没有找到对应的坐标，返回 false
        Debug.LogWarning($"坐标 {coordinate} 在地图数据中不存在");
        return false;
    }
    /// <summary>
    /// 在指定坐标放置物体
    /// </summary>
    /// <param name="coordinate">要放置的坐标</param>
    public void PlaceAt(Vector2Int coordinate)
    {
        // 检查是否可以放置
        if (CanPlaceAt(coordinate))
        {
            // 这里可以添加实际的放置逻辑，如实例化物体
            bool success = SoldierGeneratorManager.Instance.GenerateSoldier(coordinate);

            if (!success)
            {
                Debug.LogWarning($"无法生成士兵, 未选中士兵或");
                return;
            }
            // 生成成功
            // 将坐标添加到已放置列表中
            if (!placedCoordinates.Contains(coordinate))
            {
                placedCoordinates.Add(coordinate);
                // 通过 coordinate 获取疲劳值数据
                FatigueDetail fatigueDetail = FatigueManager.Instance.GetFatigueDetailByCoordinate(coordinate);
                if (fatigueDetail != null)
                {
                    // 如果疲劳详情存在，可以在这里使用 fatigueDetail 进行其他操作
                    PlayerAttributeDataManager.Instance.SetFatigueSpeedByID(SoldierGeneratorManager.Instance.GetCurrentSoldierID(), fatigueDetail.fatigueIncreaseSpeed, fatigueDetail.fatigueRecoverSpeed);
                }
                else
                {
                    Debug.LogWarning($"无法获取坐标 {coordinate} 的疲劳详情");
                }
            }
        }
        else
        {
            Debug.LogWarning($"无法在坐标 {coordinate} 放置物体");
        }
    }

    /// <summary>
    /// 检查指定坐标是否已经被放置
    /// </summary>
    /// <param name="coordinate">要检查的坐标</param>
    /// <returns>如果坐标已经被放置则返回 true，否则返回 false</returns>
    public bool IsPlaced(Vector2Int coordinate)
    {
        return placedCoordinates.Contains(coordinate);
    }

    /// <summary>
    /// 从已放置坐标列表中移除指定坐标
    /// </summary>
    /// <param name="coordinate">要移除的坐标</param>
    /// <returns>如果成功移除则返回 true，否则返回 false</returns>
    public bool RemoveFromPlacedCoordinates(Vector2Int coordinate)
    {
        if (IsPlaced(coordinate))
        {
            bool removed = placedCoordinates.Remove(coordinate);
            if (removed)
            {
                Debug.Log($"成功从已放置列表中移除坐标 {coordinate}");
            }
            return removed;
        }
        else
        {
            Debug.LogWarning($"坐标 {coordinate} 未在已放置列表中，无法移除");
            return false;
        }
    }

    /// <summary>
    /// 清空所有已放置的坐标
    /// </summary>
    public void ClearAllPlacedCoordinates()
    {
        int count = placedCoordinates.Count;
        placedCoordinates.Clear();
        Debug.Log($"已清空所有已放置的坐标，共移除 {count} 个坐标");
    }

    /// <summary>
    /// 获取所有已放置坐标的只读列表
    /// </summary>
    /// <returns>已放置坐标的只读列表</returns>
    public List<Vector2Int> GetPlacedCoordinates()
    {
        return new List<Vector2Int>(placedCoordinates);
    }

    /// <summary>
    /// 获取已放置坐标的数量
    /// </summary>
    /// <returns>已放置坐标的数量</returns>
    public int GetPlacedCoordinatesCount()
    {
        return placedCoordinates.Count;
    }

    // 检测鼠标是否在UI上
    private bool IsPointerOverUIObject()
    {
        // 检查鼠标是否在UI上
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}
