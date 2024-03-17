using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float maxHeight = 10f;

    private Vector3 initialPosition;
    public bool movingUp = true;
    public bool movingDown = false;
    public GameObject startZone;

    private void Start()
    {
        initialPosition = transform.position;
        OpenBar();
    }

    private void Update()
    {
        MoveBar();
    }

    public void OpenBar()
    {
        movingUp = true;
        movingDown = false;
    }

    public void CloseBar()
    {
        movingUp = false;
        movingDown = true;
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
        if (movingDown)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            if (transform.position.y <= initialPosition.y)
            {
                transform.position = initialPosition;
                movingDown = false;
            }
            startZone.SetActive(false);
        }
    }
}
