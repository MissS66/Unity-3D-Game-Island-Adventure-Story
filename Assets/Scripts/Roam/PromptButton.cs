using UnityEngine;

public class PromptButton : MonoBehaviour
{
    public MonoBehaviour targetScript;
    public Camera targetCam;
    public GameTriggerInfo.GameType targetGameType;

    public void SetTarget(MonoBehaviour script, Camera cam, GameTriggerInfo.GameType type)
    {
        targetScript = script;
        targetCam = cam;
        targetGameType = type;
    }
}