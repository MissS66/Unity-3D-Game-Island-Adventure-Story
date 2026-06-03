using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [Header("漫游相机")]
    public FreeRoamCamera roamCam;

    [Header("所有原地小游戏脚本")]
    public List<MonoBehaviour> allGameScripts;

    [Header("所有原地小游戏相机")]
    public List<Camera> gameCameras;

    [Header("弹窗UI")]
    public GameObject promptUI;
    public Button btnStartGame;
    public Button btnContinueRoam;

    [Header("返回漫游按钮")]
    public GameObject returnToRoamButton;

    [Header("小游戏UI")]
    public GameObject GameUI_RollaBall;

    [Header("兜底漫游场景名")]
    public string defaultRoamSceneName = "GameScene";

    private MonoBehaviour pendingGameScript;
    private Camera pendingGameCam;
    private GameTriggerInfo.GameType pendingGameType = GameTriggerInfo.GameType.None;

    private static string lastRoamSceneName = "GameScene";
    private static bool hasSavedRoamTransform = false;
    private static Vector3 savedRoamPosition;
    private static Quaternion savedRoamRotation;

    void Awake()
    {
        Instance = this;

        if (SceneManager.GetActiveScene().name != "Game")
        {
            lastRoamSceneName = SceneManager.GetActiveScene().name;
        }
    }

    void Start()
    {
        BindButtons();
        RestoreRoamTransformIfNeeded();
        SetRoamMode();
        ClearPromptState();
        PlayGlobalBGM();
    }

    private void BindButtons()
    {
        if (btnStartGame != null)
        {
            btnStartGame.onClick.RemoveListener(OnStartGame);
            btnStartGame.onClick.AddListener(OnStartGame);
        }

        if (btnContinueRoam != null)
        {
            btnContinueRoam.onClick.RemoveListener(OnContinueRoam);
            btnContinueRoam.onClick.AddListener(OnContinueRoam);
        }
    }

    private void SaveRoamTransform()
    {
        if (roamCam != null)
        {
            savedRoamPosition = roamCam.transform.position;
            savedRoamRotation = roamCam.transform.rotation;
            hasSavedRoamTransform = true;
        }
    }

    private void RestoreRoamTransformIfNeeded()
    {
        if (hasSavedRoamTransform && roamCam != null)
        {
            roamCam.transform.position = savedRoamPosition;
            roamCam.transform.rotation = savedRoamRotation;
            hasSavedRoamTransform = false;
        }
    }

    private void EnableRoamCamera()
    {
        if (roamCam == null) return;

        roamCam.gameObject.SetActive(true);
        roamCam.SetRoamActive(true);

        Camera roamCamera = roamCam.GetComponent<Camera>();
        if (roamCamera != null) roamCamera.enabled = true;

        AudioListener listener = roamCam.GetComponent<AudioListener>();
        if (listener != null) listener.enabled = true;
    }

    private void DisableRoamCamera()
    {
        if (roamCam == null) return;

        roamCam.SetRoamActive(false);

        Camera roamCamera = roamCam.GetComponent<Camera>();
        if (roamCamera != null) roamCamera.enabled = false;

        AudioListener listener = roamCam.GetComponent<AudioListener>();
        if (listener != null) listener.enabled = false;
    }

    private void StopGlobalBGM()
    {
        if (GlobalBGMManager.Instance != null)
        {
            GlobalBGMManager.Instance.StopBGM();
        }
    }

    private void PlayGlobalBGM()
    {
        if (GlobalBGMManager.Instance != null)
        {
            GlobalBGMManager.Instance.PlayBGM();
        }
    }

    private void ClearPromptState()
    {
        pendingGameScript = null;
        pendingGameCam = null;
        pendingGameType = GameTriggerInfo.GameType.None;

        if (promptUI != null) promptUI.SetActive(false);
        if (returnToRoamButton != null) returnToRoamButton.SetActive(false);
    }

    public void SetRoamMode()
    {
        EnableRoamCamera();

        if (allGameScripts != null)
        {
            foreach (var s in allGameScripts)
            {
                if (s != null)
                {
                    s.enabled = false;

                    Rigidbody rb = s.GetComponent<Rigidbody>();
                    if (rb != null) rb.isKinematic = true;
                }
            }
        }

        if (gameCameras != null)
        {
            foreach (var c in gameCameras)
            {
                if (c != null)
                {
                    c.gameObject.SetActive(false);

                    AudioListener listener = c.GetComponent<AudioListener>();
                    if (listener != null) listener.enabled = false;
                }
            }
        }

        if (GameUI_RollaBall != null) GameUI_RollaBall.SetActive(false);
        if (returnToRoamButton != null) returnToRoamButton.SetActive(false);
        if (promptUI != null) promptUI.SetActive(false);
    }

    private void ResetCurrentGameObjectOnly()
    {
        if (pendingGameScript == null) return;

        Rigidbody rb = pendingGameScript.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        pendingGameScript.enabled = false;
    }

    public void ShowGamePrompt(MonoBehaviour gameScript, Camera gameCam, GameTriggerInfo.GameType gameType)
    {
        pendingGameScript = gameScript;
        pendingGameCam = gameCam;
        pendingGameType = gameType;

        if (promptUI != null)
        {
            promptUI.SetActive(true);
        }
        else
        {
            Debug.LogError("PromptUI 没有绑定。");
        }
    }

    public void EnterGame(MonoBehaviour gameScript, Camera gameCam, GameTriggerInfo.GameType gameType)
    {
        // 进入这些游戏时关闭背景音乐111：
        // Tank / CrazySphere(HitBoxes) / RollaBall
        if (gameType == GameTriggerInfo.GameType.Tank ||
            gameType == GameTriggerInfo.GameType.HitBoxes ||
            gameType == GameTriggerInfo.GameType.RollaBall)
        {
            StopGlobalBGM();
        }

        // 进入这些游戏时继续播放背景音乐111：
        // TableBall / CarRace
        if (gameType == GameTriggerInfo.GameType.TableBall ||
            gameType == GameTriggerInfo.GameType.CarRace)
        {
            PlayGlobalBGM();
        }

        // 坦克：跳转独立场景
        if (gameType == GameTriggerInfo.GameType.Tank)
        {
            SaveRoamTransform();

            if (promptUI != null) promptUI.SetActive(false);
            if (returnToRoamButton != null) returnToRoamButton.SetActive(false);

            DisableRoamCamera();

            lastRoamSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("Game");
            return;
        }

        // HitBoxes / CrazySphere：跳转独立场景
        if (gameType == GameTriggerInfo.GameType.HitBoxes)
        {
            SaveRoamTransform();

            if (promptUI != null) promptUI.SetActive(false);
            if (returnToRoamButton != null) returnToRoamButton.SetActive(false);

            DisableRoamCamera();

            lastRoamSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("CrazySphereScene");
            return;
        }

        // 主场景内小游戏：RollaBall / TableBall / CarRace
        DisableRoamCamera();

        if (allGameScripts != null)
        {
            foreach (var s in allGameScripts)
            {
                if (s != null)
                {
                    s.enabled = false;

                    Rigidbody rb = s.GetComponent<Rigidbody>();
                    if (rb != null) rb.isKinematic = true;
                }
            }
        }

        if (gameCameras != null)
        {
            foreach (var c in gameCameras)
            {
                if (c != null)
                {
                    c.gameObject.SetActive(false);

                    AudioListener listener = c.GetComponent<AudioListener>();
                    if (listener != null) listener.enabled = false;
                }
            }
        }

        if (gameScript != null)
        {
            gameScript.enabled = true;

            Rigidbody rb = gameScript.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = false;
        }

        if (gameCam != null)
        {
            gameCam.gameObject.SetActive(true);
            gameCam.enabled = true;

            AudioListener listener = gameCam.GetComponent<AudioListener>();
            if (listener != null) listener.enabled = true;
        }
        else
        {
            Debug.LogError("当前小游戏没有绑定 Game Cam，请检查触发器。");
        }

        if (GameUI_RollaBall != null) GameUI_RollaBall.SetActive(true);
        if (returnToRoamButton != null) returnToRoamButton.SetActive(true);
    }

    public static void ReturnToRoam()
    {
        // 独立场景返回主漫游场景
        if (SceneManager.GetActiveScene().name == "Game" ||
            SceneManager.GetActiveScene().name == "CrazySphereScene")
        {
            if (GlobalBGMManager.Instance != null)
            {
                GlobalBGMManager.Instance.PlayBGM();
            }

            SceneManager.LoadScene(lastRoamSceneName);
            return;
        }

        // 主场景内小游戏返回漫游
        if (Instance != null)
        {
            Instance.ResetCurrentGameObjectOnly();
            Instance.SetRoamMode();
            Instance.ClearPromptState();
            Instance.PlayGlobalBGM();
        }
    }

    public void OnStartGame()
    {
        if (promptUI != null) promptUI.SetActive(false);

        if (pendingGameType == GameTriggerInfo.GameType.None)
        {
            Debug.LogError("没有记录到要启动的游戏类型，请重新走进触发器。");
            return;
        }

        EnterGame(pendingGameScript, pendingGameCam, pendingGameType);
    }

    public void OnContinueRoam()
    {
        ClearPromptState();
        PlayGlobalBGM();
    }
}