using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBallController : MonoBehaviour
{
    [Header("环绕灯光球（手动摆放的4个球）")]
    public Transform Sphere_revolution_back;  
    public Transform Sphere_revolution_right; 
    public Transform Sphere_revolution_left;  
    public Transform Sphere_revolution_front; 
    public Vector3 surroundCenter = new Vector3(0, 4.5f, 0); 
    public float surroundCycleTime = 3f; 
    public float surroundSelfRotateSpeed = 50f; 

    [Header("屋顶灯光球")]
    public Transform Sphere_autoroatation; 
    public float roofTopRotateSpeed = 60f; 

    [Header("屋顶内灯光球")]
    public Transform Sphere_autoroatation_first_top; 
    public float roofInnerRotateSpeed = 90f; 
    public float colorChangeInterval = 4f; 

    
    private Transform[] surroundBalls;
    private float[] surroundRadiuses;

    private Color[] lightColors = { Color.red, Color.green, Color.blue, Color.yellow };
    private int currentColorIndex = 0;
    private float colorTimer = 0f;

    void Start()
    {
        // 1. 把你手动放的4个公转球存入数组（和你的命名完全对应）
        surroundBalls = new Transform[] {
            Sphere_revolution_back,
            Sphere_revolution_right,
            Sphere_revolution_left,
            Sphere_revolution_front
        };

        // 2. 自动计算每个球的初始环绕半径（适配不同半径）
        surroundRadiuses = new float[4];
        for (int i = 0; i < 4; i++)
        {
            if (surroundBalls[i] != null)
            {
                // 计算球到环绕中心的水平距离（Y轴高度不变）
                Vector3 horizontalPos = new Vector3(
                    surroundBalls[i].position.x - surroundCenter.x,
                    0,
                    surroundBalls[i].position.z - surroundCenter.z
                );
                surroundRadiuses[i] = horizontalPos.magnitude; // 半径=距离

                // 给公转球添加初始黄色灯光（如果没有）
                SetLightColor(surroundBalls[i], Color.yellow);
            }
        }

        // 3. 初始化屋顶内灯光球颜色
        if (Sphere_autoroatation_first_top != null)
            SetLightColor(Sphere_autoroatation_first_top, lightColors[currentColorIndex]);

        // 4. 给屋顶上方自转球也加上初始颜色（黄色），让它发光可见
        if (Sphere_autoroatation != null)
            SetLightColor(Sphere_autoroatation, Color.yellow);
    }

    void Update()
    {
        // 1. 控制4个手动摆放的公转球（按各自半径环绕+变色）
        UpdateSurroundBalls();

        // 2. 控制屋顶上方自转球
        if (Sphere_autoroatation != null)
        {
            // 绕 Y 轴自转，速度提高到 60°/秒，更容易观察
            Sphere_autoroatation.Rotate(Vector3.up, roofTopRotateSpeed * Time.deltaTime);
            // 给屋顶上的自转球也加一个缓慢颜色变化（可选，让它更显眼）
            float topColorPhase = (Time.time * 0.2f) % 4f;
            int topColorIndex = Mathf.FloorToInt(topColorPhase);
            Color topColor = Color.Lerp(
                lightColors[topColorIndex],
                lightColors[(topColorIndex + 1) % 4],
                topColorPhase - topColorIndex
            );
            SetLightColor(Sphere_autoroatation, topColor);
        }

        // 3. 控制屋顶内自变色球
        if (Sphere_autoroatation_first_top != null)
        {
            Sphere_autoroatation_first_top.Rotate(Vector3.up, roofInnerRotateSpeed * Time.deltaTime);

            // 颜色切换逻辑
            colorTimer += Time.deltaTime;
            if (colorTimer >= colorChangeInterval)
            {
                colorTimer = 0f;
                currentColorIndex = (currentColorIndex + 1) % lightColors.Length;
                SetLightColor(Sphere_autoroatation_first_top, lightColors[currentColorIndex]);
            }
        }
    }

    // 更新4个公转球的位置（按各自半径环绕）+ 颜色渐变
    void UpdateSurroundBalls()
    {
        float angularSpeed = 360f / surroundCycleTime; // 角速度（°/秒）
        float currentAngle = angularSpeed * Time.time;  // 当前总角度

        for (int i = 0; i < 4; i++)
        {
            if (surroundBalls[i] == null) continue; // 跳过空物体

            // 每个球的初始角度偏移（保持4个球均匀分布）
            float angleOffset = i * 90f; // 0°、90°、180°、270°
            float totalAngle = (currentAngle + angleOffset) * Mathf.Deg2Rad;

            // 计算新位置（用各自的半径，Y轴保持初始高度）
            Vector3 newPos = new Vector3(
                surroundCenter.x + Mathf.Cos(totalAngle) * surroundRadiuses[i],
                surroundBalls[i].position.y, // 保持你手动设置的Y高度
                surroundCenter.z + Mathf.Sin(totalAngle) * surroundRadiuses[i]
            );

            // 更新位置+自转
            surroundBalls[i].position = newPos;
            surroundBalls[i].Rotate(Vector3.up, surroundSelfRotateSpeed * Time.deltaTime);

            // 公转球颜色渐变逻辑（每个球错开相位，颜色过渡更自然）
            float colorPhase = (Time.time + i * 0.5f) % 4f;
            int colorIndex = Mathf.FloorToInt(colorPhase);
            Color ballColor = Color.Lerp(
                lightColors[colorIndex],
                lightColors[(colorIndex + 1) % 4],
                colorPhase - colorIndex
            );
            SetLightColor(surroundBalls[i], ballColor);
        }
    }

    // 设置灯光球颜色（自动添加Light组件）
    void SetLightColor(Transform ball, Color color)
    {
        Light light = ball.GetComponent<Light>();
        if (light == null)
        {
            light = ball.gameObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.range = 20f; // 进一步增大光照范围
            light.intensity = 12f; // 进一步提高光照强度
            light.bounceIntensity = 1f; // 增强环境光反射
            light.shadows = LightShadows.Soft; // 开启软阴影，让发光更真实
        }
        light.color = color;

        // 设置材质颜色（让球体本身也变色，更容易观察旋转）
        Renderer renderer = ball.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
            // 给材质加自发光，让球体本身发光，不依赖场景光照
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", color * 0.5f);
        }
    }
}