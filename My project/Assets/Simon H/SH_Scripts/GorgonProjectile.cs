using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorgonProjectile : MonoBehaviour
{
    Rigidbody2D body;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (body.velocity.x > 0)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (body.velocity.x < 0)
        {
            transform.localEulerAngles =  Vector3.zero;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject); // Destroying the player for now but put the Death() here instead I guess
        }
        Destroy(this.gameObject);
    }
}
