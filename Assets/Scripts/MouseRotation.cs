using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    public float ySpeed;
    public float xSpeed;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        Vector3 r = transform.localRotation.eulerAngles;
        r.y += x * Time.deltaTime * xSpeed * 5;
        r.x += y * Time.deltaTime * ySpeed * 5;
        transform.localRotation = Quaternion.Euler(r);
    }
}
