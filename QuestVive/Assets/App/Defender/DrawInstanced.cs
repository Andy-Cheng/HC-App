using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawInstanced : MonoBehaviour
{
    // How many meshes to draw.
    public int population;
    // Range to draw meshes within.
    public float range;

    // Material to use for drawing the meshes.
    public Material material;

    private Matrix4x4[] matrices;

    public Mesh mesh;
    bool isSetUp = false;
    public float BoundingBoxRange = 2;
    [Range(1, 5)]
    public float ScaleRangeMax = 2;

    private void Setup()
    {
        matrices = new Matrix4x4[population];
        Vector3 environmentPos = transform.position;

        for (int i = 0; i < population; i++)
        {
            // Build matrix.
            Vector3 position = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
            if (InRange(position.x, -BoundingBoxRange, BoundingBoxRange) || InRange(position.y, -BoundingBoxRange, BoundingBoxRange) || InRange(position.z, -BoundingBoxRange, BoundingBoxRange))
            {
                position += Vector3.one * 10 * BoundingBoxRange;

                //Debug.Log( $"InRange: {environmentPos}, {position}");

            }

            position += environmentPos;
            Quaternion rotation = Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
            float scaleFactor = Random.Range(1, ScaleRangeMax);
            Vector3 scale = Vector3.one * scaleFactor;
            matrices[i] = Matrix4x4.TRS(position, rotation, scale); ;

        }
        isSetUp = true;
    }


    bool InRange(float value, float min, float max)
    {
        return (value > min) && (value < max);
    }


    private void Awake()
    {
        Setup();
    }


    private void Start()
    {

    }

    private void Update()
    {
        if (isSetUp)
        {
            // Draw a bunch of meshes each frame.
            Graphics.DrawMeshInstanced(mesh, 0, material, matrices, population);
        }
    }
}