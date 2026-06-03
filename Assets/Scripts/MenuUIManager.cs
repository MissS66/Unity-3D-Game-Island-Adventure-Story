using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    public AudioSource bgm;
    public Toggle musicToggle;
    public Slider volumeSlider;

    private void Start()
    {
        if (bgm != null)
        {
            bgm.mute = false;
            bgm.volume = 0.5f;
        }

        if (musicToggle != null)
        {
            musicToggle.isOn = true;
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = 0.5f;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("ÍË³öÓÎÏ·");
    }

    public void ToggleMusic(bool isOn)
    {
        if (bgm != null)
        {
            bgm.mute = !isOn;
        }
    }

    public void ChangeVolume(float value)
    {
        if (bgm != null)
        {
            bgm.volume = value;
        }
    }
} 