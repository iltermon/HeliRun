using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heliControl : MonoBehaviour
{
    Rigidbody2D helicopterRigid;
    public Sprite[] helicopterSprite;
    SpriteRenderer spriteRenderer;
    float _time = 0;
    float timeLimit = 0.1F;
    float vertical = 0;
    public int speed = 10;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        helicopterRigid = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
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
        Vector3 vec = new Vector3(0, vertical, 0);
        helicopterRigid.AddForce(vec * speed);
    }
}
