using UnityEngine;

public class TopFlipperControl : MonoBehaviour
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
            // 注意：这里只声明一次 currentY
            float currentY = transform.rotation.eulerAngles.y;
            if (currentY > 180) currentY -= 360;

            if (Mathf.Abs(currentY) > 0.1f)
            {
                float backSpeed = Mathf.Sign(currentY) * -rotateSpeed * Time.deltaTime;
                transform.Rotate(0, backSpeed, 0);
            }
        }

        // 限制角度
        Vector3 euler = transform.rotation.eulerAngles;
        // 注意：这里使用计算后的 y 值，不重复声明 currentY
        float finalY = euler.y > 180 ? euler.y - 360 : euler.y;
        euler.y = Mathf.Clamp(finalY, -maxAngle, maxAngle);
        transform.rotation = Quaternion.Euler(euler);
    }
}