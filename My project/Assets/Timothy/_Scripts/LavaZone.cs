using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaZone : MonoBehaviour
{
    public LayerMask layerMask;
    public Vector2 size;

    [Header("Gizmo parameters")] public Color gizmoColor = Color.blue;
    public bool showGizmo = true;

    private void FixedUpdate()
    {
        Collider2D collider2D = Physics2D.OverlapBox(transform.position, size, 0, layerMask);

        if (collider2D != null)
        {
            Agent agent = collider2D.GetComponent<Agent>();
            
            if (agent == null)
            {
                Destroy(collider2D.gameObject);
                return;
            }

            agent.AgentDied(); 
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(transform.position, size);
        }
    }
}
