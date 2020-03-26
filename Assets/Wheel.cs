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
    public float forwardFriction = 1;
    public Vector3 frictionForce;
    public Vector3 forwardVelocity;

    public float brakes = 1;

    public float torque = 0;
    public float RPM = 0;

    public float steerAngle = 0;
    public float wheelAngle = 0;
    public bool isSteer;
    public float steerSpeed = 10;

    void Start()
    {
        minLength = -suspensionDistance / 2;
        maxLength = suspensionDistance / 2;
        car = transform.root.GetComponent<Rigidbody>();
        weight = car.centerOfMass * car.mass;
    }

    public float lastContactDepth = 0;
    public float contactSpeed = 0;

    private Vector3 lastForwardVelocity;

    void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(transform.up * wheelAngle);
    }

    void FixedUpdate()
    {
        Vector3 localVelocity = car.transform.InverseTransformDirection(car.velocity);
        Vector3 localAngularVelocity = car.transform.InverseTransformDirection(car.angularVelocity);

        RPM = (localVelocity.x - lastForwardVelocity.x) / wheelRadius;

        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up * wheelRadius, Color.red);
        if (Physics.Raycast(transform.position, -transform.up, out hit, wheelRadius, hitLayer))
        {

            lastContactDepth = contactDepth;

            contactDepth = Mathf.Clamp(hit.distance - wheelRadius, minLength, maxLength);
            contactSpeed = (lastContactDepth - contactDepth) / Time.fixedDeltaTime;

            //f = -kx
            float f = -contactDepth * stiffness + damper * contactSpeed;
            Vector3 springForce = transform.up * f;

            wheelPosition = -contactDepth;

            Vector3 sidewaysFrictionForce = car.transform.TransformDirection(
                Vector3.forward *
                -sidewaysFriction *
                localVelocity.z
            );
            Vector3 forwardFrictionForce = car.transform.TransformDirection(
                 Vector3.right *
                 -forwardFriction *
                 localVelocity.x
            ) * brakes;
            Vector3 angularFrictionForce =
                Vector3.forward *
                sidewaysFriction *
                localAngularVelocity.y
           ;
            Vector3 forwardForce = car.transform.right * torque;

            Vector3 angularVeolcity = car.angularVelocity;
            angularVeolcity.y = 0;
            car.angularVelocity = angularVeolcity;

            frictionForce = sidewaysFrictionForce + forwardFrictionForce;

            Debug.DrawRay(transform.position, frictionForce, Color.blue);

            car.AddForceAtPosition(springForce + frictionForce + forwardForce, transform.position);
            forwardVelocity = car.velocity;
            lastForwardVelocity = localVelocity;

        }
    }
}