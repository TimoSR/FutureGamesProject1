using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationStates
{
    None,
    Idle,
    Walk,
    Attack
}
public enum AttackStates
{
    None,
    Attack
}

public class EnemyController : MonoBehaviour
{
    // Points the enemy will move between
    public Transform pointA;
    public Transform pointB;

    // States
    public AnimationStates animState;
    public AttackStates attackState;

    public GameObject target;
    public bool canWalk;
    [SerializeField] private bool canSeeTarget;
    [SerializeField] private float speed;

    private float directionX;
    private Vector3 moveTowardsPosition;
    private Vector3 pointAPosition;
    private Vector3 pointBPosition;

    [Header("Field of View")] //Field of view still a work in progress, currently only works in 90 degrees and doesnt change vertically
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 1f;
    [SerializeField] private int rayCount = 20;
    private float angleIncrease;
    private Vector3 fovDirection;
    private Vector3 fovStartPoint;
    private Vector3 fovEndPoint;
    private Vector3 increase;

    Animator animator;

    void Awake()
    {
        animState = AnimationStates.Idle;
        canWalk = true;
        angleIncrease = fov/rayCount;
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        fovStartPoint = new Vector3(Mathf.Sin((fov+(fov/2))*Mathf.Deg2Rad), Mathf.Cos((fov+(fov/2))*Mathf.Deg2Rad)).normalized;
        fovEndPoint = new Vector3(Mathf.Sin((fov-(fov/2))*Mathf.Deg2Rad), Mathf.Cos((fov-(fov/2))*Mathf.Deg2Rad)).normalized;
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
            RaycastHit hit;
            Debug.DrawRay(transform.position, fovDirection * viewDistance, Color.red);
            if(Physics.Raycast(transform.position, fovDirection, out hit, viewDistance))
            {
                
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
    }
    void Flip()
    {
        if (directionX == 1 || directionX == 0)
        {
            // Flip enemy and FOV
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            fovStartPoint = new Vector3(Mathf.Sin((fov+(fov/2))*Mathf.Deg2Rad), Mathf.Cos((fov+(fov/2))*Mathf.Deg2Rad)).normalized;
            fovEndPoint = new Vector3(Mathf.Sin((fov-(fov/2))*Mathf.Deg2Rad), Mathf.Cos((fov-(fov/2))*Mathf.Deg2Rad)).normalized;
        }
        else if (directionX == -1)
        {
            // Flip enemy and FOV
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            fovStartPoint = new Vector3(Mathf.Sin((-fov-(fov/2))*Mathf.Deg2Rad), Mathf.Cos((-fov-(fov/2))*Mathf.Deg2Rad)).normalized;
            fovEndPoint = new Vector3(Mathf.Sin((-fov+(fov/2))*Mathf.Deg2Rad), Mathf.Cos((-fov+(fov/2))*Mathf.Deg2Rad)).normalized;
        }
    }
}
