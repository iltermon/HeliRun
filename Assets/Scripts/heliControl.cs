using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heliControl : MonoBehaviour
{
    public Sprite[] helicopterSprite;
    SpriteRenderer spriteRenderer;
    float _time = 0;
    float timeLimit = 0.1F; //gaz verildiğin timeLimit 0,04 olmalı
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        
        _time += Time.deltaTime;
        if(_time > timeLimit && spriteRenderer.sprite == helicopterSprite[0])
        {
            spriteRenderer.sprite = helicopterSprite[1];
            _time = 0;
            Debug.Log("0");
        }
        else if (_time > timeLimit && spriteRenderer.sprite == helicopterSprite[1])
        {
            spriteRenderer.sprite = helicopterSprite[0];
            _time = 0;
            Debug.Log("1");
        }
        

    }
}
