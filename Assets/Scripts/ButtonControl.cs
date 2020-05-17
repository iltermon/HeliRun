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
        heliControl.vertical = 1;

    }
    public void ReleaseVertical()
    {
        heliControl.vertical = 0;
    }
    public void PauseGame()
    {
        if (GameControl.paused)
        {
            Time.timeScale = 1;
            GameControl.paused = false;
            resumeButton.gameObject.SetActive(false);
            heliControl.sounds[0].Play();

        }
        else
        {
            Time.timeScale = 0;
            GameControl.paused = true;
            resumeButton.gameObject.SetActive(true);
            heliControl.sounds[0].Pause();
        }
    }
    public void MuteGame()
    {
        if (GameControl.muted)
        {
            GameControl.muted = false;
            for (int i = 0; i < heliControl.sounds.Length; i++)
            {
                heliControl.sounds[i].mute = true;
            }
        }
        else if (!GameControl.muted)
        { 
            GameControl.muted = true;
            for (int i = 0; i < heliControl.sounds.Length; i++)
            {
                heliControl.sounds[i].mute = false;
            }
        }

    }
}
    