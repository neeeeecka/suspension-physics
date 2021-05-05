using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEtension : MonoBehaviour
{
    public bool isTouching;

    public Vector3 startLocalPos;
    Vector3 modLocalPos;
    private void Start()
    {
        startLocalPos = transform.localPosition;
        modLocalPos = new Vector3(startLocalPos.x, startLocalPos.y, startLocalPos.z + 0.0001f);
        Debug.Log(startLocalPos);
    }
    private void Update()
    {
        transform.localPosition = transform.localPosition.z == startLocalPos.z ? modLocalPos : startLocalPos;
    }

    private void OnCollisionEnter(Collision other)
    {
        isTouching = true;
    }
    private void OnCollisionExit(Collision other)
    {
        isTouching = false;
    }
}
