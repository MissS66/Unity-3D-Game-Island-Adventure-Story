using UnityEngine;

public class CarHitEffect : MonoBehaviour
{
    public AudioSource crashAudio;
    public CanvasGroup hitImage; // 拖入HitEffect的CanvasGroup（给Image加CanvasGroup组件）
    public float flashDuration = 0.2f;

    private float flashTimer;

    void Update()
    {
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            // 闪红后逐渐透明
            hitImage.alpha = Mathf.Lerp(1, 0, flashTimer / flashDuration);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 播放碰撞音效
        if (crashAudio != null)
        {
            crashAudio.Play();
        }
        // 触发屏幕闪红
        if (hitImage != null)
        {
            hitImage.alpha = 1;
            flashTimer = flashDuration;
        }
    }
}