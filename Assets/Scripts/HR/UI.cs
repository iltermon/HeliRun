using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text scoreText;
    public Text yourScore;
    public Text newRecord;
     public Button pauseButton;
    public Image gameOverImage;
    public Text clickToStart;
    private static UI instance;
    public Text highscoreText;
    public Image title;
    public static UI Instance { get { return instance; } }

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
    // Start is called before the first frame update
    void Start()
    {
        highscoreText.text = "High Score: " + GameControl.Instance.highscore.ToString();
        gameOverImage.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        newRecord.gameObject.SetActive(false);
        yourScore.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivateUI()
    {
        title.gameObject.SetActive(false);
        highscoreText.gameObject.SetActive(false);
        clickToStart.gameObject.SetActive(false);
    }
}
