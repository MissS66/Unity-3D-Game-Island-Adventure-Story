using UnityEngine;

public class GameTrigger : MonoBehaviour
{
    [Header("对应小游戏的控制物体和相机（主场景内）")]
    public MonoBehaviour gameScript;   // TableBall / CarRace 对应脚本
    public Camera gameCam;             // 游戏专用摄像机

    private const string TARGET_NAME = "roamCam";

    void OnTriggerEnter(Collider other)
    {
        if (other.name == TARGET_NAME && other.CompareTag("Player"))
        {
            Debug.Log("√ 成功触发小游戏");

            GameTriggerInfo info = GetComponent<GameTriggerInfo>();

            if (info == null)
            {
                Debug.LogError("当前触发器物体上没有挂 GameTriggerInfo 脚本！");
                return;
            }

            if (GameStateManager.Instance == null)
            {
                Debug.LogError("场景中没有 GameStateManager！");
                return;
            }

            // 仅针对主场景内游戏，确保脚本和摄像机非空
            if ((info.gameType == GameTriggerInfo.GameType.TableBall || info.gameType == GameTriggerInfo.GameType.CarRace)
                && (gameScript == null || gameCam == null))
            {
                Debug.LogError("主场景内小游戏触发器缺少 GameScript 或 GameCam，请在 Inspector 填写");
                return;
            }

            GameStateManager.Instance.ShowGamePrompt(gameScript, gameCam, info.gameType);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == TARGET_NAME)
        {
            if (GameStateManager.Instance != null && GameStateManager.Instance.promptUI != null)
            {
                GameStateManager.Instance.promptUI.SetActive(false);
            }
        }
    }
}