using UnityEngine;

public class BackFlipperControl : MonoBehaviour
{
    public float rotateSpeed = 300f;
    public float maxAngle = 30f;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
        }
        else
        {
            float currentY = transform.rotation.eulerAngles.y;
            if (currentY > 180) currentY -= 360;

            if (Mathf.Abs(currentY) > 0.1f)
            {
                float backSpeed = Mathf.Sign(currentY) * -rotateSpeed * Time.deltaTime;
                transform.Rotate(0, backSpeed, 0);
            }
        }

        // ÏÞÖÆ½Ç¶È
        Vector3 euler = transform.rotation.eulerAngles;
        float finalY = euler.y > 180 ? euler.y - 360 : euler.y;
        euler.y = Mathf.Clamp(finalY, -maxAngle, maxAngle);
        transform.rotation = Quaternion.Euler(euler);
    }
}