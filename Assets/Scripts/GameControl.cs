using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject background1;
    public GameObject background2;
    public GameObject block;
    public int blockNumber;
    public float backgroundSpeed = -5f;
    private GameObject[] blocks;

    Rigidbody2D bgrigid1;
    Rigidbody2D bgrigid2;
    Rigidbody2D blockRigid;

    float t = 0;
    int counter = 0;
    private float size = 0;
    void Start()
    {
        bgrigid1 = background1.GetComponent<Rigidbody2D>();
        bgrigid2 = background2.GetComponent<Rigidbody2D>();
        
        bgrigid1.velocity = new Vector2(-15f, 0);
        bgrigid2.velocity = new Vector2(-15f, 0);
        
        size = background1.GetComponent<BoxCollider2D>().size.x;
        blocks = new GameObject[blockNumber];
        Debug.Log(background1.transform.position.x);
        Debug.Log(background2.transform.position.x);
        //for (int i= 0; i < blocks.Length; i++)
        //{
        //    blocks[i] = Instantiate(block, new Vector2(-20, -20), Quaternion.identity);
        //    blockRigid = blocks[i].AddComponent<Rigidbody2D>();
        //    blockRigid.gravityScale = 0;
        //    blockRigid.velocity = new Vector2(-backgroundSpeed, 0);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if(background1.transform.position.x <= -size)
        {
            background1.transform.position += new Vector3(size * 2, 0);
        }
        if (background2.transform.position.x <= -size)
        {
            background2.transform.position += new Vector3(size * 2, 0);
        }
        t = Time.deltaTime;
        if (t > 2f)
        {
            t = 0;
            float yAxis = Random.Range(-0.48f, 1.71f);
            blocks[counter].transform.position = new Vector3(6, yAxis);
            counter++;
            if (counter >= blocks.Length)
            {
                counter = 0;
            }
        }
    }
}
