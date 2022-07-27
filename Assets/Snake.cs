using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public float speed = 5f;
    // Current Movement Direction
    // (by default it moves to the right)
    Vector2 dir = Vector2.right;
    // Keep Track of Tail
    List<Transform> tail = new List<Transform>();

    // Did the snake eat something?
    bool ate = false;

    // Tail Prefab
    public GameObject tailPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Move the Snake every 300ms
        InvokeRepeating("Move", 0.3f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        var X = Input.acceleration.x;
        var Y = Input.acceleration.y;
        //Debug.Log(String.Format("\n X = \"" + X + "\"" + "Y = \" " + Y + "\""));
        if (X > 0 && Y > 0)
        {
            if (Math.Abs(X) > Math.Abs(Y))
            {
                dir = Vector2.right;
            }
            else
            {
                dir = Vector2.up;
            }
        }
        else if (X > 0 && Y < 0)
        {
            if (Math.Abs(X) > Math.Abs(Y))
            {
                dir = Vector2.right;
            }
            else
            {
                dir = Vector2.down;
            }
        }
        else if (X < 0 && Y < 0)
        {
            if (Math.Abs(X) > Math.Abs(Y))
            {
                dir = Vector2.left;
            }
            else
            {
                dir = Vector2.down;
            }
        }
        else if (X < 0 && Y > 0)
        {
            if (Math.Abs(X) > Math.Abs(Y))
            {
                dir = Vector2.left;
            }
            else
            {
                dir = Vector2.up;
            }
        }
    }
    void Move()
    {
        // Save current position (gap will be here)
        Vector2 v = transform.position;

        // Move head into new direction (now there is a gap)
        transform.Translate(dir);

        // Ate something? Then insert new Element into gap
        if (ate)
        {
            // Load Prefab into the world
            GameObject g = (GameObject)Instantiate(tailPrefab,
                                                  v,
                                                  Quaternion.identity);

            // Keep track of it in our tail list
            tail.Insert(0, g.transform);

            // Reset the flag
            ate = false;
        }
        // Do we have a Tail?
        else if (tail.Count > 0)
        {
            // Move last Tail Element to where the Head was
            tail.Last().position = v;

            // Add to front of list, remove from the back
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // Food?
        if (coll.name.StartsWith("FoodPrefab"))
        {
            // Get longer in next Move call
            ate = true;

            // Remove the Food
            Destroy(coll.gameObject);
        }
        // Collided with Tail or Border
        else
        {
            // ToDo 'You lose' screen
        }
    }
}
