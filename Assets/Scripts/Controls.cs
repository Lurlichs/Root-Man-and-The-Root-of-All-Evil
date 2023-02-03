using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] private Player playerController;

    // Update is called once per frame
    void Update()
    {
        if (playerController.alive && playerController.playing)
        {
            /*
            float x = Input.GetAxisRaw("Horizontal");
            playerController.MoveDirection(x, Time.deltaTime);*/

            if (Input.GetKey(KeyCode.D))
            {
                playerController.MoveDirection(false, Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                playerController.MoveDirection(true, Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                playerController.ShootProjectile();
            }
        }
    }
}
