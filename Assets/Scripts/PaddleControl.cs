using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleControl : MonoBehaviour
{
    public bool isLeft;
    public float rotateSpeed = 80f;

    void FixedUpdate()
    {
        if (isLeft && Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, rotateSpeed * Time.fixedDeltaTime);
        }
        else if (isLeft && Input.GetKeyUp(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // ÓÒµ²°å D ¼ü
        if (!isLeft && Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, -rotateSpeed * Time.fixedDeltaTime);
        }
        else if (!isLeft && Input.GetKeyUp(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}