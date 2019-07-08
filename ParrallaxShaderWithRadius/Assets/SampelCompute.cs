using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampelCompute : MonoBehaviour
{

    public ComputeShader compute;
    public RenderTexture result;
    int kernel;
    public float alpha=1.0f;
    public int[] randomData = new int[] { 15, 1645, 135, 567 };
    // Start is called before the first frame update
    void Start()
    {
        kernel = compute.FindKernel("CSMain");

        result = new RenderTexture(512, 512, 24);
        result.enableRandomWrite = true;
        result.Create();


        compute.SetFloat("a", alpha);
        compute.SetInt("amountOfObjects", randomData.Length);

        var buffer = new ComputeBuffer(randomData.Length, sizeof(int));
        buffer.SetData(randomData);
        compute.SetBuffer(kernel,"RD", buffer);

        compute.SetTexture(kernel, "Result", result);
        
        compute.Dispatch(kernel, 512 / 8, 512 / 8, 1);

        buffer.GetData(randomData);
        buffer.Release();

    }

    // Update is called once per frame
    void Update()
    {
        
        compute.SetFloat("a", alpha);
        compute.Dispatch(kernel, 512 / 8, 512 / 8, 1);
        

    }
}
