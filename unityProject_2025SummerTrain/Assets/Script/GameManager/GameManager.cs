using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private bool isGameStarted = false; // 游戏是否已开始
    private void OnEnable()
    {
        EventHandler.GameStartEvent += OnGameStart; // 订阅游戏开始事件
        EventHandler.GameOverEvent += OnGameOver; // 订阅游戏结束事件
    }
    private void OnDisable()
    {
        EventHandler.GameStartEvent -= OnGameStart; // 取消订阅游戏开始事件
        EventHandler.GameOverEvent -= OnGameOver; // 取消订阅游戏结束事件
    }
    private void Start()
    {
        SceneManager.LoadSceneAsync("UI_Start", LoadSceneMode.Additive);
    }

    void Update()
    {
        if (!isGameStarted)
        {
            return; // 如果游戏未开始，则不处理逻辑
        }
        if (PlayerAttributeDataManager.Instance.currentPlayerAttributeData.playerAttribute.HP <= 0)
        {
            Debug.Log("Game Over: Player HP is 0 or less.");
            // 如果玩家属性数据中的生命值小于等于0，则触发游戏结束事件
            EventHandler.CallGameOverEvent();
        }
    }
    public void OnGameStart()
    {
        isGameStarted = true; // 设置游戏已开始
    }
    public void OnGameOver()
    {
        isGameStarted = false; // 设置游戏未开始
    }
}
