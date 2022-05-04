using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class Water : MonoBehaviour
{
    private MeshFilter meshFilter; //Modify water surface
    
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }
    
    void Update()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;

        //Get vertex positions based on Sin waves
        for(int i = 0; i < vertices.Length; i++)
        {
            float x = Waves.instance.GetWaveHeight(transform.position.x + vertices[i].x);
            float z = Waves.instance.GetWaveHeight(transform.position.z + vertices[i].z);

            vertices[i].y = x + z;
        }
        
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
    }
}
