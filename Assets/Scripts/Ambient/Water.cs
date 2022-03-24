using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class Water : MonoBehaviour
{
    private MeshFilter meshFilter;
    // Start is called before the first frame update
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;
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
