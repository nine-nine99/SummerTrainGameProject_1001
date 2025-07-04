using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    public CanvasGroup fadeCanvasGroup; // 用于淡入淡出的 UI 组件
    private float fadeDuration = 1f;    // 淡入淡出的持续时间

    private void OnEnable()
    {
        EventHandler.SceneTransitionEvent += ScenenTransition;
        EventHandler.SceneAddEvent += AddScene;
        // 游戏开始时
        EventHandler.GameStartEvent += OnGameStart;
        EventHandler.GameOverEvent += OnGameOver; // 游戏结束时也可以触发同样的淡出效果
    }
    private void OnDisable()
    {
        EventHandler.SceneTransitionEvent -= ScenenTransition;
        EventHandler.SceneAddEvent -= AddScene;
        // 游戏开始时
        EventHandler.GameStartEvent -= OnGameStart;
        EventHandler.GameOverEvent -= OnGameOver; // 游戏结束时也可以触发同样的淡出效果
    }
    public void OnGameOver()
    {
        StartCoroutine(GameOverTransition());
    }
    private IEnumerator GameOverTransition()
    {
        // 开始淡出(变黑)
        yield return StartCoroutine(Fade(1f));
        // 等待淡出完成
        // 例如，显示游戏结束界面、重置游戏状态等
        yield return SceneManager.UnloadSceneAsync("UI");
        yield return SceneManager.UnloadSceneAsync("01.Field");
        // 加载游戏结束界面
        yield return SceneManager.LoadSceneAsync("GameStartUI", LoadSceneMode.Additive);

        // 设置新场景为活动场景
        Scene newScene = SceneManager.GetSceneByName("GameStartUI");
        if (newScene.IsValid())
        {
            SceneManager.SetActiveScene(newScene);
        }
        // 开始淡入(变亮）
        yield return StartCoroutine(Fade(0f));
    }
    public void OnGameStart()
    {
        StartCoroutine(GameStartTransition());
    }

    private IEnumerator GameStartTransition()
    {
        // 开始淡出(变黑)
        yield return StartCoroutine(Fade(1f));
        // 等待淡出完成
        // 例如，初始化游戏状态、加载关卡等
        yield return SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("01.Field", LoadSceneMode.Additive);
        // 卸载游戏开始界面
        yield return SceneManager.UnloadSceneAsync("GameStartUI");
        // 设置新场景为活动场景
        Scene newScene = SceneManager.GetSceneByName("01.Field");
        if (newScene.IsValid())
        {
            SceneManager.SetActiveScene(newScene);
        }
        // 开始淡入(变亮）
        yield return StartCoroutine(Fade(0f));
    }

    /// <summary>
    /// 切换到新场景
    /// </summary>
    /// <param name="newSceneName">新场景的名称</param>
    public void ScenenTransition(string newSceneName)
    {
        StartCoroutine(PerformSceneTransition(newSceneName));
    }

    private IEnumerator PerformSceneTransition(string newSceneName)
    {
        // 开始淡出(变黑)
        yield return StartCoroutine(Fade(1f));
        // 等待淡出完成
        // 卸载当前场景
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 在卸载前触发事件
        EventHandler.CallSceneUnloadEvent(currentSceneName);
        // 卸载当前场景
        yield return SceneManager.UnloadSceneAsync(currentSceneName);
        // 加载新场景
        // 在加载新场景之前触发事件
        EventHandler.CallBeforeSceneLoadEvent(newSceneName);

        yield return SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);

        // 在加载完成后触发事件
        EventHandler.CallAfterSceneLoadEvent(newSceneName);

        // 将新场景设置为活动场景
        Scene newScene = SceneManager.GetSceneByName(newSceneName);
        if (newScene.IsValid())
        {
            SceneManager.SetActiveScene(newScene);
        }
        // 开始淡入(变亮）
        yield return StartCoroutine(Fade(0f));
    }
    /// <summary>
    /// 添加（叠加）新场景，不卸载当前场景
    /// </summary>
    /// <param name="sceneName">要添加的场景名</param>
    public void AddScene(string sceneName)
    {
        StartCoroutine(AddSceneCoroutine(sceneName));
    }

    private IEnumerator AddSceneCoroutine(string sceneName)
    {
        // // 开始淡出
        // yield return StartCoroutine(Fade(1f));
        Debug.Log($"Adding scene: {sceneName}");
        // 加载新场景（Additive模式，不卸载当前场景）
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // 设置新场景为活动场景
        Scene newScene = SceneManager.GetSceneByName(sceneName);
        if (newScene.IsValid())
        {
            SceneManager.SetActiveScene(newScene);
        }

        // 开始淡入
        // yield return StartCoroutine(Fade(0f));
    }
    /// <summary>
    /// 控制淡入淡出效果
    /// </summary>
    /// <param name="targetAlpha">目标透明度（0为完全透明，1为完全不透明）</param>
    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}
