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
    int point = 0;
    public Text point_text;
    bool gameOver = false;
    public GameControl gameControl;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        helicopterRigid = GetComponent<Rigidbody2D>();
        gameControl = GameObject.FindGameObjectWithTag("gameControlScript").GetComponent<GameControl>();
        
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
            point++;
            point_text.text = point.ToString();
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "block")
        {
            gameOver = true;
            gameControl.gameOver();
        }
    }
}
