using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDetect : MonoBehaviour
{
    public bool isLeft;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LeftCube")
        {
            if (isLeft)
            {
                Destroy(other.gameObject);
                BeatSaberManager.instance.OnHitCube();
                BeatSaberManager.instance.OnDestroyCube();
            }
        }

        if (other.tag == "RightCube")
        {
            if (!isLeft)
            {
                Destroy(other.gameObject);
                BeatSaberManager.instance.OnHitCube();
                BeatSaberManager.instance.OnDestroyCube();
            }
        }
    }
}
