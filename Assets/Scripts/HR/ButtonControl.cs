using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public Button resumeButton;

    public void GetVertical()
    {
        heliControl.Instance.vertical = 1;

    }
    public void ReleaseVertical()
    {
        heliControl.Instance.vertical = 0;
    }
    public void PauseGame()
    {
        if (GameControl.Instance.paused)
        {
            Time.timeScale = 1;
            GameControl.Instance.paused = false;
            resumeButton.gameObject.SetActive(false);
            heliControl.Instance.sounds[0].Play();

        }
        else
        {
            Time.timeScale = 0;
            GameControl.Instance.paused = true;
            resumeButton.gameObject.SetActive(true);
            heliControl.Instance.sounds[0].Pause();
        }
    }
    public void MuteGame()
    {
        if (GameControl.Instance.muted)
        {
            GameControl.Instance.muted = false;
            for (int i = 0; i < heliControl.Instance.sounds.Length; i++)
            {
                heliControl.Instance.sounds[i].mute = true;
            }
        }
        else if (!GameControl.Instance.muted)
        { 
            GameControl.Instance.muted = true;
            for (int i = 0; i < heliControl.Instance.sounds.Length; i++)
            {
                heliControl.Instance.sounds[i].mute = false;
            }
        }

    }
}
    