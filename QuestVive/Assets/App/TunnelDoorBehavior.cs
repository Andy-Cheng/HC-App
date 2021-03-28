using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelDoorBehavior : MonoBehaviour
{
    public bool isBackDoor = false;

    public float DoorOpeningDuration = 0.5f;
    public float DoorMoveDistance = .5f;
    public float DoorOpenDelay = 1f;

    public Transform LeftDoorTransform;
    public Transform RightDoorTransform;

    bool DoorIsMoving = false;
    bool doorClosed = true;


    public void OpenDoor()
    {


        if (!doorClosed)
        {
            return;
        }
        doorClosed = false;
        StartCoroutine(DoorOpening());
    }

    IEnumerator DoorOpening()
    {

        yield return new WaitForSeconds(DoorOpenDelay);
        while (DoorIsMoving)
        {
            yield return null;
        }
        DoorIsMoving = true;
        

        float acc = 0f;
        while (acc < DoorOpeningDuration)
        {
            LeftDoorTransform.Translate( Vector3.left * DoorMoveDistance /DoorOpeningDuration * Time.deltaTime , Space.Self);
            RightDoorTransform.Translate(Vector3.right * DoorMoveDistance / DoorOpeningDuration * Time.deltaTime, Space.Self);
            acc += Time.deltaTime;
            yield return null;
        }
        DoorIsMoving = false;
    }

    public void CloseDoor()
    {
        if (doorClosed)
        {
            return;
        }
        doorClosed = true;
        StartCoroutine(DoorClosing());
    }

    IEnumerator DoorClosing()
    {
        while (DoorIsMoving)
        {
            yield return null;
        }
        DoorIsMoving = true;

        float acc = 0f;
        while (acc < DoorOpeningDuration)
        {
            LeftDoorTransform.Translate(Vector3.right * DoorMoveDistance / DoorOpeningDuration * Time.deltaTime, Space.Self);
            RightDoorTransform.Translate(Vector3.left * DoorMoveDistance / DoorOpeningDuration * Time.deltaTime, Space.Self);
            acc += Time.deltaTime;
            yield return null;
        }
        DoorIsMoving = false;
    }



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // testing 
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenDoor();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CloseDoor();
        }

    }
}
