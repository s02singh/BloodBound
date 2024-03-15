using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMovement : MonoBehaviour
{
    public bool move = false;
    public float moveSpeed = 1f;
    public float maxHeight = 10f;

    private Vector3 initialPosition;
    private bool movingUp = true;

    private void Start()
    {
        initialPosition = transform.position; ;
    }

    private void Update()
    {
        if (move)
        {
            MoveBar();
        }
    }

    private void MoveBar()
    {
        if (movingUp)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // bar reached its max height
            if (transform.position.y >= initialPosition.y + maxHeight)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            if (transform.position.y <= initialPosition.y)
            {
                transform.position = initialPosition;
                movingUp = true;
                move = false;
            }
        }
    }
}
