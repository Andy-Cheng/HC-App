using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class User : MonoBehaviour
{
    public static User instance;
    public Transform UserCamRotationalOffset;
    public Transform Cam;
    public OVRCameraRig ovrRig;
    public float ResetDuration = 5f;

    public bool HaveSetCamera = false;
    public bool ShouldSetCamera = true;

    public float EffectTransitionTime = 1f;
    public PostProcessVolume volume;
    public TMP_Text UserStatusText;
    public bool haveSentCalibration;
    Vignette vignette;

    Quaternion negateRotation;
    Vector3 negatePosition;

    void EnableVignette(bool enable)
    {
        //StartCoroutine(VignetteCoroutine(enable));
        vignette.intensity.value = enable? 1:0;

    }

    //IEnumerator VignetteCoroutine(bool enable)
    //{
    //    float increment = enable ? 1 / EffectTransitionTime : -1 / EffectTransitionTime;
    //    float acc = 0;
        
    //    while (acc < EffectTransitionTime)
    //    {
    //        vignette.intensity.value += increment * Time.deltaTime; ;
    //        yield return null;
    //    }
    //}


    void RecieveCamTransform(Vector3 pos, Quaternion rot)
    {
        Debug.Log("Recieve cam transform");
        if (!HaveSetCamera)
        {
            StartCoroutine(ResetCamCoroutine());
            HaveSetCamera = true;

        }
        else
        {
            if (!haveSentCalibration)
            { 
                ClientSend.NotifyCalibrationDone();
                haveSentCalibration = true;
            }

        }
        if (ShouldSetCamera)
        {
            transform.position = pos;
            transform.rotation = rot;
        }
    }

    IEnumerator ResetCamCoroutine()
    {
        volume.enabled = true;

        EnableVignette(true);
        UserStatusText.text = "Calibrating...\nDon't move your head";
        ShouldSetCamera = true;
        yield return new WaitForSeconds(ResetDuration);
        ShouldSetCamera = false;
        EnableVignette(false);
        //ClientSend.NotifyCalibrationDone();
        //UserStatusText.text = "Calibration done\n Press the button to select your prop";
        volume.enabled = false;
        //yield return new WaitForSeconds(2);
        UserStatusText.text = "";
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
        UserStatusText.text = "";
        volume.profile.TryGetSettings(out vignette);
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
