using UnityEngine;

/// <summary>
/// 敌人使用示例 - 展示如何创建和使用敌人对象
/// </summary>
public class EnemyUsageExample : MonoBehaviour
{
    [Header("敌人预制体")]
    [SerializeField] private GameObject targetDummyPrefab;
    
    [Header("生成设置")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int maxEnemies = 5;
    
    private void Start()
    {
        // 如果没有指定生成点，使用当前物体位置
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }
        
        // 生成一些测试敌人
        SpawnTestEnemies();
    }
    
    /// <summary>
    /// 生成测试敌人
    /// </summary>
    private void SpawnTestEnemies()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            Vector3 spawnPosition = spawnPoint.position + new Vector3(i * 2f, 0, 0);
            SpawnTargetDummy(spawnPosition);
        }
    }
    
    /// <summary>
    /// 生成靶子敌人
    /// </summary>
    /// <param name="position">生成位置</param>
    public void SpawnTargetDummy(Vector3 position)
    {
        GameObject enemyObject;
        
        if (targetDummyPrefab != null)
        {
            // 使用预制体生成
            enemyObject = Instantiate(targetDummyPrefab, position, Quaternion.identity);
        }
        else
        {
            // 动态创建靶子敌人
            enemyObject = CreateTargetDummyDynamically(position);
        }
        
        // 设置物体名称
        enemyObject.name = $"TargetDummy_{Time.frameCount}";
        
        Debug.Log($"已生成靶子敌人: {enemyObject.name} 在位置: {position}");
    }
    
    /// <summary>
    /// 动态创建靶子敌人（不使用预制体）
    /// </summary>
    /// <param name="position">生成位置</param>
    /// <returns>创建的敌人对象</returns>
    private GameObject CreateTargetDummyDynamically(Vector3 position)
    {
        // 创建一个基础的立方体作为靶子
        GameObject enemyObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        enemyObject.transform.position = position;
        
        // 添加靶子敌人组件
        TargetDummy targetDummy = enemyObject.AddComponent<TargetDummy>();
        
        // 设置材质颜色
        Renderer renderer = enemyObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = new Material(Shader.Find("Standard"));
            material.color = Color.red;
            renderer.material = material;
        }
        
        return enemyObject;
    }
    
    /// <summary>
    /// 测试攻击所有敌人
    /// </summary>
    [ContextMenu("测试攻击所有敌人")]
    public void TestAttackAllEnemies()
    {
        // 查找场景中所有的敌人
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
        
        foreach (EnemyBase enemy in enemies)
        {
            if (!enemy.IsDead())
            {
                // 随机伤害值
                float damage = Random.Range(10f, 50f);
                enemy.TakeDamage(damage, gameObject);
            }
        }
        
        Debug.Log($"攻击了 {enemies.Length} 个敌人");
    }
    
    /// <summary>
    /// 重置所有敌人
    /// </summary>
    [ContextMenu("重置所有敌人")]
    public void ResetAllEnemies()
    {
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
        
        foreach (EnemyBase enemy in enemies)
        {
            enemy.ResetEnemy();
        }
        
        Debug.Log($"重置了 {enemies.Length} 个敌人");
    }
    
    /// <summary>
    /// 在指定位置生成靶子（公共方法，可在Inspector中调用）
    /// </summary>
    public void SpawnTargetAtPosition(Vector3 position)
    {
        SpawnTargetDummy(position);
    }
    
    /// <summary>
    /// 在鼠标位置生成靶子
    /// </summary>
    public void SpawnTargetAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            SpawnTargetDummy(hit.point);
        }
        else
        {
            // 如果没有击中任何物体，在摄像机前方生成
            Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * 5f;
            SpawnTargetDummy(spawnPos);
        }
    }
    
    private void Update()
    {
        // 按T键生成靶子
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnTargetAtMousePosition();
        }
        
        // 按R键重置所有敌人
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetAllEnemies();
        }
        
        // 按A键攻击所有敌人
        if (Input.GetKeyDown(KeyCode.A))
        {
            TestAttackAllEnemies();
        }
    }
} 