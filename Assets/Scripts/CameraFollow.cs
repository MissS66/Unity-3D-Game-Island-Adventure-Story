using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("跟随目标")]
    [SerializeField] private Transform target;

    [Header("普通跟车（保留原有效果）")]
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
    [SerializeField] private float smoothSpeed = 0.125f;

    [Header("终点拉远")]
    [SerializeField] private Vector3 endWorldOffset = new Vector3(0, 50, -100);
    [SerializeField] private float endSmoothSpeed = 0.03f;
    [SerializeField] private float endLookAtHeight = 2f;

    [Header("调试")]
    [SerializeField] private bool isEnding = false;

    private void LateUpdate()
    {
        if (target == null) return;

        if (!isEnding)
        {
            // ===== 普通阶段：完全保留你原来的跟车效果 =====
            Vector3 desiredPosition = target.position + target.TransformDirection(offset);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // 保持原来的代入感：一直看车
            transform.LookAt(target);
        }
        else
        {
            // ===== 终点阶段：切换到全景拉远 =====
            Vector3 desiredPosition = target.position + endWorldOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, endSmoothSpeed);
            transform.position = smoothedPosition;

            // 拉远后仍然看向车，稍微抬高一点更自然
            Vector3 lookTarget = target.position + Vector3.up * endLookAtHeight;
            transform.LookAt(lookTarget);
        }
    }

    public void SetEndingView()
    {
        isEnding = true;
    }
}