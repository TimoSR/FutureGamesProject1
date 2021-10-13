using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLogic : MonoBehaviour
{
    public bool isFollowing;
    public float moveSpeed;
    public Transform objectToFollow;
    
    
    void Start()
    {
    }

    void Update()
    {
        if (isFollowing)
        {
            transform.position = Vector3.Lerp(transform.position, objectToFollow.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isFollowing)
            {
                Agent thePlayer = FindObjectOfType<Agent>();

                objectToFollow = thePlayer.KeyFollowPoint;

                isFollowing = true;
                thePlayer.followingKey = this;
            }
        }



    }
}
