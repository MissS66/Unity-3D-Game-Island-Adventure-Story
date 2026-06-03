using UnityEngine;

public class RightFlipperControl : MonoBehaviour
{
    public float rotateSpeed = 300f;
    public float maxAngle = 30f;

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
        else
        {
            // 第一次用 currentZ，保留
            float currentZ = transform.rotation.eulerAngles.z;
            if (currentZ > 0 && currentZ < 180)
            {
                transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            }
        }

        // 限制角度时，改名避免重复：currentZ → clampedZ
        Vector3 euler = transform.rotation.eulerAngles;
        float clampedZ = euler.z > 180 ? euler.z - 360 : euler.z;
        euler.z = Mathf.Clamp(clampedZ, 0.0f, maxAngle);
        transform.rotation = Quaternion.Euler(euler);
    }
}