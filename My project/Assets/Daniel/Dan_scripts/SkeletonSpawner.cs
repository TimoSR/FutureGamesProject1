using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{

    public GameObject skeleton;

    private Vector2 whereToSpawn;

    [field: SerializeField] private float xRange;
    [field:SerializeField] public float yRange = 15f;
    [field: SerializeField] public float spawnRate;
    private float nextSpawn = 0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        SpawnSkeleton();
    }

    void SpawnSkeleton()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            xRange = Random.Range(-30, 30);
            whereToSpawn = new Vector3(xRange, yRange, 100f);
            Instantiate(skeleton, whereToSpawn, Quaternion.identity);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "KillObject")
        {
            Debug.Log("Colliding with death");
            gameObject.SetActive(false);
        }
    }

}
