﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampelCompute : MonoBehaviour
{

    public ComputeShader distanceCompute;
    public ComputeShader returnKNNCompute;
    public int amountOfObjects = 50;
    public GameObject objectPrefab;
    public GameObject playerPrefab;
    public int k =3;
    int kernel;


    public GameObject[] objectPrefabs;
    public Vector3[] randomPoints;
    public float[] returnedDistance;
    public float[] returnedDistanceCopy;
    public int[] knn;
    // Start is called before the first frame update
    void Start()
    {

        randomPoints = new Vector3[amountOfObjects];
        returnedDistance = new float[amountOfObjects];
       
        knn = new int[k];

        objectPrefabs = new GameObject[amountOfObjects];

        InstantiateObjects();
        CalculateDistance();
        CalculateKNN();
        ColorObjects();


    }

    // Update is called once per frame
    void Update()
    {

            ColorObjectsWhite();
            CalculateDistanceDynamic();
            CalculateKNNDynamic();
            ColorObjects();
        

    }

    public void CalculateDistance()
    {

        kernel = distanceCompute.FindKernel("CSMain");


        distanceCompute.SetInt("amountOfObjects", randomPoints.Length);
        distanceCompute.SetVector("playerPos", playerPrefab.transform.position);

        var buffer = new ComputeBuffer(randomPoints.Length, sizeof(float) * 3);
        buffer.SetData(randomPoints);
        distanceCompute.SetBuffer(kernel, "RP", buffer);

        var bufferD = new ComputeBuffer(returnedDistance.Length, sizeof(float));
        bufferD.SetData(returnedDistance);
        distanceCompute.SetBuffer(kernel, "Distance", bufferD);


        distanceCompute.Dispatch(kernel, 1, 1, 1);

        buffer.Release();
        bufferD.GetData(returnedDistance);

        bufferD.Release();
    }

    public void CalculateDistanceDynamic()
    {

        distanceCompute.SetVector("playerPos", playerPrefab.transform.position);

       var buffer = new ComputeBuffer(randomPoints.Length, sizeof(float) * 3);
        buffer.SetData(randomPoints);
        distanceCompute.SetBuffer(kernel, "RP", buffer);

        var bufferD = new ComputeBuffer(returnedDistance.Length, sizeof(float));
        bufferD.SetData(returnedDistance);
        distanceCompute.SetBuffer(kernel, "Distance", bufferD);

    
        distanceCompute.Dispatch(kernel, 1, 1, 1);

        buffer.Release();
        bufferD.GetData(returnedDistance);

        bufferD.Release();
    }

    public void CalculateKNN()
    {
        kernel = returnKNNCompute.FindKernel("CSMain");

        returnKNNCompute.SetInt("amountOfObjects", randomPoints.Length);
        returnKNNCompute.SetInt("k", k);

        var bufferD = new ComputeBuffer(returnedDistance.Length, sizeof(float));
        bufferD.SetData(returnedDistance);
        returnKNNCompute.SetBuffer(kernel, "Distance", bufferD);

        var buffer = new ComputeBuffer(knn.Length, sizeof(int));
        buffer.SetData(knn);
        returnKNNCompute.SetBuffer(kernel, "Knn", buffer);


        returnKNNCompute.Dispatch(kernel,  1, 1, 1);

        buffer.GetData(knn);
        buffer.Release();

        bufferD.GetData(returnedDistance);
        bufferD.Release();

    }

    public void CalculateKNNDynamic()
    {
      

        var bufferD = new ComputeBuffer(returnedDistance.Length, sizeof(float));
        bufferD.SetData(returnedDistance);
        returnKNNCompute.SetBuffer(kernel, "Distance", bufferD);

        var buffer = new ComputeBuffer(knn.Length, sizeof(int));
        buffer.SetData(knn);
        returnKNNCompute.SetBuffer(kernel, "Knn", buffer);


        returnKNNCompute.Dispatch(kernel, 1, 1, 1);

        buffer.GetData(knn);
        buffer.Release();

        bufferD.GetData(returnedDistance);
        bufferD.Release();

    }


    public void InstantiateObjects()

    {

        for (int i = 0; i < amountOfObjects; i++)
        {
            GameObject tmp = Instantiate(objectPrefab);
            tmp.transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-0f, 0f));
            randomPoints[i] = tmp.transform.position;
            objectPrefabs[i] = tmp;
        }

    }


    public void ColorObjects()
    {
        for (int i = 0; i < k; i++)
        {
            objectPrefabs[knn[i]].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

    public void ColorObjectsWhite()
    {
        for (int i = 0; i < k; i++)
        {
            objectPrefabs[knn[i]].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
    }
}
