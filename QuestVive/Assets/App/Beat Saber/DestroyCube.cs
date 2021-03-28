using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LeftCube" || other.tag == "RightCube")
        {
            Destroy(other.gameObject);
            BeatSaberManager.instance.OnDestroyCube();
        
        }
    }
}
