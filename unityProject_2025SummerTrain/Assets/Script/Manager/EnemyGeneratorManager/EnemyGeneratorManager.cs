using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyGeneratorManager : Singleton<EnemyGeneratorManager>
{
    public List<Vector2> pathPoints_1001; // 路径点列表
    public List<Vector2> pathPoints_1002; // 路径点列表
    public List<Vector2> pathPoints_1003; // 路径点列表

    public GameObject enemyFather; // 敌人父物体
    private bool canControl = false; // 是否可以控制生成敌人
    private float generateInterval = 1.0f; // 敌人生成间隔时间
    private float timer = 0.0f; // 总计时器
    private float generateTime = 0.0f; // 生成敌人计时器
    private float gameDuration = 120.0f; // 游戏持续时间
    private void OnEnable()
    {
        EventHandler.GameStartEvent += OnGameStartEvent; // 游戏开始时清空所有敌人
        EventHandler.GameOverEvent += OnGameOverEvent; // 游戏结束时清空所有敌人
    }
    private void OnDisable() {
        EventHandler.GameStartEvent -= OnGameStartEvent; // 取消订阅游戏开始事件
        EventHandler.GameOverEvent -= OnGameOverEvent; // 取消订阅游戏结束事件
    }
    private void Start()
    {
        // GenerateEnemy(1001, pathPoints[0]); // 示例生成敌人
    }
    void Update()
    {
        if (!canControl)
        {
            return; // 如果不能控制，则不处理生成敌人
        }

        timer += Time.deltaTime; // 增加计时器
        if (timer <= gameDuration)
        {
            generateTime += Time.deltaTime; // 增加生成敌人计时器
            if (generateTime >= generateInterval)
            {
                // 每隔一定时间生成一个敌人
                // int enemyID = Random.Range(1001, 1004); // 随机选择敌人ID (1001, 1002, 或 1003)
                int enemyID = 1001;
                List<Vector2> pathPoints = null;
                int randomIndex = Random.Range(1, 4); // 随机选择路径点列表

                // 根据敌人ID选择对应的路径点
                switch (randomIndex)
                {
                    case 1:
                        pathPoints = pathPoints_1001;
                        break;
                    case 2:
                        pathPoints = pathPoints_1002;
                        break;
                    case 3:
                        pathPoints = pathPoints_1003;
                        break;
                    default:
                        Debug.LogError("未知的敌人ID: " + enemyID);
                        return;
                }

                GenerateEnemy(enemyID, pathPoints[0], pathPoints); // 生成敌人
                generateTime = 0.0f; // 重置生成计时器
            }
        }
        else
        {
            // 超过120秒后不再生成敌人
            Debug.Log("已超过120秒，不再生成敌人");
            EventHandler.CallGameOverEvent(); // 触发游戏结束事件
        }
    }
    public void OnGameStartEvent()
    {
        // 游戏开始时清空所有敌人
        ClearAllEnemies();
        Debug.Log("游戏开始，已清空所有敌人, 准备开始释放敌人");
        // 可以在这里设置 canControl 为 true，允许生成敌人
        canControl = true;
    }
    public void OnGameOverEvent()
    {
        // 游戏结束时清空所有敌人
        ClearAllEnemies();
        Debug.Log("游戏结束，已清空所有敌人");
        // 可以在这里设置 canControl 为 false，禁止生成敌人
        canControl = false;
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
    public void GenerateEnemy(int enemyID, Vector3 position, List<Vector2> pathPoints)
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
