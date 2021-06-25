using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWheel : MonoBehaviour
{
    public Rigidbody body;
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
        body = transform.parent.GetComponent<Rigidbody>();
        m_forwardFriction = new WheelFrictionCurveSource();

    }

    RaycastHit hit;
    float hitDistance;

    Vector3 lastPosition;
    public float maxCompression = 0.2f;
    public float torque = 0;

    public float brakeTorque;
    private float angluarVeolcity;

    public float springStretch
    {
        get
        {
            return hitDistance;
        }
    }

    public float wheelAngle = 0;
    float groundFriction = 0;
    public float maxGroundFriction = 1;

    // Update is called once per frame
    public float drift = 0;
static float Sign(float number) {
      return number < 0 ? -1 : (number > 0 ? 1 : 0);
  }
    void FixedUpdate()
    {

        // Debug.Log(cylinderCol.isTouching);
        Vector3 forceDirection = Vector3.zero;
        float contactSpeed = 0;

        if (Physics.Raycast(transform.position, -transform.up, out hit, springLength))

        // if (Physics.SphereCast(transform.position, radius, -transform.up, out hit, radius / 2))
        {
            // Debug.Log(transform.name + " has hit");
            contactSpeed = (hit.distance - hitDistance) / Time.deltaTime;
            // Debug.Log(contactSpeed);
            hitDistance = Mathf.Max(hit.distance, maxCompression);
            // forceDirection = (hit.point - transform.position).normalized;
        }
        else
        {
            //resting position = springlen
            hitDistance = springLength;
        }
        // Debug.DrawRay(transform.position, -transform.up * hitDistance, Color.red);


        groundFriction = 0;
        Vector3 totalForce = Vector3.zero;

        //if is grounded
        if (hitDistance != springLength)
        {
            // Vector3 force = stiffness * forceDirection * Mathf.Pow(hitDistance, 2);
            Vector3 v = body.velocity;

            //x is relative to resting position
            float x = springLength - hitDistance;

            Vector3 force = stiffness * x * transform.up - contactSpeed * transform.up * damping;

            totalForce += force;

            // angluarVeolcity += ((torque / radius)) * Time.deltaTime;

            CalculateSlips();
          
            // totalForce += ;

            // Vector3 forwardForce = transform.forward * Mathf.Sign(m_forwardSlip) * m_forwardFriction.Evaluate(m_forwardSlip);
            groundFriction = body.mass * maxGroundFriction;
            Vector3 forwardForce = transform.forward * Sign(torque) * groundFriction;
            // Debug.Log(forwardForce + " - " + Sign(torque));
            totalForce += forwardForce + Sign(m_forwardSlip) * brakeTorque * transform.forward;

            totalForce -= transform.right * Mathf.Sign(m_sidewaysSlip) * m_forwardFriction.Evaluate(m_sidewaysSlip);

            RPM = -m_forwardSlip / (2 * Mathf.PI * radius) * 60;
        }
        else
        {
            RPM /= 1 + internalFriction;
        }

        // totalForce += 
        body.AddForceAtPosition(totalForce, transform.position);

        //angluarVeolcity -= Mathf.Sign(angluarVeolcity) * Mathf.Min(Mathf.Abs(angluarVeolcity), brakeTorque * radius * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(0, wheelAngle, 0);
    }

    public float forwardVel;

    private float m_forwardSlip, r_forwardSlip;
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

        //r_forwardSlip = m_forwardSlip + (angluarVeolcity * Mathf.PI / 180.0f * radius);
    }

    public float lastHitDistance;
    void Update()
    {
        if (Physics.Raycast(transform.position, -transform.up, out hit, springLength))
        {

            lastHitDistance = Mathf.Clamp(hit.distance, maxCompression, hit.distance);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Vector3 point = transform.localPosition;
        point.y = transform.localPosition.y - hitDistance;

        Gizmos.DrawSphere(body.transform.TransformPoint(point), 0.1f);

        Gizmos.color = Color.green;
        //  Gizmos.DrawSphere(transform.position, radius);

    }
}
