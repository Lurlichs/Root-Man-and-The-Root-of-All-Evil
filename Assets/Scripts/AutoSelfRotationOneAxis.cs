using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSelfRotationOneAxis : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + Time.deltaTime * rotationSpeed));
    }
}
