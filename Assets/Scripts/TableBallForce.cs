using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallForce : MonoBehaviour
{
    public float power = 300f;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(1, 1, 1) * power);
    }
}