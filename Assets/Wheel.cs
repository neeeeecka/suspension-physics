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

    public float weight;
    public Rigidbody car;

    public LayerMask hitLayer;
    private float minLength = 0;
    private float maxLength = 0;

    public float sidewaysFriction = 1;
    public float forwardFriction = 1;
    public Vector3 frictionForce;
    public Vector3 wheelVelocity;

    public float brakes = 1;

    public float torque = 0;
    public float RPM = 0;

    public float steerAngle = 0;
    public float wheelAngle = 0;
    public bool isSteer;
    public float steerSpeed = 10;

    public float intraFriction = 100;

    public bool drive = false;

    void Start()
    {
        minLength = -suspensionDistance / 2;
        maxLength = suspensionDistance / 2;
        car = transform.root.GetComponent<Rigidbody>();
        //weight = car.centerOfMass * car.mass;
    }

    public float lastContactDepth = 0;
    public float contactSpeed = 0;

    private Vector3 lastForwardVelocity;

    void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(transform.up * wheelAngle);
    }

    Vector3 lastPosition;

    void FixedUpdate()
    {
        //Vector3 dist = (transform.localPosition - car.centerOfMass);
        //dist.y = 0;
        //Debug.Log(dist.magnitude);
        //weight = dist.magnitude * car.mass;

        wheelVelocity = (transform.position - lastPosition) * Time.fixedDeltaTime;
        lastPosition = transform.position;

        RPM = (wheelVelocity.x - lastForwardVelocity.x) / wheelRadius;

        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up * wheelRadius, Color.red);
        if (Physics.Raycast(transform.position, -transform.up, out hit, wheelRadius, hitLayer))
        {

            lastContactDepth = contactDepth;

            contactDepth = Mathf.Clamp(hit.distance - wheelRadius, minLength, maxLength);
            contactSpeed = (lastContactDepth - contactDepth) / Time.fixedDeltaTime;

            //f = -kx
            float springForceScalar = -contactDepth * stiffness + damper * contactSpeed;
            Vector3 springForce = transform.up * springForceScalar;

            wheelPosition = -contactDepth;

            Vector3 forwardForce = (torque / wheelRadius) * transform.forward;
            Vector3 wv = wheelVelocity;

            Vector3 sideForce = wv * sidewaysFriction;
            Debug.DrawRay(hit.point, sideForce, Color.green);

            car.AddForceAtPosition(springForce + forwardForce, hit.point);


        }
    }
}