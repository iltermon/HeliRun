using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class buttonHandler : MonoBehaviour
{
    Image image;
    public Image[] flags;//0 eng 1 tur
    public void changeLanguage()
    {
        image = GetComponent<Image>();
        if (image.sprite == flags[0])
        {
            image = flags[1];
        }
        else if (image.sprite == flags[1])
        {
            image = flags[0];
        }
    }
}
    