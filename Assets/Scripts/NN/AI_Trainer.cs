using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;
using UnityEngine.UI;
public class AI_Trainer : MonoBehaviour
{
	public Population population;
	public NeuralNetwork.NeuralNetwork currNN;
	Transform raycastPoint;
	RaycastHit2D[] sensors;
    public Material lineRendererMaterial;
	Vector2 startPosition;
	Transform topSensorPos;
	Transform bottomSensorPos;
	Vector2 startPosCave1;
	Vector2 startPosCave2;
	Vector2 currCarPos;
	Vector2 lastCarPos;
	public int maxDist;
	public float timePassed;
	public float timeScale = 1f;
    readonly float rayDist = 8;
	public int dist=0;
	private static AI_Trainer instance;
	public float topfwd, botfwd, right, up, down, vel;
    private float maxVel;
    private float minVel,lowestVel;

    public static AI_Trainer Instance { get { return instance; } }
	
	
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
	// Use this for initialization
	void Start ()
	{
		dist=0;
		population = new Population(10, new int[]{6, 64, 1}, 1f,30);
		raycastPoint = transform.Find("RaycastPoint2");	
		topSensorPos = transform.Find("RaycastPoint1");	
		bottomSensorPos = transform.Find("RaycastPoint3");	
		startPosition = transform.position;
		currCarPos = lastCarPos = startPosition;
		currNN = population.Next();
		startPosCave1 = GameControl.Instance.background1.GetComponent<Rigidbody2D>().position;
		startPosCave2 = GameControl.Instance.background2.GetComponent<Rigidbody2D>().position;
	}

    //public void NewGenome()
    //{
    //    OnTriggerEnter2D();
    //}

    public void ChangeSpeed()
    {
        if (timeScale == 1f)
            timeScale = 2f;
        else if (timeScale == 2f)
            timeScale = 5f;
        else if (timeScale == 5f)
            timeScale = 10f;
        else if (timeScale == 10f)
            timeScale = 1f;
    }
	// Update is called once per frame
	void Update()
	{
		
		dist++;
		AI_UI.Instance.distanceText.text = dist.ToString();
		Time.timeScale = timeScale;
		sensors = new RaycastHit2D[5];
		
		sensors[0] = Physics2D.Raycast(raycastPoint.position, raycastPoint.up, rayDist);
		sensors[1] = Physics2D.Raycast(raycastPoint.position, raycastPoint.right, rayDist);
		sensors[2] = Physics2D.Raycast(raycastPoint.position, -raycastPoint.up, rayDist);
		sensors[3] = Physics2D.Raycast(topSensorPos.position, topSensorPos.right, rayDist);
		sensors[4] = Physics2D.Raycast(bottomSensorPos.position, bottomSensorPos.right, rayDist);
		right = topfwd = botfwd = down = up = rayDist;
			if (sensors[0].collider != null )
			{
				if(!sensors[0].collider.CompareTag("point"))
				{
					up = sensors[0].distance;
				}
			}
			if (sensors[1].collider!=null)
			{
				if(!sensors[1].collider.CompareTag("point"))
				{
				right = sensors[1].distance;
				}
				
			}
			if (sensors[2].collider !=null)
			{
				if(!sensors[2].collider.CompareTag("point"))
				{
				down = sensors[2].distance;
				}
			}
			if (sensors[3].collider !=null)
			{
				if(!sensors[3].collider.CompareTag("point"))
				{
				topfwd = sensors[3].distance;
				}
			}
			if (sensors[4].collider !=null)
			{
				if(!sensors[4].collider.CompareTag("point"))
				{
				botfwd = sensors[4].distance;
				}
			}
		maxVel=14;
		minVel=-10f;
		
		vel = GetComponent<Rigidbody2D>().velocity[1];
		if(lowestVel>vel)
		{
			lowestVel=vel;	
		}
		//TODO: Bi fonksiyon oluşturabilirsin 
		currNN.input.matrix[0, 0] = (2f / rayDist) * up - 1f;
		currNN.input.matrix[1, 0] = (2f / rayDist) * right - 1f;
		currNN.input.matrix[2, 0] = (2f / rayDist) * down - 1f;
		currNN.input.matrix[3, 0] = 2f*((vel-minVel)/(maxVel-minVel))-1;
		currNN.input.matrix[4, 0] = (2f / rayDist) * topfwd - 1f;
		currNN.input.matrix[5, 0] = (2f / rayDist) * botfwd - 1f;
		currNN.FeedForward(); 
		if(currNN.output.matrix[0, 0]>0)
		{
		heliControl.Instance.Accelerate();
		}
		else
		{
			heliControl.Instance.vertical=0;
		}

		currCarPos = transform.position;
		lastCarPos = currCarPos;
		timePassed += Time.deltaTime;

	}
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("point"))
		{
			population.SetFitnessOfCurrIndividual(dist);
			currNN = population.Next();
			ResetCarPosition(); 
		}
	}

	public void ResetCarPosition()
	{
		transform.position = startPosition;
		currCarPos = startPosition;
		lastCarPos = startPosition;
		GameControl.Instance.bgrigid1.position = startPosCave1;
		GameControl.Instance.bgrigid2.position = startPosCave2;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GameControl.Instance.CreateBlocks();
		if(maxDist<dist)
			maxDist=dist;
		dist=0;
		
	}
 }
