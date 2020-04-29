using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class buttonHandler : MonoBehaviour
{
    
    public Sprite[] flags;//0 eng 1 tur
    public void changeLanguage()
    {
        this.GetComponent<Image>().sprite= flags[0];
    }
}
    