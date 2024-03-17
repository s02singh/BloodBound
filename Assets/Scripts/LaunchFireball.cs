using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchFireball : MonoBehaviour
{   
    [SerializeField] private GameObject dragon;
    [SerializeField] private GameObject player;
    public void Launch(GameObject projectile)
    {
        player = GameObject.Find("PlayerArmature");
        // Get the position of the dragon
        Vector3 startingPosition = dragon.transform.position;
        startingPosition.y += 23;
        startingPosition.x -= 10;

        // Get the direction from the starting position to the player
        Vector3 direction = (player.transform.position - startingPosition).normalized;

        // Calculate the rotation to look towards the player
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Instantiate the projectile with the calculated rotation
        Instantiate(projectile, startingPosition, rotation);
    }
}
