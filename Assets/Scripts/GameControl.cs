using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject background1;
    public GameObject background2;
    Rigidbody2D rigid1;
    Rigidbody2D rigid2;
    private float size = 0;
    void Start()
    {
        rigid1 = background1.GetComponent<Rigidbody2D>();
        rigid2 = background2.GetComponent<Rigidbody2D>();

        rigid1.velocity = new Vector2(-1.5f, 0);
        rigid2.velocity = new Vector2(-1.5f, 0);

        size = background1.GetComponent<BoxCollider2D>().size.x;
        Debug.Log(size);
    }

    // Update is called once per frame
    void Update()
    {
        if(background1.transform.position.x<= -size)
        {
            background1.transform.position += new Vector3(size * 2, 0);
        }
        if (background2.transform.position.x <= -size)
        {
            background2.transform.position += new Vector3(size * 2, 0);
        }
    }
}
