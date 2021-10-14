using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraAttack : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
        }
    }
}
