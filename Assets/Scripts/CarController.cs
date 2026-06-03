using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("=== 车轮碰撞体引用（物理驱动） ===")]
    public WheelCollider frontLeftWheelCol;
    public WheelCollider frontRightWheelCol;
    public WheelCollider rearLeftWheelCol;
    public WheelCollider rearRightWheelCol;

    [Header("=== 车轮模型引用（视觉旋转） ===")]
    public Transform frontLeftWheelMesh;
    public Transform frontRightWheelMesh;
    public Transform rearLeftWheelMesh;
    public Transform rearRightWheelMesh;

    [Header("=== 车辆性能参数 ===")]
    [Tooltip("最大油门扭矩，数值越大车越快")] public float motorTorque = 1200f;
    [Tooltip("最大转向角度，数值越大转向越灵")] public float maxSteerAngle = 30f;
    [Tooltip("刹车力，数值越大刹车越猛")] public float brakeTorque = 3000f;

    [Header("氮气加速设置")]
    public float boostMultiplier = 2f;
    public float boostDuration = 3f;
    public float boostCooldown = 5f;

    private float currentBoostTime;
    private float currentCooldownTime;
    private bool isBoosting;

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBrakeTorque;

    private void FixedUpdate()
    {
        GetPlayerInput();
        SteerCar();
        AccelerateCar();
        UpdateWheelMeshes();
    }

    private void GetPlayerInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // 氮气加速
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentCooldownTime <= 0)
        {
            isBoosting = true;
            currentBoostTime = boostDuration;
            currentCooldownTime = boostCooldown;
        }
    }

    private void SteerCar()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCol.steerAngle = currentSteerAngle;
        frontRightWheelCol.steerAngle = currentSteerAngle;
    }

    private void AccelerateCar()
    {
        // ===== 氮气加速计算 =====
        float currentMultiplier = 1f;

        if (isBoosting)
        {
            currentMultiplier = boostMultiplier;
            currentBoostTime -= Time.fixedDeltaTime;
            if (currentBoostTime <= 0)
            {
                isBoosting = false;
            }
        }

        if (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.fixedDeltaTime;
        }

        // ===== 油门 / 倒车 / 刹车 =====
        if (verticalInput != 0)
        {
            // 应用加速倍数
            frontLeftWheelCol.motorTorque = verticalInput * motorTorque * currentMultiplier;
            frontRightWheelCol.motorTorque = verticalInput * motorTorque * currentMultiplier;
            rearLeftWheelCol.motorTorque = verticalInput * motorTorque * currentMultiplier;
            rearRightWheelCol.motorTorque = verticalInput * motorTorque * currentMultiplier;
            currentBrakeTorque = 0;
        }
        else
        {
            // 没输入 → 刹车
            frontLeftWheelCol.motorTorque = 0;
            frontRightWheelCol.motorTorque = 0;
            rearLeftWheelCol.motorTorque = 0;
            rearRightWheelCol.motorTorque = 0;
            currentBrakeTorque = brakeTorque;
        }

        // 应用刹车
        frontLeftWheelCol.brakeTorque = currentBrakeTorque;
        frontRightWheelCol.brakeTorque = currentBrakeTorque;
        rearLeftWheelCol.brakeTorque = currentBrakeTorque;
        rearRightWheelCol.brakeTorque = currentBrakeTorque;
    }

    private void UpdateWheelMeshes()
    {
        UpdateSingleWheel(frontLeftWheelCol, frontLeftWheelMesh);
        UpdateSingleWheel(frontRightWheelCol, frontRightWheelMesh);
        UpdateSingleWheel(rearLeftWheelCol, rearLeftWheelMesh);
        UpdateSingleWheel(rearRightWheelCol, rearRightWheelMesh);
    }

    private void UpdateSingleWheel(WheelCollider wheelCol, Transform wheelMesh)
    {
        Vector3 wheelPos;
        Quaternion wheelRot;
        wheelCol.GetWorldPose(out wheelPos, out wheelRot);
        wheelMesh.SetPositionAndRotation(wheelPos, wheelRot);
    }
}