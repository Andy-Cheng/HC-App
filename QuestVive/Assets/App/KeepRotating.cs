using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRotating : MonoBehaviour
{
    public float AngularSpeed;
    public Vector3 RotateAround;

    bool stopRotating = false;


    IEnumerator Rotate()
    {
        while (!stopRotating)
        {
            transform.Rotate(RotateAround * AngularSpeed * Time.deltaTime);
            yield return null;
        }
    
    }

    private void OnEnable()
    {
        stopRotating = false;
        StartCoroutine(Rotate());
    }

    private void OnDisable()
    {
        stopRotating = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
