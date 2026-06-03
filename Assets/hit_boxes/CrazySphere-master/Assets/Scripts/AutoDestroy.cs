using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Update()
    {
        // 如果对象离开摄像机视锥或掉落太低也销毁
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}