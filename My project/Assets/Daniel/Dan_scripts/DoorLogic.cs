using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    private Agent thePlayer;

    public bool doorOpen, waitingToOpen;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Agent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingToOpen)
        {
            DoorOpening();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(thePlayer.KeyFollowPoint != null)
            {
                thePlayer.followingKey.objectToFollow = this.transform;
            }
        }
    }

    void DoorOpening()
    {
        if (Vector3.Distance(thePlayer.followingKey.transform.position, transform.position) < .1f)
        {
            waitingToOpen = false;
            doorOpen = true;
            thePlayer.followingKey.gameObject.SetActive(false);
            thePlayer.followingKey = null;
            //gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            //gameObject.transform.Rotate(0, 35, 0, Space.World);
            this.gameObject.SetActive(false);

        }
    }
}
