using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    [Header("移动")]
    public float moveSpeed = 4f;
    public float boostSpeed = 7f;
    public float friction = 5f;

    [Header("跳跃（长按）")]
    public float jumpForce = 5f;
    public float maxJumpCharge = 2f;
    private float jumpCharge = 0f;

    [Header("投篮")]
    public float maxShootForce = 20f;
    public float chargeSpeed = 10f;
    public float aimSensitivity = 2f;

    [Header("引用")]
    public LineRenderer aimLine;
    public Slider forceSlider;

    private Rigidbody rb;
    private Camera cam;

    private bool isGrounded;
    private bool isCharging;
    private float currentCharge;

    private Vector2 mouseStart;
    private Vector3 aimDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        // 更真实篮球手感
        rb.mass = 0.6f;
        rb.drag = 0.2f;
        rb.angularDrag = 0.05f;

        // ==============================================
        // 加了空判断！没赋值也不会报错
        // ==============================================
        if (aimLine != null)
        {
            aimLine.positionCount = 30;
            aimLine.enabled = false;
        }

        if (forceSlider != null)
        {
            forceSlider.minValue = 0;
            forceSlider.maxValue = maxShootForce;
            forceSlider.value = 0;
            forceSlider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!enabled) return;  // <--- 加这一行！禁用时完全不运行
        GroundCheck();
        Move();
        JumpControl();
        ShootControl();
    }

    // =============================
    // 地面检测
    // =============================
    void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    // =============================
    // 移动
    // =============================
    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;
        float speed = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : moveSpeed;

        if (dir.magnitude > 0.1f)
        {
            Vector3 vel = dir * speed;
            vel.y = rb.velocity.y;
            rb.velocity = vel;
        }
        else
        {
            Vector3 horizontal = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            horizontal = Vector3.Lerp(horizontal, Vector3.zero, friction * Time.deltaTime);
            rb.velocity = new Vector3(horizontal.x, rb.velocity.y, horizontal.z);
        }
    }

    // =============================
    // 跳跃（支持长按）
    // =============================
    void JumpControl()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            jumpCharge += Time.deltaTime;
            jumpCharge = Mathf.Clamp(jumpCharge, 0, maxJumpCharge);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isGrounded)
        {
            float finalForce = jumpForce + jumpCharge * 5f;
            rb.velocity = new Vector3(rb.velocity.x, finalForce, rb.velocity.z);
            jumpCharge = 0f;
        }
    }

    // =============================
    // 投篮系统
    // =============================
    void ShootControl()
    {
        // 开始蓄力
        if (Input.GetMouseButtonDown(0) && isGrounded)
        {
            isCharging = true;
            currentCharge = 0;
            mouseStart = Input.mousePosition;

            if (aimLine != null) aimLine.enabled = true;
            if (forceSlider != null) forceSlider.gameObject.SetActive(true);
        }

        // 蓄力 + 瞄准
        if (Input.GetMouseButton(0) && isCharging)
        {
            currentCharge += chargeSpeed * Time.deltaTime;
            currentCharge = Mathf.Clamp(currentCharge, 0, maxShootForce);

            if (forceSlider != null) forceSlider.value = currentCharge;

            Aim();
            DrawTrajectory();
        }

        // 发射
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            rb.velocity *= 0.2f;
            rb.AddForce(aimDir * currentCharge, ForceMode.Impulse);
            rb.AddTorque(transform.right * -5f, ForceMode.Impulse);

            isCharging = false;

            if (aimLine != null) aimLine.enabled = false;
            if (forceSlider != null) forceSlider.gameObject.SetActive(false);
        }
    }

    // =============================
    // 瞄准（基于摄像机）
    // =============================
    void Aim()
    {
        Vector2 delta = (Vector2)Input.mousePosition - mouseStart;
        delta *= aimSensitivity * 0.1f;

        Vector3 baseDir = cam.transform.forward;
        baseDir = Quaternion.Euler(0, delta.x, 0) * baseDir;
        baseDir = Quaternion.Euler(-delta.y, 0, 0) * baseDir;
        aimDir = baseDir.normalized;
    }

    // =============================
    // 抛物线预测
    // =============================
    void DrawTrajectory()
    {
        if (aimLine == null) return; // 加了空判断

        int steps = 30;
        float stepTime = 0.1f;

        for (int i = 0; i < steps; i++)
        {
            float t = i * stepTime;

            Vector3 pos =
                transform.position +
                aimDir * currentCharge * t +
                0.5f * Physics.gravity * t * t;

            aimLine.SetPosition(i, pos);
        }
    }
}