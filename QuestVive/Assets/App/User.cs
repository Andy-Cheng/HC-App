using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public static User instance;
    public Transform UserCamRotationalOffset;
    public Transform Cam;
    public OVRCameraRig ovrRig;
    public float ResetDuration = 5f;

    public bool HaveSetCamera = false;
    public bool ShouldSetCamera = true;

    Quaternion negateRotation;
    Vector3 negatePosition;


    void RecieveCamTransform(Vector3 pos, Quaternion rot)
    {
        if (!HaveSetCamera)
        {
            StartCoroutine(ResetCamCoroutine());
            HaveSetCamera = true;
        
        }
        if (ShouldSetCamera)
        {
            transform.position = pos;
            transform.rotation = rot;
        }
    }

    IEnumerator ResetCamCoroutine()
    {
        ShouldSetCamera = true;
        yield return new WaitForSeconds(ResetDuration);
        ShouldSetCamera = false;

    }



    void NegateCameraTransform(OVRCameraRig cameraRig)
    {
        //Debug.Log("Update anchor callback");

        if (ShouldSetCamera)
        {
            Matrix4x4 m = cameraRig.centerEyeAnchor.parent.worldToLocalMatrix * cameraRig.centerEyeAnchor.localToWorldMatrix;
            negateRotation = m.inverse.rotation;
            negatePosition.x = m.inverse.m03;
            negatePosition.y = m.inverse.m13;
            negatePosition.z = m.inverse.m23;
        }



        cameraRig.centerEyeAnchor.parent.localRotation = negateRotation;
        cameraRig.centerEyeAnchor.parent.localPosition = negatePosition;

    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;

        Cam.SetParent(UserCamRotationalOffset, false);

    }

    // Start is called before the first frame update
    void Start()
    {
        ovrRig.UpdatedAnchors += NegateCameraTransform;
        DeviceManager.instance.OnRecieveTrackerTransforms[GeneralManager.instance.UserID] += RecieveCamTransform;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
