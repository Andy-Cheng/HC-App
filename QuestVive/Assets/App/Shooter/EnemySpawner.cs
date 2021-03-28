using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float Speed;
    int childCount;
    
    // Start is called before the first frame update
    void Start()
    {
        childCount = transform.childCount;

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < childCount; ++i)
        {
            Speed = i < 5 ? 0.2f : 5f;
            Transform t = transform.GetChild(i);
            t.position += t.forward * Speed * Time.deltaTime;
        }
    }
}
