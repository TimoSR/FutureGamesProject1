using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float moveSpeed = 3f;
    [field: SerializeField] public float Range = 4f;
    [field: SerializeField] public bool moveVertical = false, moveHorizontal = false;

    bool moveX = false;
    bool moveY = false;
    private float platformOriginX;
    private float platformOriginY;
    

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        platformOriginX = transform.position.x;
        platformOriginY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveHorizontal)
        {
            HoreizontalMovement();

        }
        if (moveVertical)
        {
            VerticalMovement();
        }

        if(moveHorizontal && moveVertical)
        {
            VerticalMovement();
            HoreizontalMovement();

        }


    }

    void HoreizontalMovement()
    {
        if (transform.position.x > platformOriginX + Range)
        {
            moveX = false;
        }
        if (transform.position.x < platformOriginX - Range)
        {
            moveX = true;
        }

        if (moveX)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        }

    }

    void VerticalMovement()
    {

        if (transform.position.y > platformOriginY + Range)
        {
            moveY = false;
        }

        if (transform.position.y < platformOriginY - Range)
        {
            moveY = true;
        }

        if (moveY)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
        }
    }

}
