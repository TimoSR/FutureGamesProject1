using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydra : MonoBehaviour
{
    [Header("States")]
    public AnimationStates animState;
    private GameObject target;
    public bool canSeeTarget;
    private bool sleeping;
    public bool fallingAsleep;
    private bool wakingUp;

    [Header("Field of View Settings")]
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 8f;
    [SerializeField] private Vector3 fovOriginOffset = Vector3.zero;
    private int rayCount = 60;
    private Vector3 fovDirection;
    private Vector3 fovStartPoint;
    private Vector3 fovEndPoint;
    private Vector3 increase;

    [Header("LayerMask (Set it to everything except enemies)")]
    [SerializeField] private LayerMask layerMask;

    [Header("Sleep timer in seconds")]
    [SerializeField] private float sleepTimer = 5f;

    Animator animator;

    

    void Awake()
    {
        animState = AnimationStates.Idle;
        animator = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        fovStartPoint = new Vector3(Mathf.Sin((180+((180-fov)/2))*Mathf.Deg2Rad), Mathf.Cos((180+((180-fov)/2))*Mathf.Deg2Rad)).normalized;
        fovEndPoint = new Vector3(Mathf.Sin((0-((180-fov)/2))*Mathf.Deg2Rad), Mathf.Cos((0-((180-fov)/2))*Mathf.Deg2Rad)).normalized;
        increase = (fovEndPoint - fovStartPoint)/(rayCount-1);
    }

    void Update()
    {
        animator.SetInteger("AnimationState", (int)animState);
        animator.SetBool("FallingAsleep", fallingAsleep);
        animator.SetBool("WakingUp", wakingUp);
        SetUpFOV();

        if (fallingAsleep)
        {
            StartCoroutine(FallingAsleep());
        }

        if (!canSeeTarget && !sleeping && !fallingAsleep)
        {
            animState = AnimationStates.Idle;
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
        }
    }

    void Attack()
    {
        animState = AnimationStates.Attack;
    }

    IEnumerator Sleep(float sleepTime)
    {
        sleeping = true;
        yield return new WaitForSeconds(sleepTime);
        StartCoroutine(WakingUp());
    }
    IEnumerator FallingAsleep()
    {
        animState = AnimationStates.Sleep;
        yield return new WaitForSeconds(1f);
        fallingAsleep = false;
        StartCoroutine(Sleep(sleepTimer));
    }
    IEnumerator WakingUp()
    {
        wakingUp = true;
        yield return new WaitForSeconds(1f);
        wakingUp = false;
        sleeping = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "PlayerProjectile")
        {
            fallingAsleep = true;
        }
    }

}
