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

            if (!playerController.activatingShield && playerController.currentRecoilTime == 0)
            {
                if (Input.GetKey(KeyCode.D))
                {
                    playerController.MoveDirection(false, Time.deltaTime);
                    playerController.pressingWalk = true;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    playerController.MoveDirection(true, Time.deltaTime);
                    playerController.pressingWalk = true;
                }

                if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
                {
                    playerController.pressingWalk = false;
                }

                if (Input.GetMouseButton(0))
                {
                    playerController.Attack();
                }

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
                {
                    playerController.Jump();
                }
            }

            if (Input.GetKey(KeyCode.LeftShift) && playerController.GetPowerByName("shield").currentlyActive)
            {
                playerController.ActivateShield();
            }
            else if(playerController.activatingShield)
            {
                playerController.DeactivateShield();
            }
        }
    }
}
