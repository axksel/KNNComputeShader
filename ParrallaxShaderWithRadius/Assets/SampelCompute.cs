using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampelCompute : MonoBehaviour
{

    public ComputeShader compute;
    public int amountOfObjects = 50;
    public GameObject objectPrefab;
    public GameObject playerPrefab;
    public int k =3;
    int kernel;



    public Vector3[] randomPoints;
    public float[] returnedKNeighbours;
    // Start is called before the first frame update
    void Start()
    {
        randomPoints = new Vector3[amountOfObjects];
        returnedKNeighbours = new float[k];

        InstantiateObjects();
        kernel = compute.FindKernel("CSMain");

       
        compute.SetInt("amountOfObjects", randomPoints.Length);
        compute.SetVector("playerObject", playerPrefab.transform.position);

        var buffer = new ComputeBuffer(randomPoints.Length, sizeof(float)*3);
        buffer.SetData(randomPoints);
        compute.SetBuffer(kernel,"RP", buffer);

        var bufferK = new ComputeBuffer(returnedKNeighbours.Length, sizeof(float));
        bufferK.SetData(returnedKNeighbours);
        compute.SetBuffer(kernel, "KNeighbours", bufferK);


        compute.Dispatch(kernel, 512 / 8, 512 / 8, 1);

        buffer.GetData(randomPoints);
        buffer.Release();

        bufferK.GetData(returnedKNeighbours);
        bufferK.Release();


    }

    // Update is called once per frame
    void Update()
    {
        
        //compute.SetFloat("a", alpha);
        //compute.Dispatch(kernel, 512 / 8, 512 / 8, 1);
        

    }

    public void InstantiateObjects()

    {

        for (int i = 0; i < amountOfObjects; i++)
        {
            GameObject tmp = Instantiate(objectPrefab);
            tmp.transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            randomPoints[i] = tmp.transform.position;
        }

    }
}
