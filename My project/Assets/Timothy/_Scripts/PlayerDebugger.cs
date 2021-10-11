using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebugger : MonoBehaviour
{
    
    [Header("Collisions")]
    [SerializeField] private LayerMask collideWith;
    [SerializeField] private int verticalRayAmount;

    private BoxCollider2D _boxCollider2D;

    private float _boundsWidth;
    private float _boundsHeight;
    
    private Vector2 _boundsTopLeft;
    private Vector2 _boundsTopRight;
    private Vector2 _boundsBottomLeft;
    private Vector2 _boundsBottomRight;
    
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        SetRayOrigins();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.timeScale > 0) DrawDebugRays();
        
    }
    
    private void DrawDebugRays()
    {
        Debug.DrawRay(_boundsBottomLeft, Vector2.left, Color.green);
        Debug.DrawRay(_boundsBottomRight, Vector2.right, Color.green);
        Debug.DrawRay(_boundsTopLeft, Vector2.left, Color.green);
        Debug.DrawRay(_boundsTopRight, Vector2.right, Color.green);
    }

    private void SetRayOrigins()
    {

        Bounds playerBounds = _boxCollider2D.bounds;

        _boundsBottomLeft = new Vector2(playerBounds.min.x, playerBounds.min.y);
        _boundsBottomRight = new Vector2(playerBounds.max.x, playerBounds.min.y);
        _boundsTopLeft = new Vector2(playerBounds.min.x, playerBounds.max.y);
        _boundsTopRight = new Vector2(playerBounds.max.x, playerBounds.max.x);

        _boundsHeight = Vector2.Distance(_boundsBottomLeft, _boundsTopLeft);
        _boundsWidth = Vector2.Distance(_boundsBottomLeft, _boundsBottomRight);

    }
    
}
