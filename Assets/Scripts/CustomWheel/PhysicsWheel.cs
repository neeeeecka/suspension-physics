using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWheel : MonoBehaviour
{
    public PhysicsCarController carController;
    public Rigidbody rigidbody;
    public float stiffness = 0.5f;
    public float damping = 0.5f;
    public float radius = 1;
    public float springLength = 0.5f;
    public float wheelFriction = 100;

    public ColliderEtension cylinderCol;

    public WheelFrictionCurveSource m_forwardFriction; //Properties of tire friction in the direction the wheel is pointing in.
    public float internalFriction = 1;
    public float RPM = 0;
    // public Transform wheelModel;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = transform.parent.GetComponent<Rigidbody>();
        m_forwardFriction = new WheelFrictionCurveSource();

    }

    RaycastHit hit;
    float hitDistance;

    Vector3 lastPosition;

    public float springStretch
    {
        get
        {
            return hitDistance;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // Debug.Log(cylinderCol.isTouching);
        Vector3 forceDirection = Vector3.zero;
        float contactSpeed = 0;
        //if (Physics.SphereCast(transform.position, radius, -transform.up, out hit, radius / 2))

        if (Physics.Raycast(transform.position, -transform.up, out hit, springLength))
        {
            // Debug.Log(transform.name + " has hit");
            contactSpeed = (hit.distance - hitDistance) / Time.deltaTime;
            // Debug.Log(contactSpeed);
            hitDistance = hit.distance;
            // forceDirection = (hit.point - transform.position).normalized;
        }
        else
        {
            //resting position = radius
            hitDistance = springLength;
        }
        Debug.DrawRay(transform.position, -transform.up * hitDistance, Color.red);

        //if is grounded
        if (hitDistance != springLength)
        {

            Vector3 totalForce = Vector3.zero;
            // Vector3 force = stiffness * forceDirection * Mathf.Pow(hitDistance, 2);

            Vector3 v = rigidbody.velocity;

            //x is relative to resting position
            float x = springLength - hitDistance;

            Vector3 force = stiffness * x * transform.up - contactSpeed * transform.up * damping;

            // Vector3 force = stiffness * x * transform.up - v * damping;
            // Debug.Log(v.y);
            // rigidbody.AddForceAtPosition(force, transform.position);
            totalForce += force;

            // Vector3 contactPoint = transform.localPosition;
            // contactPoint.y = transform.localPosition.y - hitDistance;
            // Vector3 worldPoint = rigidbody.transform.TransformPoint(contactPoint);

            // Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
            // lastPosition = transform.position;

            // float friction = wheelFriction * rigidbody.mass / 4;

            // Vector3 frictionForce = -velocity.normalized * friction;
            // frictionForce.y = 0;

            // totalForce += frictionForce;

            CalculateSlips();
            totalForce += 0.1f * wheelFriction * transform.forward * Mathf.Sign(m_forwardSlip) * m_forwardFriction.Evaluate(m_forwardSlip);
            totalForce -= wheelFriction * transform.right * Mathf.Sign(m_sidewaysSlip) * m_forwardFriction.Evaluate(m_sidewaysSlip);

            RPM = -m_forwardSlip / (2 * Mathf.PI * radius) * 60; //rounds per second * 60 = rp minute 

            // Debug.Log(m_forwardSlip);
            // rigidbody.AddForceAtPosition(frictionForce * Time.deltaTime, worldPoint);
            rigidbody.AddForceAtPosition(totalForce, transform.position);

            // Debug.Log(force.y);
        }
        else
        {
            RPM /= 1 + internalFriction;
        }
    }

    public float forwardVel;

    private float m_forwardSlip;
    private float m_sidewaysSlip;
    private void CalculateSlips()
    {
        //Calculate the wheel's linear velocity
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        //Store the forward and sideways direction to improve performance
        Vector3 forward = transform.forward;
        Vector3 sideways = -transform.right;

        // Debug.Log(transform.forward);

        //Calculate the forward and sideways velocity components relative to the wheel in world space
        Vector3 forwardVelocity = Vector3.Dot(velocity, forward) * forward;
        Vector3 sidewaysVelocity = Vector3.Dot(velocity, sideways) * sideways;

        // forwardVel = forwardVelocity.magnitude ;

        //Calculate the slip velocities. 
        //Note that these values are different from the standard slip calculation.
        m_forwardSlip = -Mathf.Sign(Vector3.Dot(forward, forwardVelocity)) * forwardVelocity.magnitude;
        m_sidewaysSlip = -Mathf.Sign(Vector3.Dot(sideways, sidewaysVelocity)) * sidewaysVelocity.magnitude;


        // Debug.Log(sidewaysVelocity + " -> " + m_sidewaysSlip);

    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Vector3 point = transform.localPosition;
        point.y = transform.localPosition.y - hitDistance;

        Gizmos.DrawSphere(rigidbody.transform.TransformPoint(point), 0.1f);
        // Gizmos.DrawSphere(transform.position, radius);

    }
}
