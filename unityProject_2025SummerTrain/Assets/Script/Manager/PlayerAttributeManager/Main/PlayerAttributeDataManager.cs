using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PlayerAttributeDataManager : Singleton<PlayerAttributeDataManager>
{
    [Header("玩家属性数据管理器")]
    public PlayerAttributeData_SO standerdPlayerAttributeData; // 标准玩家属性数据
    public PlayerAttributeData_SO currentPlayerAttributeData;
}
