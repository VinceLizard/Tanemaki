using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullGrass : MonoBehaviour
{
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        float[] distances = new float[32];
        distances[14] = 75;
        camera.layerCullDistances = distances;
    }
}
