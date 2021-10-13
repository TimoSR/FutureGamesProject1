using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationStates
{
    None,
    Idle,
    Walk,
    Attack,
    Sleep
}

public class EnemyController : MonoBehaviour
{
    // Points the enemy will move between
    public Transform pointA;
    public Transform pointB;

    // States
    public AnimationStates animState;

    private GameObject target;
    public bool canSeeTarget;
    public bool canWalk;
    public bool sleeping;

    [SerializeField] private float speed;
    private float directionX;
    private Vector3 moveTowardsPosition;
    private Vector3 pointAPosition;
    private Vector3 pointBPosition;

    //Field of view still a work in progress
    [SerializeField] private float fov = 120f;
    [SerializeField] private float viewDistance = 5f;
    private int rayCount = 60;
    private Vector3 fovDirection;
    private Vector3 fovStartPoint;
    private Vector3 fovEndPoint;
    private Vector3 increase;

    [SerializeField] private LayerMask layerMask;

    Animator animator;

    [SerializeField] private float sleepTimer = 5f;
    [SerializeField] private Vector3 fovOriginOffset = Vector3.zero;

    void Awake()
    {
        animState = AnimationStates.Idle;
        canWalk = true;
        animator = GetComponentInChildren<Animator>();
    }
    void Start()
    {

        fovStartPoint = new Vector3(Mathf.Sin((180-((180-fov)/2))*Mathf.Deg2Rad), Mathf.Cos((180-((180-fov)/2))*Mathf.Deg2Rad)).normalized;
        fovEndPoint = new Vector3(Mathf.Sin((0+((180-fov)/2))*Mathf.Deg2Rad), Mathf.Cos((0+((180-fov)/2))*Mathf.Deg2Rad)).normalized;

        // Does this so that you won't get an error if you don't give the thing pointA and pointB positions
        if (pointA == null)
        {
            pointAPosition = new Vector3(transform.position.x, transform.position.y);
        }
        else
        {
            pointAPosition = pointA.position;
        }
        if (pointB == null)
        {
            pointBPosition = new Vector3(transform.position.x, transform.position.y);
        }
        else
        {
            pointBPosition = pointB.position;
        }
        moveTowardsPosition = pointAPosition;
        increase = (fovEndPoint - fovStartPoint)/(rayCount-1);
    }

    void Update()
    {
        animator.SetInteger("AnimationState", (int)animState);
        GetDirection();

        if (directionX != 0 && canWalk)
        {
            animState = AnimationStates.Walk;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveTowardsPosition.x, transform.position.y), speed * Time.deltaTime);
        }
        else if(!canWalk)
        {
            animState = AnimationStates.Idle;
        }

        SetUpFOV();
        Flip();

        if (sleeping)
        {
            StartCoroutine(Sleep(sleepTimer));
        }

        if (target != null)
        {
            if (Mathf.Abs(target.transform.position.x - transform.position.x) > viewDistance || Mathf.Abs(target.transform.position.y - transform.position.y) > viewDistance)
            {
                canSeeTarget = false;
            }
        }
        if (target == null)
        {
            canSeeTarget = false;
        }
    }

    void GetDirection()
    {
        
        if (transform.position.x < moveTowardsPosition.x - 0.01f)
        {
            directionX = 1;
        }
        else if (transform.position.x > moveTowardsPosition.x + 0.01f)
        {
            directionX = -1;
        }
        else if (transform.position.x == moveTowardsPosition.x)
        {
            directionX = 0;
            SwitchPoint();
        }


        if (directionX == 0) 
        {
            animState = AnimationStates.Idle;
        }
    }

    void SetUpFOV()
    {
        for(float i = 0; i < rayCount; i++)
        {
            fovDirection = (fovStartPoint + (increase*i)).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position + fovOriginOffset, fovDirection, viewDistance, layerMask);


            //RaycastHit hit; Physics.Raycast(transform.position, fovDirection, out hit, viewDistance)
            Debug.DrawRay(transform.position + fovOriginOffset, fovDirection * viewDistance, Color.red);
            if(hit)
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    if (target == null)
                    {
                        target = hit.transform.gameObject;
                    }
                    moveTowardsPosition = target.transform.position;
                    canSeeTarget = true;
                }
            }
            if (i < 2)
            {
                if (!hit)
                {
                    if (canSeeTarget == true)
                    {
                        canWalk = false;
                    }
                    else
                    {
                        SwitchPoint();
                        canWalk = true;
                    }
                }
            }
        }
    }

    void SwitchPoint()
    {
        if (moveTowardsPosition.x == pointAPosition.x)
        {
            moveTowardsPosition = pointBPosition;
        }
        else if (moveTowardsPosition.x == pointBPosition.x)
        {
            moveTowardsPosition = pointAPosition;
        }
        else
        {
            moveTowardsPosition = pointAPosition;
        }
    }
    void Flip()
    {
        if (directionX == 1 || directionX == 0)
        {
            // Flip enemy and FOV
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            fovStartPoint = new Vector3(Mathf.Sin((180-((180-fov)/2))*Mathf.Deg2Rad), Mathf.Cos((180-((180-fov)/2))*Mathf.Deg2Rad)).normalized;
            fovEndPoint = new Vector3(Mathf.Sin((0+((180-fov)/2))*Mathf.Deg2Rad), Mathf.Cos((0+((180-fov)/2))*Mathf.Deg2Rad)).normalized;
        }
        else if (directionX == -1)
        {
            // Flip enemy and FOV
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            fovStartPoint = new Vector3(Mathf.Sin((180+((180-fov)/2))*Mathf.Deg2Rad), Mathf.Cos((180+((180-fov)/2))*Mathf.Deg2Rad)).normalized;
            fovEndPoint = new Vector3(Mathf.Sin((0-((180-fov)/2))*Mathf.Deg2Rad), Mathf.Cos((0-((180-fov)/2))*Mathf.Deg2Rad)).normalized;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Kill Player
            // Destroying the game object for now but implement a death function for the player if needed
            Destroy(other.gameObject);
            moveTowardsPosition = pointAPosition;
        }
        if (other.gameObject.tag == "PlayerProjectile")
        {
            StartCoroutine(Sleep(sleepTimer));
        }
    }

    IEnumerator Sleep(float sleepTime)
    {
        canWalk = false;
        animState = AnimationStates.Sleep;
        yield return new WaitForSeconds(sleepTime);
        canWalk = true;
        sleeping = false;
    }
}
