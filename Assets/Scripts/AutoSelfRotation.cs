using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSelfRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x + Time.deltaTime * rotationSpeed, 
            transform.rotation.eulerAngles.y + Time.deltaTime * rotationSpeed,
            transform.rotation.eulerAngles.z - Time.deltaTime * rotationSpeed));
    }
}
