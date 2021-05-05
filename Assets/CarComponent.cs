using System.Collections;
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

    public Rigidbody car;
    public Transform com;

    void Start()
    {

    }

    float angleLeft = 0;
    float angleRight = 0;

    void Update()
    {

    }

    void FixedUpdate()
    {

    }
}
