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
    public Text point_text;
    public GameControl gameControl;
    public static AudioSource []sounds;
    public static bool newHighScore=false;
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
            GameControl.score++;
            point_text.text = GameControl.score.ToString();
            sounds[2].Play();
            GameControl.bgrigid1.velocity = new Vector2(-(GameControl.backgroundSpeed + (GameControl.score / 10)), 0);
            GameControl.bgrigid2.velocity = new Vector2(-(GameControl.backgroundSpeed + (GameControl.score / 10)), 0);
            GameControl.blocks[0].GetComponent<Rigidbody2D>().velocity = new Vector2(-(GameControl.backgroundSpeed + (GameControl.score / 10)), 0);
            GameControl.blocks[1].GetComponent<Rigidbody2D>().velocity = new Vector2(-(GameControl.backgroundSpeed + (GameControl.score / 10)), 0);
            GameControl.blocks[2].GetComponent<Rigidbody2D>().velocity = new Vector2(-(GameControl.backgroundSpeed + (GameControl.score / 10)), 0);
            GameControl.blocks[3].GetComponent<Rigidbody2D>().velocity = new Vector2(-(GameControl.backgroundSpeed + (GameControl.score / 10)), 0);
            GameControl.blocks[4].GetComponent<Rigidbody2D>().velocity = new Vector2(-(GameControl.backgroundSpeed + (GameControl.score / 10)), 0);
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
            if (GameControl.score > GameControl.highscore)
            {
                GameControl.highscore = GameControl.score;
                PlayerPrefs.SetInt("highScore", GameControl.highscore);
            }
        }
    }
}
