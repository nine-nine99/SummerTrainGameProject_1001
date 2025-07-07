using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;
    
    [Header("缩放设置")]
    public float zoomSpeed = 2f;
    public float minZoom = 1f;
    public float maxZoom = 8f;
    public float initialZoom = 3f;
    
    [Header("边界限制")]
    public Vector2 minBounds = new Vector2(-10f, -10f);
    public Vector2 maxBounds = new Vector2(10f, 10f);
    
    private UnityEngine.Camera cam;
    private Vector3 targetPosition;
    private float targetZoom;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<UnityEngine.Camera>();
        if (cam == null)
        {
            Debug.LogError("Camera component not found!");
            return;
        }
        
        targetPosition = transform.position;
        targetZoom = initialZoom;
        cam.orthographicSize = initialZoom;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleZoom();
        ApplyBounds();
    }
    
    void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;
        
        // WASD移动输入
        if (Input.GetKey(KeyCode.W))
            moveDirection += Vector3.up;
        if (Input.GetKey(KeyCode.S))
            moveDirection += Vector3.down;
        if (Input.GetKey(KeyCode.A))
            moveDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            moveDirection += Vector3.right;
        
        // 应用移动
        if (moveDirection != Vector3.zero)
        {
            targetPosition += moveDirection.normalized * moveSpeed * Time.deltaTime;
        }
        
        // 平滑移动
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
    }
    
    void HandleZoom()
    {
        // 鼠标滚轮缩放
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0)
        {
            targetZoom -= scroll * zoomSpeed;
            
            // 计算基于边界的最大缩放限制
            float maxZoomByBounds = CalculateMaxZoomByBounds();
            
            // 应用所有限制
            targetZoom = Mathf.Clamp(targetZoom, minZoom, Mathf.Min(maxZoom, maxZoomByBounds));
        }
        
        // 平滑缩放，但同时确保不会超出边界限制
        float smoothZoom = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * 5f);
        
        // 再次检查边界限制，防止平滑过程中超出边界
        float currentMaxZoom = CalculateMaxZoomByBounds();
        smoothZoom = Mathf.Clamp(smoothZoom, minZoom, Mathf.Min(maxZoom, currentMaxZoom));
        
        cam.orthographicSize = smoothZoom;
    }
    
    void ApplyBounds()
    {
        // 根据摄像头的orthographicSize计算实际可视范围
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;
        
        // 限制摄像头位置在边界内
        float clampedX = Mathf.Clamp(targetPosition.x, minBounds.x + horzExtent, maxBounds.x - horzExtent);
        float clampedY = Mathf.Clamp(targetPosition.y, minBounds.y + vertExtent, maxBounds.y - vertExtent);
        
        targetPosition = new Vector3(clampedX, clampedY, targetPosition.z);
    }
    
    /// <summary>
    /// 根据边界大小计算最大允许的缩放值
    /// </summary>
    /// <returns>最大允许的orthographicSize值</returns>
    float CalculateMaxZoomByBounds()
    {
        // 计算边界的宽度和高度
        float boundsWidth = maxBounds.x - minBounds.x;
        float boundsHeight = maxBounds.y - minBounds.y;
        
        // 计算屏幕宽高比
        float aspectRatio = (float)Screen.width / Screen.height;
        
        // 基于高度的最大缩放
        float maxZoomByHeight = boundsHeight / 2f;
        
        // 基于宽度的最大缩放
        float maxZoomByWidth = boundsWidth / (2f * aspectRatio);
        
        // 考虑当前相机位置，计算基于位置的最大缩放
        Vector3 currentPos = transform.position;
        
        // 计算当前位置到边界的距离
        float distToLeftBound = currentPos.x - minBounds.x;
        float distToRightBound = maxBounds.x - currentPos.x;
        float distToBottomBound = currentPos.y - minBounds.y;
        float distToTopBound = maxBounds.y - currentPos.y;
        
        // 基于位置的最大缩放（确保不会超出边界）
        float maxZoomByPosHeight = Mathf.Min(distToTopBound, distToBottomBound);
        float maxZoomByPosWidth = Mathf.Min(distToLeftBound, distToRightBound) / aspectRatio;
        
        // 取所有限制中的最小值
        return Mathf.Min(maxZoomByHeight, maxZoomByWidth, maxZoomByPosHeight, maxZoomByPosWidth);
    }
    
    // 在场景视图中显示边界
    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Vector3 center = new Vector3((minBounds.x + maxBounds.x) / 2, (minBounds.y + maxBounds.y) / 2, 0);
            Vector3 size = new Vector3(maxBounds.x - minBounds.x, maxBounds.y - minBounds.y, 0);
            Gizmos.DrawWireCube(center, size);
        }
    }
    #endif
}
