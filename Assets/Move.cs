using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        forward.y = 0;

        rb.velocity = forward * speed * z + transform.right * speed * x;
    }
}
