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


    void Start()
    {

    }


    void UpdateWheel(int i)
    {
        Transform wheelModel = wheelModels[i];
        PhysicsWheel wheel = wheels[i];

        Vector3 wheelPos = wheelModel.localPosition;
        wheelPos.y = -wheel.springStretch + wheel.radius;
        wheelModel.localPosition = wheelPos;

        Vector3 wheelRot = wheelModel.localRotation.eulerAngles;
        // wheelRot
        wheelModel.Rotate(Vector3.right * wheel.RPM * 6 * Time.deltaTime, Space.Self);

        // wheelModel.transform.localRotation = Quaternion.Euler(wheelRot);
    }

    void FixedUpdate()
    {
        for (int i = 0; i < wheels.Count; i++)
        {
            UpdateWheel(i);
        }
    }
}
