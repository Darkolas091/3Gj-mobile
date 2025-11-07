using UnityEngine;

/// <summary>
/// Attached to turrets spawned from premium crops to track their plot position
/// and notify the grid when they're destroyed
/// </summary>
public class TurretPlotTracker : MonoBehaviour
{
    public Vector3 plotPosition;
    public GameGrid gridManager;

    private void OnDestroy()
    {
        // Notify the grid manager that this turret was destroyed
        if (gridManager != null)
        {
            gridManager.OnTurretDestroyed(plotPosition);
        }
    }
}

