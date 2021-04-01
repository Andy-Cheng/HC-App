using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetect : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Other player hit {other.name}");
        if (other.tag == "PositionCollider")
        {
            other.enabled = false;
            //other.gameObject.GetComponent<MeshRenderer>().enabled = false; // Debug
            DefenderManager.instance.OnEnterTargetPosition();
        }
        if (other.tag == "TunnelCollider")
        {
            if (GeneralManager.instance.CurrentStageState == StageState.WAITING_DEVICE_READY)
            {
                other.enabled = false;
                Debug.Log("enter tunnel collider");
                other.transform.parent.Find("FrontDoor").GetComponent<TunnelDoorBehavior>().CloseDoor();
                GeneralManager.instance.EnterTunnelCollider();
            }
        }
        if (other.tag == "TunnelFrontDoor")
        {
            if (GeneralManager.instance.StartWalking || GeneralManager.instance.DeviceReady)
            {
                other.gameObject.GetComponent<TunnelDoorBehavior>().OpenDoor();
            }
        }

        if (other.tag == "TunnelLastDoor")
        {
            other.transform.parent.GetComponent<TunnelDoorBehavior>().CloseDoor();
            GeneralManager.instance.LeaveTunnel();
            Debug.Log("Leave last tunnel");
        }

        if (other.tag == "PortalBackDoor")
        {
            // Change the scene
            GeneralManager.instance.OnEnterPortal();
        }
    }
}
