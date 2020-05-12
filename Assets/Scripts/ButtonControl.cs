using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonControl : MonoBehaviour
{
    public void getVertical()
    {
        heliControl.vertical = 1;
        
    }
    public void releaseVertical()
    {
        heliControl.vertical = 0;
    }
}
    