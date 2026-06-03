using UnityEngine;

public class PoliceLightController : MonoBehaviour
{
    [Header("灯光组件")]
    public Light redLight;
    public Light blueLight;

    [Header("灯模型（可选）")]
    public Renderer redRenderer;
    public Renderer blueRenderer;

    [Header("材质（可选）")]
    public Material redOnMat;
    public Material redOffMat;
    public Material blueOnMat;
    public Material blueOffMat;

    private int mode = 0; // 0关 1闪 2常亮
    private float timer;
    public float blinkInterval = 0.25f;
    private bool blinkState;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            mode = (mode + 1) % 3;
            timer = 0;
            blinkState = false;
            ApplyState();
        }

        if (mode == 1)
        {
            timer += Time.deltaTime;
            if (timer > blinkInterval)
            {
                timer = 0;
                blinkState = !blinkState;
                SetLights(blinkState);
            }
        }
    }

    void ApplyState()
    {
        if (mode == 0) SetLights(false);
        if (mode == 2) SetLights(true);
    }

    void SetLights(bool on)
    {
        if (redLight) redLight.enabled = on;
        if (blueLight) blueLight.enabled = on;
    }
}