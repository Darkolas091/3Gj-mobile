using UnityEngine;

public class SphereGizmo : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private Color gizmoColor = Color.red;
    
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
