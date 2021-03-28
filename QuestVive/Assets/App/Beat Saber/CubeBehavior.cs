using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior : MonoBehaviour
{

    public float MoveSpeed = 10; // m/s
    public Vector3 moveDirection;
    public float duration = 30;
    public bool isLeft;
    public Material LeftMat;
    public Material RightMat;


    // Start is called before the first frame update
    IEnumerator Move()
    {
        float acc = 0;
        while (acc < duration)
        {
            if (acc < 4f)
            {
                transform.Translate(moveDirection * MoveSpeed * Time.deltaTime);
            }
            else 
            { 
                transform.Translate(moveDirection * MoveSpeed/2 * Time.deltaTime);

            }

            acc += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    public void Initialize()
    {
        Material mat = isLeft ? LeftMat : RightMat;
        gameObject.tag = isLeft ? "LeftCube" : "RightCube";

        transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat;
        transform.GetChild(1).gameObject.GetComponent<Renderer>().material = mat;
        StartCoroutine(Move());

    }
}
