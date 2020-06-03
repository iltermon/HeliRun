using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class heliControl : MonoBehaviour
{   
    Rigidbody2D helicopterRigid;
    public Sprite[] helicopterSprite;
    SpriteRenderer spriteRenderer;
    float _time = 0;
    float timeLimit = 0.1F;
    public float vertical = 0f;
    public int speed = 10;
    public Text point_text;
    public GameControl gameControl;
    public AudioSource []sounds;
    public bool newHighScore=false;

    private static heliControl instance;

    public static heliControl Instance { get { return instance; } }

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
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        helicopterRigid = GetComponent<Rigidbody2D>();
        gameControl = GameObject.FindGameObjectWithTag("gameControlScript").GetComponent<GameControl>();
        sounds = GetComponents<AudioSource>();
        vertical = 0;
    }

    void Update()
    {
        if (gameControl.gameOver==false && gameControl.gameStarted==true)
        {
            Animation();
        }
    }

    void Animation()
    {
        if (vertical>0)
        {
            timeLimit = 0.07F;
        }
        else
        {
            timeLimit = 0.1F;
        }

        _time += Time.deltaTime;

        if(_time > timeLimit && spriteRenderer.sprite == helicopterSprite[0])
        {
            spriteRenderer.sprite = helicopterSprite[1];
            _time = 0;
        }
        else if (_time > timeLimit && spriteRenderer.sprite == helicopterSprite[1])
        {
            spriteRenderer.sprite = helicopterSprite[0];
            _time = 0;
     
        }
    }

    void FixedUpdate()
    {
        if (!gameControl.gameOver) 
        { 
        Vector2 vec = new Vector2(0, vertical);
        helicopterRigid.AddForce(vec * speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("point"))
        {
            gameControl.score++;
            point_text.text = gameControl.score.ToString();
            sounds[2].Play();
            //TODO:[1] bunu gamecontrole taşı metod olarak buradan çağır.
            GameControl.Instance.IncreaseVelocity();
        }
        if ((collision.gameObject.CompareTag("block") || collision.gameObject.CompareTag("top") || collision.gameObject.CompareTag("ground")) && SceneManager.GetActiveScene().name == "game_scene")
        {
            if (collision.gameObject.CompareTag("ground"))
            {
                helicopterRigid.velocity = Vector2.zero;
                helicopterRigid.gravityScale = 0;
            }
            if (!gameControl.gameOver)
            {
                sounds[0].Pause();
                sounds[1].Play();
            }
            gameControl.gameOver = true;
            if (gameControl.score > gameControl.highscore)
            {
                newHighScore=true;
                gameControl.highscore = gameControl.score;
                PlayerPrefs.SetInt("highScore", gameControl.highscore);
            }
            gameControl.GameOver();
        }
    }
}
