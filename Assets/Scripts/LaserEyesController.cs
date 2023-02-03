using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEyesController : MonoBehaviour
{
    public float attackFrequency = 10.0f;
    public float maxAngularSpeed = 360.0f;
    public float angularAcceleration = 360.0f;
    public float laserBeamLinger = 0.2f;

    private float lastAttack;
    private bool preAttackLinger;
    private bool inAttack;
    private bool sweepRight;
    private float sweepAngle;
    private float sweepAngularSpeed;
    public Transform target;
    private bool aboutToDestroyLaserBeam;
    private float preAttackLingerStart;
    private float laserBeamReachedTarget;

    public GameObject laserBeamPrefab;
    public GameObject laserBeam;

    void SetTarget(Transform desiredTarget)
    {
        target = desiredTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void EndSweep()
    {
        Debug.Log("End sweep");
        if (sweepRight)
        {
            sweepAngle = -180.0f;
            laserBeam.transform.rotation = Quaternion.Euler(new Vector3(0.0f, -270.0f, 0.0f));
            sweepAngularSpeed = 0.0f;
        }
        else
        {
            sweepAngle = 180.0f;
            laserBeam.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 270.0f, 0.0f));
            sweepAngularSpeed = 0.0f;
        }
        aboutToDestroyLaserBeam = true;
        laserBeamReachedTarget = Time.fixedTime;
        inAttack = false;
    }

    void FixedUpdate()
    {
        // Check if time to do a new attack
        if ((Time.fixedTime - lastAttack) > attackFrequency)
        {
            lastAttack = Time.fixedTime;
            // Launch a new attack
            preAttackLinger = true;
            preAttackLingerStart = Time.fixedTime;
            inAttack = true;
            sweepRight = target.position.x > transform.position.x;
            laserBeam = Instantiate(laserBeamPrefab, transform.position, Quaternion.identity, transform);
            sweepAngle = 0.0f;
            sweepAngularSpeed = 0.0f;
            if (sweepRight)
            {
                laserBeam.transform.rotation = Quaternion.Euler(new Vector3(0.0f, -90.0f, 0.0f));
            }
            else
            {
                laserBeam.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
            }
        }
        if (inAttack)
        {
            if (preAttackLinger)
            {
                if ((Time.fixedTime - preAttackLingerStart) >= laserBeamLinger)
                {
                    preAttackLinger = false;
                }
            }
            else
            {
                float t = maxAngularSpeed / angularAcceleration;
                float easeOutThreshold = 0.5f * angularAcceleration * t * t;
                bool easingOut;
                if (sweepRight)
                {
                    laserBeam.transform.rotation = Quaternion.Euler(new Vector3(0.0f, sweepAngle - 90.0f, 0.0f));
                    sweepAngle -= sweepAngularSpeed * Time.fixedDeltaTime;
                    easingOut = sweepAngle < (-180f + easeOutThreshold);
                }
                else
                {
                    laserBeam.transform.rotation = Quaternion.Euler(new Vector3(0.0f, sweepAngle + 90.0f, 0.0f));
                    sweepAngle += sweepAngularSpeed * Time.fixedDeltaTime;
                    easingOut = sweepAngle > (180f - easeOutThreshold);
                }
                if (!easingOut)
                {
                    sweepAngularSpeed += angularAcceleration * Time.fixedDeltaTime;
                }
                else
                {
                    sweepAngularSpeed -= angularAcceleration * Time.fixedDeltaTime;
                    if (sweepAngularSpeed <= 0.0f)
                    {
                        EndSweep();
                    }
                }
                if (sweepAngularSpeed > maxAngularSpeed)
                {
                    sweepAngularSpeed = maxAngularSpeed;
                }
                if (sweepRight)
                {
                    if (sweepAngle < -180.0f)
                    {
                        EndSweep();
                    }
                }
                else
                {
                    if (sweepAngle > 180.0f)
                    {
                        EndSweep();
                    }
                }
            }
        }
        if (aboutToDestroyLaserBeam)
        {
            if ((Time.fixedTime - laserBeamReachedTarget) >= laserBeamLinger)
            {
                aboutToDestroyLaserBeam = false;
                Destroy(laserBeam);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
