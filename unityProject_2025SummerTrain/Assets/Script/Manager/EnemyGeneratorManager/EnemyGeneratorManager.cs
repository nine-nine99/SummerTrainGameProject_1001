using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyGeneratorManager : Singleton<EnemyGeneratorManager>
{
    public List<Vector2> pathPoints; // 敌人生成路径点列表
    public GameObject enemyFather; // 敌人父物体
    private void OnEnable() {
        EventHandler.GameStartEvent += ClearAllEnemies; // 游戏开始时清空所有敌人
        EventHandler.GameOverEvent += ClearAllEnemies; // 游戏结束时清空所有敌人
    }
    private void OnDisable() {
        EventHandler.GameStartEvent -= ClearAllEnemies; // 取消订阅游戏开始事件
        EventHandler.GameOverEvent -= ClearAllEnemies; // 取消订阅游戏结束事件
    }
    private void Start()
    {
        GenerateEnemy(1001, pathPoints[0]); // 示例生成敌人
    }
    // 清空所有敌人
    public void ClearAllEnemies()
    {
        if (enemyFather == null)
        {
            enemyFather = GameObject.Find("EnemyFather");
        }
        if (enemyFather != null)
        {
            foreach (Transform child in enemyFather.transform)
            {
                Destroy(child.gameObject);
            }
            Debug.Log("已销毁所有敌人");
        }
        else
        {
            Debug.LogError("EnemyFather 物体未找到，无法销毁敌人");
        }
    }

    // 生成敌人
    public void GenerateEnemy(int enemyID, Vector3 position)
    {
        GameObject enemyPrefab = EnemySoldierDataManager.Instance.GetSoldierDetailByID(enemyID).soldierPrefab;
        if (enemyPrefab != null)
        {
            GameObject obj = Instantiate(enemyPrefab, position, Quaternion.identity);
            // 设置敌人的路径点
            PathController pathController = obj.GetComponent<PathController>();
            if (pathController != null)
            {
                pathController.pathPoints = new List<Vector2>(pathPoints);
            }
            else
            {
                Debug.LogError("PathController component not found on enemy prefab.");
            }

        }
        else
        {
            Debug.LogError("Enemy prefab not found for ID: " + enemyID);
        }
    }
}
