using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonHandler : MonoBehaviour
{
    
    public Sprite[] flags;//0 eng 1 tur
    private Image image;

    private void Start()
    {
        image = this.GetComponent<Image>();
        image.sprite = flags[0];
    }
    public void ChangeLanguage()
    {
        SceneManager.LoadScene("scene1");
        if (image.sprite == flags[0])
        {
            image.sprite = flags[1];
            GameControl.lang = true;
        }
        else if (image.sprite == flags[1])
        {
            image.sprite = flags[0];
            GameControl.lang = false;
        }
        
    }
}
    