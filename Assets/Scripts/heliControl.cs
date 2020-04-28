using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class heliControl : MonoBehaviour
{   
    Rigidbody2D helicopterRigid;
    public Sprite[] helicopterSprite;
    SpriteRenderer spriteRenderer;
    float _time = 0;
    float timeLimit = 0.1F;
    float vertical = 0f;
    public int speed = 10;
    int score = 0;
    public Text point_text;
    
    public GameControl gameControl;
    public static AudioSource []sounds;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        helicopterRigid = GetComponent<Rigidbody2D>();
        gameControl = GameObject.FindGameObjectWithTag("gameControlScript").GetComponent<GameControl>();
        sounds = GetComponents<AudioSource>();
    }

    void Update()
    {
        if (GameControl.gameOver==false && GameControl.gameStarted==true)
        {
            Animation();
        }
    }

    void Animation()
    {
        vertical=GameControl.getVertical();
        Debug.Log(vertical);
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
        if (!GameControl.gameOver) 
        { 
        Vector2 vec = new Vector2(0, vertical);
        helicopterRigid.AddForce(vec * speed);
        }
        else if(transform.position.y <= -7.77f){ 
            helicopterRigid.velocity = Vector2.zero;
            helicopterRigid.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "point")
        {
            score++;
            point_text.text = score.ToString();
            sounds[2].Play();
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "block")
        {   
            if(!GameControl.gameOver)
            {
                sounds[0].Pause();
                sounds[1].Play();
            }
            GetComponent<PolygonCollider2D>().enabled = false;
            GameControl.gameOver = true;
            gameControl.GameOver();
            if (score > GameControl.highscore)
            {
                GameControl.highscore = score;
                PlayerPrefs.SetInt("highScore", GameControl.highscore);
            }
        }

        
    }
}
