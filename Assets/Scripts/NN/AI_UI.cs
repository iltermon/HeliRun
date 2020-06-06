using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AI_UI : MonoBehaviour
{
    public Text distanceText;
    public Text upText;
    public Text forwardText;
    public Text downText;
    public Button changeSpeedButton;

	private static AI_UI instance;

	public static AI_UI Instance { get { return instance; } }

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}
	}

    // Update is called once per frame
    void Update ()
	{
		distanceText.text=AI_Trainer.Instance.dist.ToString();
		upText.text=AI_Trainer.Instance.up.ToString();
		downText.text=AI_Trainer.Instance.down.ToString();
		forwardText.text=AI_Trainer.Instance.forward.ToString();
	}
}
