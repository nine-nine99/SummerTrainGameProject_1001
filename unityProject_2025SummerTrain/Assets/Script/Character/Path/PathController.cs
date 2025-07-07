using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public List<Vector2> pathPoints; // 路径点列表

    // 添加路径点
    public void AddPathPoint(Vector2 point)
    { 
        if (pathPoints == null)
        {
            pathPoints = new List<Vector2>();
        }
        pathPoints.Add(point);
    }

    // 获取路径点
    public List<Vector2> GetPathPoints()
    {
        return pathPoints;
    }

    // 清空路径点
    public void ClearPathPoints()
    {
        if (pathPoints != null)
        {
            pathPoints.Clear();
        }
    }
}
