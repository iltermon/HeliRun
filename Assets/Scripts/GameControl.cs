using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{

    public GameObject background1;
    public GameObject background2;
    public GameObject block;
    public int blockNumber=5;
    public float backgroundSpeed = -5f;
    public Image title;
    public GameObject helicopter;
    public Text highscoreText;
    private GameObject[] blocks;
    Rigidbody2D bgrigid1;
    Rigidbody2D bgrigid2;
    Rigidbody2D blockRigid;
    float reset_time = 0;
    int counter = 0;
    private float size = 0;
    public int score=0;
    public static int highscore;
    public static bool gameStarted = false;

    void Start()
    {
        bgrigid1 = background1.GetComponent<Rigidbody2D>();
        bgrigid2 = background2.GetComponent<Rigidbody2D>();
        blocks = new GameObject[blockNumber];
        highscore = PlayerPrefs.GetInt("save");
        highscoreText.text = "High Score: " + highscore.ToString();
    }
    void waitforInput()
    {
        if(gameStarted==false && getVertical() > 0)
        {
            gameStarted = true;
            startGame();
            title.gameObject.SetActive(false);
            highscoreText.gameObject.SetActive(false);
        }
    }
    void startGame()
    {
        bgrigid1.velocity = new Vector2(-5f, 0);
        bgrigid2.velocity = new Vector2(-5f, 0);
        helicopter.GetComponent<Rigidbody2D>().simulated = true;
        size = background1.GetComponent<BoxCollider2D>().size.x;
        heliControl.sounds[0].Play();
        
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i] = Instantiate(block, new Vector2(-20, -20), Quaternion.Euler(0,0,270));
            blockRigid = blocks[i].AddComponent<Rigidbody2D>();
            blockRigid.gravityScale = 0;
            blockRigid.velocity = new Vector2(-5f, 0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        waitforInput();
        if(background1.transform.position.x <= -size)
        {
            background1.transform.position += new Vector3(size * 2, 0);
        }
        if (background2.transform.position.x <= -size)
        {
            background2.transform.position += new Vector3(size * 2, 0);
        }
        reset_time += Time.deltaTime;
 
        if (reset_time > 2f && heliControl.gameOver==false && gameStarted==true)
        {
            reset_time = 0;
            // TODO: engel oluşturulurken zamanı kullan.
            float yAxis = Random.Range(-8.44f, -1.91f);
            blocks[counter].transform.position = new Vector3(25f, yAxis);
            counter++;
            if (counter >= blocks.Length)
            {
                counter = 0;
            }
        }
    }

    public void gameOver()
    {
        helicopter.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            bgrigid1.velocity = Vector2.zero;
            bgrigid2.velocity = Vector2.zero;
        }
    }   
    public static float getVertical()
    {
        if (Input.GetMouseButton(0))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
