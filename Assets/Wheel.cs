using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float stiffness = 1;
    public float suspensionDistance = 1;
    public float contactDepth = 0;

    public float wheelMass = 20;

    public float wheelPosition = 0;

    public float wheelRadius = 1;
    public float damper = 1;

    public Vector3 weight;
    public Rigidbody car;

    public LayerMask hitLayer;
    private float minLength = 0;
    private float maxLength = 0;

    public float sidewaysFriction = 1;
    public Vector3 frictionForce;
    public Vector3 forwardVelocity;

    void Start()
    {
        minLength = -suspensionDistance / 2;
        maxLength = suspensionDistance / 2;
        car = transform.root.GetComponent<Rigidbody>();
        weight = car.centerOfMass * car.mass;
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

            //k = -fx
            float k = -contactDepth * stiffness + damper * contactSpeed;
            Vector3 springForce = transform.up * k;

            wheelPosition = -contactDepth;

            frictionForce = car.transform.TransformDirection(
                car.transform.right * 
                sidewaysFriction * 
                car.transform.InverseTransformDirection(car.velocity).z
            );

            car.AddForceAtPosition(springForce - frictionForce * sidewaysFriction, transform.position);

        }
    }
}
