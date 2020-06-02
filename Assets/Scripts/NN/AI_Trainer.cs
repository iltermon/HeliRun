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
	RaycastHit[] sensors;
	GameObject environments;
	GameObject[] lines;
    public Material lineRendererMaterial;
	GameObject cave1;
	GameObject cave2;
	Vector3 startPosition;
	Quaternion startRotation;

	Vector3 currCarPos;
	Vector3 lastCarPos;
	public float totalDist;
	public float timePassed;

	public float timeScale = 1f;

	float rayDist = 11;

	// Use this for initialization
	void Start ()
	{
		population = new Population(10, new int[]{5, 200, 2}, 1f);

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

		sensors = new RaycastHit[3];

		Physics.Raycast(raycastPoint.position, raycastPoint.up, out sensors[0], rayDist);
		Physics.Raycast(raycastPoint.position, raycastPoint.forward, out sensors[1], rayDist);
		Physics.Raycast(raycastPoint.position, -raycastPoint.up, out sensors[2], rayDist);

        DrawSensorLines();

		float forward, up, down;
		forward = down = up = rayDist;

		if(sensors[0].collider !=cave1.GetComponent<BoxCollider2D>() || sensors[0].collider !=cave2.GetComponent<BoxCollider2D>())
			up = sensors[0].distance;

		if(sensors[1].collider !=cave1.GetComponent<BoxCollider2D>() || sensors[1].collider !=cave2.GetComponent<BoxCollider2D>())
			forward = sensors[1].distance;

		if(sensors[2].collider !=cave1.GetComponent<BoxCollider2D>() || sensors[2].collider !=cave2.GetComponent<BoxCollider2D>())
			down = sensors[2].distance;

        double[] inputs = new double[5];
		inputs[0] = (2f / rayDist) * forward - 1f;
		inputs[1] = (2f / rayDist) * left - 1f;
		inputs[2] = (2f / rayDist) * right - 1f;
        inputs[3] = (2f / rayDist) * leftMid - 1f;
        inputs[4] = (2f / rayDist) * rightMid - 1f;

		double[] outputs;
		outputs = currNN.FeedForward(inputs);

		helicontrol.vertical=currNN.output[0,0];

		currCarPos = transform.position;
		totalDist += Vector3.Distance(currCarPos, lastCarPos);
		lastCarPos = currCarPos;

		timePassed += Time.deltaTime;
	}

	void OnCollisionEnter()
	{
		population.SetFitnessOfCurrIndividual(totalDist, timePassed);
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

	void DrawSensorLines()
	{
		Color middleSensorColor, leftSensorColor, rightSensorColor, leftMiddleSensorColor, rightMiddleSensorColor;
		middleSensorColor = (sensors[0].collider == null) ? Color.green : Color.red;
		leftSensorColor   = (sensors[1].collider == null) ? Color.green : Color.red;
		rightSensorColor  = (sensors[2].collider == null) ? Color.green : Color.red;
        leftMiddleSensorColor = (sensors[3].collider == null) ? Color.green : Color.red;
        rightMiddleSensorColor = (sensors[4].collider == null) ? Color.green : Color.red;

        if (lines == null)
		{
			lines = new GameObject[5];
			DrawLine(
				raycastPoint.position,
				(raycastPoint.position + raycastPoint.forward * rayDist), 
				middleSensorColor,
				0
			);

			DrawLine(
				raycastPoint.position,
				(raycastPoint.position + (raycastPoint.forward - raycastPoint.right) * rayDist),
				leftMiddleSensorColor,
				1
			);

			DrawLine(
				raycastPoint.position,
				(raycastPoint.position + (raycastPoint.forward + raycastPoint.right) * rayDist),
				rightMiddleSensorColor,
				2
			);

            DrawLine(
                raycastPoint.position,
                (raycastPoint.position + (-raycastPoint.right) * rayDist),
                leftMiddleSensorColor,
                3
            );

            DrawLine(
                raycastPoint.position,
                (raycastPoint.position + raycastPoint.right * rayDist),
                rightMiddleSensorColor,
                4
            );
        }
		else
		{
			UpdateLine(
				raycastPoint.position,
				(raycastPoint.position + raycastPoint.forward * rayDist), 
				middleSensorColor,
				0
			);

			UpdateLine(
				raycastPoint.position,
				(raycastPoint.position + (raycastPoint.forward - raycastPoint.right).normalized * rayDist),
				leftMiddleSensorColor,
				1
			);

			UpdateLine(
				raycastPoint.position,
				(raycastPoint.position + (raycastPoint.forward + raycastPoint.right).normalized * rayDist),
				rightMiddleSensorColor,
				2
			);

            UpdateLine(
                raycastPoint.position,
                (raycastPoint.position + (-raycastPoint.right).normalized * rayDist),
                leftSensorColor,
                3
            );

            UpdateLine(
                raycastPoint.position,
                (raycastPoint.position + raycastPoint.right.normalized * rayDist),
                rightSensorColor,
                4
            );
        }

	}
		
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
