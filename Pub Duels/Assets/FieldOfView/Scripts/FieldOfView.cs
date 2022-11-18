using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class FieldOfView : MonoBehaviour {

    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private float fov;
    private float viewDistance;
    private Vector3 origin;
    private float startingAngle;

    private void Start() 
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 50f;
        viewDistance = 2f;
        origin = Vector3.zero;
    }

    private void LateUpdate() {
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++) {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, UtilsClass.GetVectorFromAngle(angle), viewDistance, layerMask);
            if (raycastHit2D.collider == null)
                // No hit
                vertex = origin + UtilsClass.GetVectorFromAngle(angle) * viewDistance; 
            else 
            {
                if (raycastHit2D.transform.position.y < origin.y)
                    // Hit object
                    vertex = origin + (raycastHit2D.transform.position - origin) * 2;
                else
                    vertex = origin + (raycastHit2D.transform.position - origin);
            }
            vertices[vertexIndex] = vertex;

            if (i > 0) 
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public void SetOrigin(Vector3 origin) 
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection) 
    {
        startingAngle = UtilsClass.GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }

    public void SetFoV(float fov) 
    {
        this.fov = fov;
    }

    public void SetViewDistance(float viewDistance) 
    {
        this.viewDistance = viewDistance;
    }

}
