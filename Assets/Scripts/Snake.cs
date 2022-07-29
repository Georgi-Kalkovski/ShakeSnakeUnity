using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    private List<Transform> segments = new List<Transform>();
    public Transform segmentPrefab;
    public Vector2 direction = Vector2.right;
    private Vector2 input;
    public int initialSize = 4;

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        var X = Input.acceleration.x;
        var Y = Input.acceleration.y;
        //Debug.Log(String.Format("\n X = \"" + X + "\"" + "Y = \" " + Y + "\""));
        if (X > 0 && Y > 0)
        {
            if (Math.Abs(X) > Math.Abs(Y))
            {
                input = Vector2.right;
            }
            else
            {
                input = Vector2.up;
            }
        }
        else if (X > 0 && Y < 0)
        {
            if (Math.Abs(X) > Math.Abs(Y))
            {
                input = Vector2.right;
            }
            else
            {
                input = Vector2.down;
            }
        }
        else if (X < 0 && Y < 0)
        {
            if (Math.Abs(X) > Math.Abs(Y))
            {
                input = Vector2.left;
            }
            else
            {
                input = Vector2.down;
            }
        }
        else if (X < 0 && Y > 0)
        {
            if (Math.Abs(X) > Math.Abs(Y))
            {
                input = Vector2.left;
            }
            else
            {
                input = Vector2.up;
            }
        }
    }

    private void FixedUpdate()
    {
        // Set the new direction based on the input
        if (input != Vector2.zero) {
            direction = input;
        }

        // Set each segment's position to be the same as the one it follows. We
        // must do this in reverse order so the position is set to the previous
        // position, otherwise they will all be stacked on top of each other.
        for (int i = segments.Count - 1; i > 0; i--) {
            segments[i].position = segments[i - 1].position;
        }

        // Move the snake in the direction it is facing
        // Round the values to ensure it aligns to the grid
        float x = Mathf.Round(transform.position.x) + direction.x;
        float y = Mathf.Round(transform.position.y) + direction.y;

        transform.position = new Vector2(x, y);
    }

    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    public void ResetState()
    {
        direction = Vector2.right;
        transform.position = Vector3.zero;

        // Start at 1 to skip destroying the head
        for (int i = 1; i < segments.Count; i++) {
            Destroy(segments[i].gameObject);
        }

        // Clear the list but add back this as the head
        segments.Clear();
        segments.Add(transform);

        // -1 since the head is already in the list
        for (int i = 0; i < initialSize - 1; i++) {
            Grow();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food")) {
            Grow();
        } else if (other.gameObject.CompareTag("Obstacle")) {
            ResetState();
        }
    }

}
