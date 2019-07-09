using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampelCompute : MonoBehaviour
{

    public ComputeShader compute;
    public RenderTexture result;
    public int amountOfObjects = 50;
    public GameObject objectPrefab;
    public GameObject playerPrefab;
    int kernel;



    public Vector3[] randomPoints;
    // Start is called before the first frame update
    void Start()
    {
        randomPoints = new Vector3[amountOfObjects];

        InstantiateObjects();
        kernel = compute.FindKernel("CSMain");

        result = new RenderTexture(512, 512, 24);
        result.enableRandomWrite = true;
        result.Create();


       
        compute.SetInt("amountOfObjects", randomPoints.Length);
        compute.SetVector("playerObject", playerPrefab.transform.position);

        var buffer = new ComputeBuffer(randomPoints.Length, sizeof(float)*3);
        buffer.SetData(randomPoints);
        compute.SetBuffer(kernel,"RP", buffer);

        compute.SetTexture(kernel, "Result", result);

        compute.Dispatch(kernel, 512 / 8, 512 / 8, 1);

        buffer.GetData(randomPoints);
        buffer.Release();


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
