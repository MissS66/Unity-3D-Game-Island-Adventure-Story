using UnityEngine;

public class FreeRoamCamera : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float upDownSpeed = 5f;
    public float lookSensitivity = 2f;

    float yaw, pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // 视角：按住鼠标右键才转
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * lookSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * lookSensitivity;
            pitch = Mathf.Clamp(pitch, -80f, 80f);
            transform.eulerAngles = new Vector3(pitch, yaw, 0);
        }

        // 前后左右移动（交给刚体处理碰撞，脚本只负责控制）
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveDir = transform.right * h + transform.forward * v;
        moveDir.y = 0;

        if (moveDir.magnitude > 0.1f)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(
                moveDir.normalized.x * moveSpeed,
                GetComponent<Rigidbody>().velocity.y,
                moveDir.normalized.z * moveSpeed
            );
        }
        else
        {
            // 不按方向键时，水平方向减速
            GetComponent<Rigidbody>().velocity = new Vector3(
                GetComponent<Rigidbody>().velocity.x * 0.9f,
                GetComponent<Rigidbody>().velocity.y,
                GetComponent<Rigidbody>().velocity.z * 0.9f
            );
        }

        // 上下飞行（Space 上，C 下）
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * upDownSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.C))
        {
            transform.Translate(Vector3.down * upDownSpeed * Time.deltaTime);
        }

        // ESC 停止运行
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    public void SetRoamActive(bool active)
    {
        enabled = active;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}