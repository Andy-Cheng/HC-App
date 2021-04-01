using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{
    public float appearTime = 3f;
    public Collider collider;



    IEnumerator MoveUp()
    {
        Debug.Log("Move Up");
        float acc = 0f;
        while (acc < appearTime)
        {
            transform.Translate(Vector3.up * 4 / appearTime * Time.deltaTime);
            acc += Time.deltaTime;
            yield return null;
        }
        collider.enabled = true;
    
    }


    private void OnEnable()
    {
        collider.enabled = false;
        transform.Translate(-Vector3.up * 4);
        Debug.Log("On enabled");
        StartCoroutine(MoveUp());
    }
}
