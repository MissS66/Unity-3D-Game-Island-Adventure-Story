using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    [Header("摄像机列表")]
    public Camera skyCam;       // 拖入Sky_Camera
    public Camera mainCam;      // 拖入Main_Camera
    public Camera basketballCam;// 拖入basketball_camera

    [Header("切换时间")]
    public float skyCamTime = 7f;    // Sky视角持续7秒
    public float mainCamTime = 10f;  // Main视角持续10秒

    void Start()
    {
        // 初始禁用所有摄像机，只激活Sky
        DisableAllCameras();
        skyCam.gameObject.SetActive(true);
        StartCoroutine(SwitchCameraSequence());
    }

    // 自动切换摄像机协程
    IEnumerator SwitchCameraSequence()
    {
        // 1. Sky视角：7秒
        yield return new WaitForSeconds(skyCamTime);
        DisableAllCameras();
        mainCam.gameObject.SetActive(true);

        // 2. Main视角：10秒
        yield return new WaitForSeconds(mainCamTime);
        DisableAllCameras();
        basketballCam.gameObject.SetActive(true);

        // 3. 最后停在basketball视角，不再切换
    }

    // 禁用所有摄像机
    void DisableAllCameras()
    {
        if (skyCam != null) skyCam.gameObject.SetActive(false);
        if (mainCam != null) mainCam.gameObject.SetActive(false);
        if (basketballCam != null) basketballCam.gameObject.SetActive(false);
    }
}