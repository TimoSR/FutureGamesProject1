using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydra : MonoBehaviour
{
    [Header("States")]
    public AnimationStates animState;
    private GameObject target;
    public bool canSeeTarget;
    public bool sleeping;

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
        SetUpFOV();

        if (sleeping)
        {
            StartCoroutine(Sleep(sleepTimer));
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
        Debug.Log("Attacking");
    }

    IEnumerator Sleep(float sleepTime)
    {
        yield return new WaitForSeconds(sleepTime);
    }

}
