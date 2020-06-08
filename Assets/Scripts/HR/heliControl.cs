using UnityEngine;
using UnityEngine.SceneManagement;


public class heliControl : MonoBehaviour
{   
    Rigidbody2D helicopterRigid;
    public Sprite[] helicopterSprite;
    SpriteRenderer spriteRenderer;
    float _time = 0;
    float timeLimit = 0.1F;
    public float vertical = 0f;
    public int speed = 10;
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
        sounds = GetComponents<AudioSource>();
        vertical = 0;
    }

    void Update()
    {
        if (GameControl.Instance.gameOver==false && GameControl.Instance.gameStarted==true)
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
        if (!GameControl.Instance.gameOver) 
        { 
        Vector2 vec = new Vector2(0, vertical);
        helicopterRigid.AddForce(vec * speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("point"))
        {
            GameControl.Instance.Score();
        }
        if ((collision.gameObject.CompareTag("block") || collision.gameObject.CompareTag("top") || collision.gameObject.CompareTag("ground")) && SceneManager.GetActiveScene().name == "game_scene")
        {
            if (collision.gameObject.CompareTag("ground"))
            {
                helicopterRigid.velocity = Vector2.zero;
                helicopterRigid.gravityScale = 0;
            }
            if (!GameControl.Instance.gameOver)
            {
                sounds[0].Pause();
                sounds[1].Play();
            }
            GameControl.Instance.gameOver = true;
            if (GameControl.Instance.score > GameControl.Instance.highscore)
            {
                newHighScore=true;
                GameControl.Instance.highscore = GameControl.Instance.score;
                PlayerPrefs.SetInt("highScore", GameControl.Instance.highscore);
            }
            GameControl.Instance.GameOver();
        }
    }
    public void Accelerate()
    {
        heliControl.Instance.vertical=1;
        if(helicopterRigid.velocity[1]<0)
          {  
            helicopterRigid.velocity=new Vector2(helicopterRigid.velocity[0],helicopterRigid.velocity[1]/2);
          }  
    }
}