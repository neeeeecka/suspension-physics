using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarComponent : MonoBehaviour
{

    public List<Wheel> wheelColliders;
    public List<GameObject> wheelObjs;

    public GameObject wheelMesh;

    public Transform wheelMeshparent;



    void Start()
    {
        foreach (Wheel wheelCollider in wheelColliders)
        {
            GameObject instance = Instantiate(wheelMesh, wheelCollider.transform.position, wheelMesh.transform.rotation);
            instance.transform.parent = wheelMeshparent;
            wheelObjs.Add(instance);
        }
    }

    void FixedUpdate()
    {
        int i = 0;
        foreach(GameObject wheel in wheelObjs)
        {
            Vector3 pos = wheel.transform.localPosition;
            pos.y = wheelColliders[i].wheelPosition;
            wheel.transform.localPosition = pos;
            i++;
        }
    }
}
