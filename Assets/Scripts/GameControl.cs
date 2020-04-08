using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject background1;
    public GameObject background2;
    public GameObject block;
    public int blockNumber=5;
    public float backgroundSpeed = -5f;
    
    private GameObject[] blocks;

    Rigidbody2D bgrigid1;
    Rigidbody2D bgrigid2;
    Rigidbody2D blockRigid;
    

    float reset_time = 0;
    int counter = 0;
    private float size = 0;
    private int point;
    void Start()
    {
        bgrigid1 = background1.GetComponent<Rigidbody2D>();
        bgrigid2 = background2.GetComponent<Rigidbody2D>();
        
        bgrigid1.velocity = new Vector2(-5f, 0);
        bgrigid2.velocity = new Vector2(-5f, 0);
        
        size = background1.GetComponent<BoxCollider2D>().size.x;
        blocks = new GameObject[blockNumber];
        for (int i= 0; i < blocks.Length; i++)
        {
            blocks[i] = Instantiate(block, new Vector2(-20, -20), Quaternion.identity);
            blockRigid = blocks[i].AddComponent<Rigidbody2D>();
            blockRigid.gravityScale = 0;
            blockRigid.velocity = new Vector2(-5f, 0);
        }
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
        reset_time += Time.deltaTime;
        if (reset_time > 2f)
        {
            reset_time = 0;
            float yAxis = Random.Range(-1.4f, 3f);
            blocks[counter].transform.position = new Vector3(25f, yAxis);
            counter++;
            if (counter >= blocks.Length)
            {
                counter = 0;
            }
        }
    }

    public void gameOver()
    {
        for (int i= 0; i<blocks.Length; i++)
        {
            blocks[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            bgrigid1.velocity = Vector2.zero;
            bgrigid2.velocity = Vector2.zero;
            
        }
    }   
    
}
