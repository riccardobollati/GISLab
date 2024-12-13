using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PointController : MonoBehaviour
{
    private Vector3 lastPosition; // Stores the position in the previous frame
    private Rigidbody rb;
    private bool flag = true;// Reference to the Rigidbody component

    void Start()
    {
        // Initialize the last position and get the Rigidbody component
        lastPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached to the Point GameObject. Please add one.");
        }
    }

    void Update()
    {
        if (flag)
        {
            if (transform.position != lastPosition)
            {
                lastPosition = transform.position;
            }
            else
            {
                Destroy(rb);
                flag = false;
            }
        }           
    }
}
