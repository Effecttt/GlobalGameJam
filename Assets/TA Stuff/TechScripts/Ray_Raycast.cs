using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ray_Raycast : MonoBehaviour
{
    private Mesh mesh;
    
    public Vector3 start_pt;
    public Vector3 end_pt;
    
    public Vector3[] vertices;
    private Vector2[] uvs;
    int[] triangles;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        DrawPoly();
    }
    
    void Update()
    {
        //start_pt = transform.position;
        //Raycast();
        
        DrawPoly();
        UpdateMesh();
    }
    
    void Raycast()
    {
        //hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity);
    }

    private void DrawPoly()
    {
        vertices = new Vector3[]
        {
            start_pt + Vector3.left,
            start_pt + Vector3.right,
            end_pt + Vector3.left,
            end_pt + Vector3.right,
        };

        triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 1;
        triangles[4] = 3;
        triangles[5] = 2;

        uvs = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,0),
            new Vector2(1,1),
        };
    }
    
    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
    }

    private void OnDrawGizmos()
    {
        //Debug.DrawRay(transform.position, transform.position + transform.right * 5, Color.red, 0);
        
        Gizmos.DrawWireSphere(start_pt, 0.25f);
        Gizmos.DrawWireSphere(end_pt, 0.25f);

        for (int i = 0; i<vertices.Length; i++)
        {
            Gizmos.DrawWireSphere(vertices[i], 0.25f);
        }
    }
}
