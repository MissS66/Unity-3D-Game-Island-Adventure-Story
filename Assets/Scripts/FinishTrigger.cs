using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public CameraFollow cameraFollow;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (cameraFollow != null)
            {
                cameraFollow.SetEndingView();
            }
        }
    }
}