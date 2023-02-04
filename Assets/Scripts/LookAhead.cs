using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Looks ahead
public class LookAhead : MonoBehaviour
{
    [Tooltip("How far is considererd in front")]
    public float inFrontFactor = 0.7f;
    [Tooltip("How far above position is the floor")]
    public float floorPosition = 0.26f;
    [Tooltip("Size of collision check sphere, make this large but not too large that the object will collide with itself")]
    public float collisionCheckSize = 0.2f;
    [Tooltip("Show collision check sphere, for debugging")]
    public bool showCollider = false;

    // Check if an object moving along a ground is about to hit a wall or fall off the
    // ground.
    // Returns true if it's okay to move forward (i.e. no wall and there's ground)
    // or false if it's not okay to move forward.
    // transform is the location of the object that's moving along the ground.
    // moveRight should be true if the object is moving to the right (positive X),
    // false if the object is moving to the left (negative X).
    public bool CheckIfCanMoveInFront(Transform transform, bool moveRight)
    {
        
        float xCheck = inFrontFactor * transform.localScale.x;
        if (moveRight)
        {
            xCheck = transform.position.x + xCheck;
        }
        else
        {
            xCheck = transform.position.x - xCheck;
        }
        Vector3 checkPosition = new Vector3(xCheck, transform.position.y + floorPosition * transform.localScale.y, transform.position.z);
        float rScaled = collisionCheckSize * transform.localScale.y;
        if (showCollider)
        {
            Debug.DrawLine(checkPosition, checkPosition + Vector3.left * rScaled);
            Debug.DrawLine(checkPosition, checkPosition + Vector3.right * rScaled);
            Debug.DrawLine(checkPosition, checkPosition + Vector3.up * rScaled);
            Debug.DrawLine(checkPosition, checkPosition + Vector3.down * rScaled);
            Debug.DrawLine(checkPosition, checkPosition + Vector3.forward * rScaled);
            Debug.DrawLine(checkPosition, checkPosition + Vector3.back * rScaled);
        }
        Collider[] collisions = Physics.OverlapSphere(checkPosition, rScaled);
        bool groundInFront = false;
        bool obstacleInFront = false;
        foreach (Collider collider in collisions)
        {
            if (collider.tag == "Ground")
            {
                groundInFront = true;
            }
            else if (collider.tag != "Enemy")
            {
                obstacleInFront = true;
            }
        }

        if (!groundInFront || obstacleInFront)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
