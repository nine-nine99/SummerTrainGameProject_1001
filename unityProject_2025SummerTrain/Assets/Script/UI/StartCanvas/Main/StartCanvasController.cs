using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanvasController : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        // 调用事件处理器，触发游戏开始事件
        EventHandler.CallGameStartEvent();
    }
}
