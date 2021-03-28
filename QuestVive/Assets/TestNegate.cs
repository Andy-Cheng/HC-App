using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNegate : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 m = transform.parent.worldToLocalMatrix * transform.localToWorldMatrix;

        transform.parent.localRotation = m.inverse.rotation;
        Vector3 position;
        position.x = m.inverse.m03;
        position.y = m.inverse.m13;
        position.z = m.inverse.m23;
        transform.parent.localPosition = position;

    }
}
