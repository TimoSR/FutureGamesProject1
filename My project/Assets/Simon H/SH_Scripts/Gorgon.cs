using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorgon : MonoBehaviour
{
    [Header("Points the enemy will move between")]
    public Transform pointA;
    public Transform pointB;

    [Header("States")]
    public AnimationStates animState;
    private GameObject target;
    public bool canSeeTarget;
    public bool canWalk;
    public bool sleeping;

    private bool canFire = true;
    [Header("Speed")]
    [SerializeField] private float speed;
    private float directionX;
    private Vector3 moveTowardsPosition;
    private Vector3 pointAPosition;
    private Vector3 pointBPosition;

    [Header("Field of View Settings")]
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 8f;
    private int rayCount = 60;
    private Vector3 fovDirection;
    private Vector3 fovStartPoint;
    private Vector3 fovEndPoint;
    private Vector3 increase;

    Animator animator;

    [SerializeField] private Vector3 fovOriginOffset = Vector3.zero;

    [Header("Projectile Settings")]
    [SerializeField] private float projectilesPerSecond = 1;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;

    [Header("LayerMask (Set it to everything except enemies)")]
    [SerializeField] private LayerMask layerMask;

    [Header("Sleep timer in seconds")]
    [SerializeField] private float sleepTimer = 5f;

    private bool bounced;

    void Awake()
    {
        animState = AnimationStates.Idle;
        canWalk = true;
        animator = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        // Calculates the direction points the fov will be between
        fovStartPoint = new Vector3(Mathf.Sin((180-((180-fov)/2))*Mathf.Deg2Rad), Mathf.Cos((180-((180-fov)/2))*Mathf.Deg2Rad)).normalized;
        fovEndPoint = new Vector3(Mathf.Sin((0+((180-fov)/2))*Mathf.Deg2Rad), Mathf.Cos((0+((180-fov)/2))*Mathf.Deg2Rad)).normalized;

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

        if (directionX != 0 && canWalk && !sleeping)
        {
            animState = AnimationStates.Walk;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveTowardsPosition.x, transform.position.y), speed * Time.deltaTime);
        }
        else if(!canWalk && !sleeping)
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


        if (directionX == 0 && !sleeping) 
        {
            animState = AnimationStates.Idle;
        }
    }

    void SetUpFOV()
    {
        for(float i = 0; i < rayCount; i++)
        {
            fovDirection = (fovStartPoint + (increase*i)).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position+ fovOriginOffset, fovDirection, viewDistance, layerMask);


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
                    canSeeTarget = true;
                    if(!sleeping)
                    {
                        Attack();
                    }
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

   
    
    public IEnumerator Sleep(float sleepTime)
    {
        canWalk = false;
        animState = AnimationStates.Sleep;
        if (!bounced)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up*7, ForceMode2D.Impulse);
            bounced = true;
        }
        yield return new WaitForSeconds(sleepTime);
        if (!canSeeTarget && !bounced)
            canWalk = true;
        sleeping = false;
    }

    void Attack()
    {
        canWalk = false;
        animState = AnimationStates.Attack;
        if (canFire)
        {
            StartCoroutine(Fire(projectilesPerSecond));
        }
    }
    IEnumerator Fire(float timeBeforeNextAttack)
    {
        var projectile = Instantiate(projectilePrefab, new Vector3 (transform.position.x, transform.position.y + 0.8f), Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.x * projectileSpeed, 0), ForceMode2D.Impulse);
        Destroy(projectile, 2f);
        canFire = false;
        yield return new WaitForSeconds(timeBeforeNextAttack);
        canFire = true;
        canWalk = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
            moveTowardsPosition = pointAPosition;
        }
        if (other.gameObject.tag == "PlayerProjectile")
        {
            sleeping = true;
        }
        if (other.gameObject.tag == "Ground")
        {
            bounced = false;
        }
    }
}