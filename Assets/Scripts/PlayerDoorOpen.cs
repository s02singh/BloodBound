using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoorOpen : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float maxDistance = 10f;

    public bool isOpening = false;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    public void OpenDoor()
    {
        if (isOpening)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.position, initialPosition) >= maxDistance)
            {
                isOpening = false;
            }
        }
    }

    private void Update()
    {
        OpenDoor();
    }
}
