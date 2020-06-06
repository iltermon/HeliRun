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
	Vector3 startPosition;
	Vector3 startPosCave1;
	Vector3 startPosCave2;
	Vector3 currCarPos;
	Vector3 lastCarPos;
	public int maxDist;
	public float timePassed;
	public float timeScale = 1f;
    readonly float rayDist = 8;
	public int dist=0;
	private static AI_Trainer instance;
	public float right, up, down, vel;
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
		population = new Population(10, new int[]{3, 25, 1}, 1f);
		raycastPoint = transform.Find("RaycastPoint");	
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
		sensors = new RaycastHit2D[3];
		sensors[0] = Physics2D.Raycast(raycastPoint.position, raycastPoint.up, rayDist);
		sensors[1] = Physics2D.Raycast(raycastPoint.position, raycastPoint.right, rayDist);
		sensors[2] = Physics2D.Raycast(raycastPoint.position, -raycastPoint.up, rayDist);
		right = down = up = rayDist;
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
		vel = GetComponent<Rigidbody2D>().velocity[1];
		currNN.input.matrix[0, 0] = (2f / rayDist) * up - 1f;
		currNN.input.matrix[1, 0] = (2f / rayDist) * right - 1f;
		currNN.input.matrix[2, 0] = (2f / rayDist) * down - 1f;
		currNN.FeedForward();

		heliControl.Instance.vertical = currNN.output.matrix[0, 0];

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

	void ResetCarPosition()
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

// 	void DrawSensorLines()
// 	{
// 		Color middleSensorColor, leftSensorColor, rightSensorColor, leftMiddleSensorColor, rightMiddleSensorColor;
// 		middleSensorColor = (sensors[0].collider == null) ? Color.green : Color.red;
// 		leftSensorColor   = (sensors[1].collider == null) ? Color.green : Color.red;
// 		rightSensorColor  = (sensors[2].collider == null) ? Color.green : Color.red;


//        if (lines == null)
// 		{
// 			lines = new GameObject[5];
// 			DrawLine(
// 				raycastPoint.position,
// 				(raycastPoint.position + raycastPoint.forward * rayDist), 
// 				middleSensorColor,
// 				0
// 			);

// 			DrawLine(
// 				raycastPoint.position,
// 				(raycastPoint.position + (raycastPoint.forward - raycastPoint.right) * rayDist),
// 				leftMiddleSensorColor,
// 				1
// 			);

// 			DrawLine(
// 				raycastPoint.position,
// 				(raycastPoint.position + (raycastPoint.forward + raycastPoint.right) * rayDist),
// 				rightMiddleSensorColor,
// 				2
// 			);

//            DrawLine(
//                raycastPoint.position,
//                (raycastPoint.position + (-raycastPoint.right) * rayDist),
//                leftMiddleSensorColor,
//                3
//            );

//            DrawLine(
//                raycastPoint.position,
//                (raycastPoint.position + raycastPoint.right * rayDist),
//                rightMiddleSensorColor,
//                4
//            );
//        }
// 		else
// 		{
// 			UpdateLine(
// 				raycastPoint.position,
// 				(raycastPoint.position + raycastPoint.forward * rayDist), 
// 				middleSensorColor,
// 				0
// 			);

// 			UpdateLine(
// 				raycastPoint.position,
// 				(raycastPoint.position + (raycastPoint.forward - raycastPoint.right).normalized * rayDist),
// 				leftMiddleSensorColor,
// 				1
// 			);

// 			UpdateLine(
// 				raycastPoint.position,
// 				(raycastPoint.position + (raycastPoint.forward + raycastPoint.right).normalized * rayDist),
// 				rightMiddleSensorColor,
// 				2
// 			);

//            UpdateLine(
//                raycastPoint.position,
//                (raycastPoint.position + (-raycastPoint.right).normalized * rayDist),
//                leftSensorColor,
//                3
//            );

//            UpdateLine(
//                raycastPoint.position,
//                (raycastPoint.position + raycastPoint.right.normalized * rayDist),
//                rightSensorColor,
//                4
//            );
//         }

// 	}
		
// 	void DrawLine(Vector3 start, Vector3 end, Color color, int lineIndex)
// 	{
// 		GameObject line = new GameObject();
// 		line.name = "Line " + lineIndex;
// 		line.transform.SetParent(environments.transform);

// 		line.transform.position = start;
// 		line.AddComponent<LineRenderer>();
// 		LineRenderer lr = line.GetComponent<LineRenderer>();
//        lr.material = lineRendererMaterial; //new Material(Shader.Find("Particles/Priority Alpha Blended"));
// 		lr.startColor = color;
// 		lr.endColor = color;
// 		lr.startWidth = 0.05f;
// 		lr.endWidth = 0.05f;
// 		lr.SetPosition(0, start);
// 		lr.SetPosition(1, end);

// 		lines[lineIndex] = line;
// 	}

// 	void UpdateLine(Vector3 start, Vector3 end, Color color, int lineIndex)
// 	{
// 		LineRenderer lr = lines[lineIndex].GetComponent<LineRenderer>();
// 		lr.startColor = color;
// 		lr.endColor = color;
// 		lr.SetPosition(0, start);
// 		lr.SetPosition(1, end);
// 	}
 }
