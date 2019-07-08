using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCalc : MonoBehaviour
{

    public Vector3 position;
    public GameObject player;
    Renderer render;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        position = player.transform.position;
        render.material.SetVector("_Position", position);
    }
}
