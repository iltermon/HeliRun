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
    float vertical = 0;
    public int speed = 10;
    int score = 0;
    public Text point_text;
    public static bool gameOver = false;
    public GameControl gameControl;
    public AudioSource audio;
    public AudioClip scoreSound;
    public AudioClip gameOverSound;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        helicopterRigid = GetComponent<Rigidbody2D>();
        gameControl = GameObject.FindGameObjectWithTag("gameControlScript").GetComponent<GameControl>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            Animasyon();
        }
    }

    void Animasyon()
    {
        vertical = Input.GetAxisRaw("Vertical");
        Debug.Log(vertical);
        if (vertical > 0)
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
        if (!gameOver) 
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
            audio.clip = scoreSound;
            audio.Play();
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "block")
        {
            if(!gameOver)
            {
                audio.clip = gameOverSound;
                audio.Play();
            }
            gameOver = true;
            gameControl.gameOver();
            
        }
    }
}
