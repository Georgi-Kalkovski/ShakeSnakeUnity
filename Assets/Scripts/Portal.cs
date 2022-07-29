using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Portal exitPortal;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            GameObject snake = collider.gameObject;
            Rigidbody2D rigidbody = snake.GetComponent<Rigidbody2D>();

            Vector3 inPosition = this.transform.InverseTransformPoint(snake.transform.position);
            inPosition.x = -inPosition.x;
            Vector3 outPosition = exitPortal.transform.TransformPoint(inPosition);

            Vector3 inDirection = this.transform.InverseTransformDirection(rigidbody.velocity);
            Vector3 outDirection = exitPortal.transform.TransformDirection(inDirection);

            snake.transform.position = outPosition;
            rigidbody.velocity = -outDirection;
        }
    }
}