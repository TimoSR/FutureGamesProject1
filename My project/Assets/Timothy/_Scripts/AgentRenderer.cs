using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentRenderer : MonoBehaviour
{

    public void FaceDirection(Vector2 input)
    {
        var localScale = transform.parent.localScale;
        
        if (input.x < 0)
        {
            
            transform.parent.localScale = new Vector3(-1 * Mathf.Abs(localScale.x), localScale.y, localScale.z);
            
        }
        else if (input.x > 0)
        {
            transform.parent.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
        
    }
    
}
