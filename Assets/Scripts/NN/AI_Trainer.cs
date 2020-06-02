using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;

public class AI_Trainer : MonoBehaviour
{
	public Population population;
	NeuralNetwork.NeuralNetwork currNN;

	AI_DriveController driveController;
	Transform raycastPoint;
	RaycastHit2D[] sensors;
	GameObject environments;
	GameObject[] lines;
    public Material lineRendererMaterial;
	public GameObject cave1;
	public GameObject cave2;
	
	Vector3 startPosition;
	Quaternion startRotation;
	
	Vector3 currCarPos;
	Vector3 lastCarPos;
	public float totalDist;
	public float timePassed;

	public float timeScale = 1f;

	float rayDist = 10f;

	// Use this for initialization
	void Start ()
	{

		population = new Population(10, new int[]{4, 200, 1}, 1f);
		raycastPoint = transform.Find("RaycastPoint");	
		startPosition = transform.position;
		startRotation = transform.rotation;
		currCarPos = lastCarPos = startPosition;
		currNN = population.Next();
	}

    public void NewGenome()
    {
        OnCollisionEnter();
    }

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
	void Update ()
	{

		Time.timeScale = timeScale;

		sensors = new RaycastHit2D[3];

		sensors[0] = Physics2D.Raycast(raycastPoint.position, raycastPoint.up,rayDist);
		sensors[1] = Physics2D.Raycast(raycastPoint.position, raycastPoint.forward, rayDist);
		sensors[2] = Physics2D.Raycast(raycastPoint.position, -raycastPoint.up, rayDist);
		Debug.DrawLine(raycastPoint.position, sensors[2].point);
		Debug.DrawLine(raycastPoint.position, sensors[0].point);
		Debug.DrawLine(raycastPoint.position, sensors[1].point);

		float forward, up, down;
		forward = down = up = rayDist;
		if (sensors[0].collider != cave1.GetComponent<BoxCollider2D>() || sensors[0].collider != cave2.GetComponent<BoxCollider2D>() || !sensors[1].collider.CompareTag("point")) 
		{ 
			up = sensors[0].distance;

		}

		if (sensors[1].collider != cave1.GetComponent<BoxCollider2D>() || sensors[1].collider != cave2.GetComponent<BoxCollider2D>() || !sensors[1].collider.CompareTag("point"))
		{ 
			forward = sensors[1].distance;
			sensors[1].collider.tag.ToString();
			Debug.Log(sensors[1].collider.tag.ToString());
		}

		if (sensors[2].collider != cave1.GetComponent<BoxCollider2D>() || sensors[2].collider != cave2.GetComponent<BoxCollider2D>() || !sensors[1].collider.CompareTag("point"))
		{ 
			down = sensors[2].distance;

		}

		currNN.input.matrix[0, 0] = (2f / rayDist) * up - 1f;
		currNN.input.matrix[1, 0] = (2f / rayDist) * forward - 1f;
		currNN.input.matrix[2, 0] = (2f / rayDist) * down - 1f;
		currNN.input.matrix[3, 0] = GetComponent<Rigidbody2D>().velocity[1];
		currNN.FeedForward();


		//heliControl.vertical = currNN.output.matrix[0, 0];

		currCarPos = transform.position;
		totalDist += Vector3.Distance(currCarPos, lastCarPos);
		lastCarPos = currCarPos;

		timePassed += Time.deltaTime;

	}

	void OnCollisionEnter()
	{
		//population.SetFitnessOfCurrIndividual(totalDist, timePassed);
		currNN = population.Next();
		ResetCarPosition();
	}

	void ResetCarPosition()
	{
		transform.position = startPosition;
		transform.rotation = startRotation;
		currCarPos = startPosition;
		lastCarPos = startPosition;

		driveController.SetMotorTorque(0f);
        driveController.GetComponent<Rigidbody>().velocity = Vector3.zero;
		totalDist = 0f;
		timePassed = 0f;
	}

	//void DrawSensorLines()
	//{
	//	Color middleSensorColor, leftSensorColor, rightSensorColor, leftMiddleSensorColor, rightMiddleSensorColor;
	//	middleSensorColor = (sensors[0].collider == null) ? Color.green : Color.red;
	//	leftSensorColor   = (sensors[1].collider == null) ? Color.green : Color.red;
	//	rightSensorColor  = (sensors[2].collider == null) ? Color.green : Color.red;


  //      if (lines == null)
		//{
		//	lines = new GameObject[5];
		//	DrawLine(
		//		raycastPoint.position,
		//		(raycastPoint.position + raycastPoint.forward * rayDist), 
		//		middleSensorColor,
		//		0
		//	);

		//	DrawLine(
		//		raycastPoint.position,
		//		(raycastPoint.position + (raycastPoint.forward - raycastPoint.right) * rayDist),
		//		leftMiddleSensorColor,
		//		1
		//	);

		//	DrawLine(
		//		raycastPoint.position,
		//		(raycastPoint.position + (raycastPoint.forward + raycastPoint.right) * rayDist),
		//		rightMiddleSensorColor,
		//		2
		//	);

  //          DrawLine(
  //              raycastPoint.position,
  //              (raycastPoint.position + (-raycastPoint.right) * rayDist),
  //              leftMiddleSensorColor,
  //              3
  //          );

  //          DrawLine(
  //              raycastPoint.position,
  //              (raycastPoint.position + raycastPoint.right * rayDist),
  //              rightMiddleSensorColor,
  //              4
  //          );
  //      }
		//else
		//{
		//	UpdateLine(
		//		raycastPoint.position,
		//		(raycastPoint.position + raycastPoint.forward * rayDist), 
		//		middleSensorColor,
		//		0
		//	);

		//	UpdateLine(
		//		raycastPoint.position,
		//		(raycastPoint.position + (raycastPoint.forward - raycastPoint.right).normalized * rayDist),
		//		leftMiddleSensorColor,
		//		1
		//	);

		//	UpdateLine(
		//		raycastPoint.position,
		//		(raycastPoint.position + (raycastPoint.forward + raycastPoint.right).normalized * rayDist),
		//		rightMiddleSensorColor,
		//		2
		//	);

  //          UpdateLine(
  //              raycastPoint.position,
  //              (raycastPoint.position + (-raycastPoint.right).normalized * rayDist),
  //              leftSensorColor,
  //              3
  //          );

  //          UpdateLine(
  //              raycastPoint.position,
  //              (raycastPoint.position + raycastPoint.right.normalized * rayDist),
  //              rightSensorColor,
  //              4
  //          );
        //}

	//}
		
	void DrawLine(Vector3 start, Vector3 end, Color color, int lineIndex)
	{
		GameObject line = new GameObject();
		line.name = "Line " + lineIndex;
		line.transform.SetParent(environments.transform);

		line.transform.position = start;
		line.AddComponent<LineRenderer>();
		LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = lineRendererMaterial; //new Material(Shader.Find("Particles/Priority Alpha Blended"));
		lr.startColor = color;
		lr.endColor = color;
		lr.startWidth = 0.05f;
		lr.endWidth = 0.05f;
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);

		lines[lineIndex] = line;
	}

	void UpdateLine(Vector3 start, Vector3 end, Color color, int lineIndex)
	{
		LineRenderer lr = lines[lineIndex].GetComponent<LineRenderer>();
		lr.startColor = color;
		lr.endColor = color;
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
	}
}
