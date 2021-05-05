using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCarController : MonoBehaviour
{
    public PhysicsWheel[] wheels;
    // Start is called before the first frame update
    void Start()
    {
        foreach(PhysicsWheel w in wheels){
            w.carController = this;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
