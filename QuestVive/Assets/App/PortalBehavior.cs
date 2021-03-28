using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{
    public float appearTime = 3f;
    public Collider frontDoorCollider;



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
        frontDoorCollider.enabled = true;
    
    }


    private void OnEnable()
    {
        frontDoorCollider.enabled = false;
        transform.Translate(-Vector3.up * 4);
        Debug.Log("On enabled");
        StartCoroutine(MoveUp());
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
