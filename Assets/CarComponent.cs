﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarComponent : MonoBehaviour
{

    public List<Wheel> wheelColliders;
    public List<GameObject> wheelObjs;

    public GameObject wheelMesh;

    public Transform wheelMeshparent;

    public float forwardTorque = 100;
    public float brakesMax = 1;

    public float maxSteer = 25;


    void Start()
    {
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));

        foreach (Wheel wheelCollider in wheelColliders)
        {
            GameObject instance = Instantiate(wheelMesh, wheelCollider.transform.position, wheelMesh.transform.rotation);
            instance.transform.parent = wheelMeshparent;
            wheelObjs.Add(instance);
            instance.transform.localScale = wheelCollider.transform.localScale;
        }
    }

    float angleLeft = 0;
    float angleRight = 0;

    void Update()
    {
        float forward = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");

        float brakes = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            brakes = brakesMax;
        }



        for(int i = 0; i < wheelObjs.Count; i++)
        {
            GameObject wheel = wheelObjs[i];
            Wheel wc = wheelColliders[i];

            Vector3 pos = wheel.transform.localPosition;
            pos.y = wc.wheelPosition;
            wheel.transform.localPosition = pos;

            wc.brakes = brakes;
            wc.torque = forward * forwardTorque;

            if (wc.isSteer)
            {
                wc.steerAngle = Mathf.Clamp(Mathf.Rad2Deg * Mathf.Asin(steer), -maxSteer, maxSteer);
                Vector3 rot = wheel.transform.localRotation.eulerAngles;
                rot.y = wc.wheelAngle;
                wheel.transform.localRotation = Quaternion.Euler(rot);
            }
        }
    }

    void FixedUpdate()
    {
        
    }
}