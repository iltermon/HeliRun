using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{ public GameObject bType = null;
    void Start()
    {
        
    }

    private void Update()
    {
        int i = 0;
        while (true)
        {
            GameObject bx = GameObject.Instantiate(bType);

            float xPosition = i + i / 1.6f;
            float yPoistion = -6f + Mathf.PerlinNoise(i / 42f, 0) * 4f;

            bx.transform.position = new Vector3(xPosition, yPoistion);
            i++;
        }
    }


}
