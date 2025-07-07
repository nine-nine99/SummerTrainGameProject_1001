using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "FatigueData", menuName = "ScriptableObjects/FatigueData_SO")]
public class FatigueData_SO : ScriptableObject
{
    public List<FatigueDetail> fatigueDetails; // 疲劳详情列表
}

[System.Serializable]
public class FatigueDetail
{
    public GridType gridType; // 网格类型
    public float fatigueIncreaseSpeed; // 疲劳增加速度
    public float fatigueRecoverSpeed; // 疲劳恢复速度

    public FatigueDetail(GridType gridType, float fatigueIncreaseSpeed, float fatigueRecoverSpeed)
    {
        this.gridType = gridType;
        this.fatigueIncreaseSpeed = fatigueIncreaseSpeed;
        this.fatigueRecoverSpeed = fatigueRecoverSpeed;
    }
    public FatigueDetail(float fatigueIncreaseSpeed, float fatigueRecoverSpeed)
    {
        this.gridType = GridType.CanPlace; // 默认网格类型为 CanPlace
        this.fatigueIncreaseSpeed = fatigueIncreaseSpeed;
        this.fatigueRecoverSpeed = fatigueRecoverSpeed;
    }
    public FatigueDetail() { } // 默认构造函数
}
