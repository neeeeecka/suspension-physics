using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float stiffness = 1;
    public float suspensionDistance = 1;
    public float contactDepth = 0;

    public float wheelRadius = 1;
    public float damper = 1;

    public Vector3 weight;
    public Rigidbody main;

    public LayerMask hitLayer;
    private float minLength = 0;
    private float maxLength = 0;


    void Start()
    {
        minLength = -suspensionDistance / 2;
        maxLength = suspensionDistance / 2;
        main = transform.root.GetComponent<Rigidbody>();
        weight = main.centerOfMass * main.mass;
    }

    public float lastContactDepth = 0;
    public float contactSpeed = 0;

    void FixedUpdate()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up * wheelRadius, Color.red);
        if (Physics.Raycast(transform.position, -transform.up, out hit, wheelRadius, hitLayer))
        {
            lastContactDepth = contactDepth;

            contactDepth = Mathf.Clamp(hit.distance - wheelRadius, minLength, maxLength);
            contactSpeed = (lastContactDepth - contactDepth) / Time.fixedDeltaTime;

            float springForce = -contactDepth * stiffness + damper * contactSpeed;
            main.AddForceAtPosition(transform.up * springForce, transform.position);

        }
    }
}
