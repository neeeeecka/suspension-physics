using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarComponent : MonoBehaviour
{
    public List<PhysicsWheel> wheels;
    public List<Transform> wheelModels;
    public Transform wheelMeshparent;

    public float forwardTorque = 100;
    public float brakesMax = 1;

    public float maxSteer = 25;
    public Transform com;

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = com.localPosition;
    }


    void UpdateWheel(int i)
    {
        Transform wheelModel = wheelModels[i];
        PhysicsWheel wheel = wheels[i];

        Vector3 wheelPos = wheelModel.localPosition;
        wheelPos.y = -wheel.lastHitDistance + wheel.radius;
        wheelModel.localPosition = wheelPos;

        Vector3 wheelRot = wheelModel.localRotation.eulerAngles;
        // wheelRot
        wheelModel.Rotate(Vector3.right * wheel.RPM * 6 * Time.deltaTime, Space.Self);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        wheel.drift = 1;

        if (i < 2)
        {
            wheel.wheelAngle = horizontal * maxSteer;
        }

        if (i < 2)
        {
            wheel.torque = forwardTorque * vertical;
        }


        wheel.brakeTorque = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            wheel.brakeTorque = brakesMax;
        }
        // wheelModel.transform.localRotation = Quaternion.Euler(wheelRot);
    }

    void Update()
    {
        for (int i = 0; i < wheels.Count; i++)
        {
            UpdateWheel(i);
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 r = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(0, r.y, 0);
        }

        // Debug.Log(horizontal);

    }
}
