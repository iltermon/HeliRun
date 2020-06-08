using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AI_UI : MonoBehaviour
{
    public Text distanceText;
    public Text upText;
	public Text topText;
	public Text botText;
    public Text rightText;
    public Text downText;
	public Text populationText;
	public Text indivText;
	public Text maxText;
    public Button changeSpeedButton;
	private static AI_UI instance;
	public int indiv;
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
		Debug.Log(AI_Trainer.Instance.currNN.input.matrix[3,0].ToString());
		distanceText.text=AI_Trainer.Instance.dist.ToString();
		upText.text=AI_Trainer.Instance.currNN.input.matrix[0, 0].ToString();
		downText.text=AI_Trainer.Instance.currNN.input.matrix[2, 0].ToString();
		rightText.text=AI_Trainer.Instance.currNN.input.matrix[1, 0].ToString();
		indivText.text=AI_Trainer.Instance.population.generationNumber.ToString();
		populationText.text=AI_Trainer.Instance.population.currIndiv.ToString();
		maxText.text = AI_Trainer.Instance.maxDist.ToString();
		topText.text= AI_Trainer.Instance.currNN.input.matrix[4, 0].ToString();
		botText.text = AI_Trainer.Instance.currNN.input.matrix[5, 0].ToString();
	}
}
