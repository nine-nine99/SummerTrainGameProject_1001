using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectController : MonoBehaviour
{
    public float detectionRadius = 5f;
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    // public LayerMask targetLayer;

    private void Update()
    {
        DetectTarget();
    }

    private void DetectTarget()
    {
        detectedColliders.Clear();
        // 使用OverlapCircleAll检测范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (Collider2D collider in colliders)
        {
            detectedColliders.Add(collider);
        }
        detectedColliders.Remove(transform.parent.GetChild(1).GetComponent<Collider2D>());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
