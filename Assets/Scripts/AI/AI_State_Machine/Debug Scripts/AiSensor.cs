using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Serialization;

public class AiSensor : MonoBehaviour
{
    public float Distance = 10f;
    public float Angle = 30f;
    public float Height = 1f;
    public Color MeshColor = Color.red;
    public int ScanFrequency = 30;
    public LayerMask Layers;

    Collider[] _Colliders = new Collider[50];
    Mesh _Mesh;
    int _Count = 0;
    private float _ScanInterval;
    private float _ScanTimer;


    // Start is called before the first frame update
    void Start()
    {
        _ScanInterval = 1f / ScanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        _ScanTimer -= Time.deltaTime;
        if (_ScanTimer < 0)
        {
			_ScanTimer += _ScanInterval;
			Scan();
		}
    }

    private void Scan()
    {
	    _Count = Physics.OverlapSphereNonAlloc(transform.position, Distance, _Colliders, Layers,
		    QueryTriggerInteraction.Collide);    
    }

    Mesh CreateWedgeMesh()
    {
	    Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -Angle, 0) * Vector3.forward * Distance;
        Vector3 bottomRight = Quaternion.Euler(0, Angle, 0) * Vector3.forward * Distance;

	    Vector3 topCenter = bottomCenter + Vector3.up * Height;
        Vector3 topLeft = bottomLeft + Vector3.up * Height;
        Vector3 topRight = bottomRight + Vector3.up * Height;

        int vert = 0;

        // left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -Angle;
        float deltaAngle = (Angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * Distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * Distance;

            topLeft = bottomLeft + Vector3.up * Height;
            topRight = bottomRight + Vector3.up * Height;

            // far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

			currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
		}

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    private void OnValidate()
    {
		_Mesh = CreateWedgeMesh();
        _ScanInterval = 1f / ScanFrequency;
	}

    private void OnDrawGizmos()
    {
	    if (_Mesh)
	    {
		    Gizmos.color = MeshColor;
		    Gizmos.DrawMesh(_Mesh, transform.position, transform.rotation);
	    }

        Gizmos.DrawWireSphere(transform.position, Distance);
        for (int i = 0; i < _Count; i++)
        {
	        Gizmos.DrawSphere( _Colliders[i].transform.position, 0.2f);
		}
    }
}
