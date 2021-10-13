using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicProjectile : MonoBehaviour
{
    // Array of colors
    Color[] colors = { Color.red, Color.green, Color.magenta, Color.cyan, Color.yellow };
    void Start()
    {
        // Sets the object to a random color whenever it spawns
        gameObject.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Length)];
    }
}
