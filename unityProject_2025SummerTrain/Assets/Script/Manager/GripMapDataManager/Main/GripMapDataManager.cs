using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripMapDataManager : Singleton<GripMapDataManager>
{
    public MapData_SO currentGripMapData;

    /// <summary>
    /// 检查指定坐标是否可以放置
    /// </summary>
    /// <param name="coordinate">要检查的坐标</param>
    /// <returns>如果坐标存在且 GridType 为 CanPlace 则返回 true，否则返回 false</returns>
    public bool CanPlaceAtCoordinate(Vector2 coordinate)
    {
        // 检查当前地图数据是否存在
        if (currentGripMapData == null)
        {
            Debug.LogWarning("当前地图数据为空，无法检查坐标");
            return false;
        }

        // 将 Vector2 转换为 Vector2Int 进行比较
        Vector2Int targetCoordinate = new Vector2Int(Mathf.RoundToInt(coordinate.x), Mathf.RoundToInt(coordinate.y));

        // 在 tileProperties 中查找匹配的坐标
        foreach (var tileProperty in currentGripMapData.tileProperties)
        {
            if (tileProperty.tileCoordinate == targetCoordinate)
            {
                // 检查 GridType 是否为 CanPlace
                return tileProperty.gridType == GridType.CanPlace;
            }
        }

        // 如果没有找到对应的坐标，返回 false
        Debug.LogWarning($"坐标 {targetCoordinate} 在地图数据中不存在");
        return false;
    }

    /// <summary>
    /// 检查指定坐标是否可以放置 (Vector2Int 重载)
    /// </summary>
    /// <param name="coordinate">要检查的坐标</param>
    /// <returns>如果坐标存在且 GridType 为 CanPlace 则返回 true，否则返回 false</returns>
    public bool CanPlaceAtCoordinate(Vector2Int coordinate)
    {
        // 检查当前地图数据是否存在
        if (currentGripMapData == null)
        {
            Debug.LogWarning("当前地图数据为空，无法检查坐标");
            return false;
        }

        // 在 tileProperties 中查找匹配的坐标
        foreach (var tileProperty in currentGripMapData.tileProperties)
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
    /// 获取指定坐标的网格类型
    /// </summary>
    /// <param name="coordinate">要检查的坐标</param>
    /// <returns>返回对应的 GridType，如果不存在则返回 null</returns>
    public GridType? GetGridTypeAtCoordinate(Vector2Int coordinate)
    {
        // 检查当前地图数据是否存在
        if (currentGripMapData == null)
        {
            Debug.LogWarning("当前地图数据为空，无法获取网格类型");
            return null;
        }

        // 在 tileProperties 中查找匹配的坐标
        foreach (var tileProperty in currentGripMapData.tileProperties)
        {
            if (tileProperty.tileCoordinate == coordinate && tileProperty.gridType != GridType.CanPlace)
            {
                return tileProperty.gridType;
            }
        }

        // 如果没有找到对应的坐标，返回 null
        return null;
    }
}
