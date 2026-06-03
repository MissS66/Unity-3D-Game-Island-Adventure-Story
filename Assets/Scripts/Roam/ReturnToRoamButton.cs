using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToRoamButton : MonoBehaviour
{
    [Header("返回的漫游主场景名")]
    public string roamSceneName = "GameScene";

    public void OnClickReturnToRoam()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.ReturnToRoam();
        }
        else
        {
            SceneManager.LoadScene(roamSceneName);
        }
    }
}