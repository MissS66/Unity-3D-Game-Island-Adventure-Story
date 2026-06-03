using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorController : MonoBehaviour
{
    [Header("门参数设置")]
    public Transform door_left;
    public Transform door_right;
    public Vector3 leftDoorPivot = new Vector3(-3.2f, 1f, 1f);
    public Vector3 rightDoorPivot = new Vector3(-3.2f, 1f, -1f);
    public float openDuration = 5f;
    public float targetAngle = 90f;

    private float currentTime = 0f;
    private bool isOpening = true;

    void Start()
    {
        //  加空值判断：只有赋值了才初始化旋转
        if (door_left != null)
            door_left.rotation = Quaternion.identity;
        if (door_right != null)
            door_right.rotation = Quaternion.identity;
    }

    void Update()
    {
        
        if (!isOpening || door_left == null || door_right == null)
            return;

        if (currentTime < openDuration)
        {
            currentTime += Time.deltaTime;
            float progress = currentTime / openDuration;

            float leftAngle = Mathf.Lerp(0, targetAngle, progress);
            door_left.RotateAround(leftDoorPivot, Vector3.up, leftAngle - door_left.rotation.eulerAngles.y);

            float rightAngle = Mathf.Lerp(0, -targetAngle, progress);
            door_right.RotateAround(rightDoorPivot, Vector3.up, rightAngle - door_right.rotation.eulerAngles.y);

            if (currentTime >= openDuration)
                isOpening = false;
        }
    }
}