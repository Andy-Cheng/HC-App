using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public struct SetTransform
//{
//    public bool haveSet;
//    public bool shouldSet;
//    public SetTransform(bool have, bool should)
//    {
//        haveSet = have;
//        shouldSet = should;
//    }
//}

public class PropManager : MonoBehaviour
{
    public DeviceManager deviceManager;

    public float CalibrationDuration = 5f;

    // Material attached to cartridge and cartridge holder
    public Material HolderMat;

    // Static (need calibration)
    public GameObject HC_Origin;
    public GameObject ShiftyCartridge;
    public GameObject Panel;
    public GameObject ControllerCartridge;
    public GameObject GunCartridge;
    public GameObject ShieldCartridge;

    // Device 
    public GameObject Sword;
    public GameObject LeftLightSaber;
    public GameObject RightLightSaber;
    public GameObject Gun;
    public GameObject Shield;

    // Other player visualization
    public GameObject OtherPlayer;
    public GameObject Droid;

    // Flags
    public bool HaveSetHC_Origin = false;
    bool ShouldSetHC_Origin = true;

    public bool HaveSetShiftyCartridge = false;
    bool ShouldSetShiftyCartridge = true;

    public bool HaveSetPanel = false;
    bool ShouldSetPanel = true;

    public bool HaveSetControllerCartridge = false;
    bool ShouldSetControllerCartridge = true;

    public bool HaveSetGunCartridge = false;
    bool ShouldSetGunCartridge = true;

    public bool HaveSetShieldCartridge = false;
    bool ShouldSetShieldCartridge = true;


    public void DisplayCartridge(bool shouldDisplay)
    {
        Color32 col = HolderMat.GetColor("_Color");
        if (shouldDisplay)
        { 
            col.a = 255;
            HolderMat.EnableKeyword("_EMISSION");
        }
        if (!shouldDisplay)
        {
            col.a = 0;
            HolderMat.DisableKeyword("_EMISSION");

        }
        HolderMat.SetColor("_Color", col);
    }

 

    IEnumerator CalibrationCoroutine(TrackerNum trackerNum)
    {

        if (trackerNum == TrackerNum.HC_Origin)
        {
            ShouldSetHC_Origin = true;
        }
        else if (trackerNum == TrackerNum.Shifty_Cartridge)
        {
            ShouldSetShiftyCartridge = true;
        }
        else if (trackerNum == TrackerNum.Panel)
        {
            ShouldSetPanel = true;
        }
        else if (trackerNum == TrackerNum.Controller_Cartridge)
        {
            ShouldSetControllerCartridge = true;
        }
        else if (trackerNum == TrackerNum.Gun_Cartridge)
        {
            ShouldSetGunCartridge = true;
        }
        else if (trackerNum == TrackerNum.Shield_Cartridge)
        {
            ShouldSetShieldCartridge = true;
        }

        yield return new WaitForSeconds(CalibrationDuration);

        if (trackerNum == TrackerNum.HC_Origin)
        {
            ShouldSetHC_Origin = false;

        }
        else if (trackerNum == TrackerNum.Shifty_Cartridge)
        {
            ShouldSetShiftyCartridge = false;
        }
        else if (trackerNum == TrackerNum.Panel)
        {
            ShouldSetPanel = false;

        }
        else if (trackerNum == TrackerNum.Controller_Cartridge)
        {
            ShouldSetControllerCartridge = false;

        }
        else if (trackerNum == TrackerNum.Gun_Cartridge)
        {
            ShouldSetGunCartridge = false;

        }
        else if (trackerNum == TrackerNum.Shield_Cartridge)
        {
            ShouldSetShieldCartridge = false;
        }

    }


    public void RecieveHCOrigin(Vector3 pos, Quaternion rot)
    {
        if (!HaveSetHC_Origin)
        {
            StartCoroutine(CalibrationCoroutine(TrackerNum.HC_Origin));
            HaveSetHC_Origin = true;
        }
        if (ShouldSetHC_Origin)
        {
            HC_Origin.transform.position = pos;
            HC_Origin.transform.rotation = rot;
        }
    }
    public void RecieveShiftyCartridge(Vector3 pos, Quaternion rot)
    {
        if (!HaveSetShiftyCartridge)
        {
            StartCoroutine(CalibrationCoroutine(TrackerNum.Shifty_Cartridge));
            HaveSetShiftyCartridge = true;
        }
        if (ShouldSetShiftyCartridge)
        {
            ShiftyCartridge.transform.position = pos;
            ShiftyCartridge.transform.rotation = rot;
        }
    }

    public void RecievePanel(Vector3 pos, Quaternion rot)
    {
        if (!HaveSetPanel)
        {
            StartCoroutine(CalibrationCoroutine(TrackerNum.Panel));
            HaveSetPanel = true;
        }

        if (ShouldSetPanel)
        {
            Panel.transform.position = pos;
            Panel.transform.rotation = rot;
        }
    }

    public void RecieveControllerCartridge(Vector3 pos, Quaternion rot)
    {
        if (!HaveSetControllerCartridge)
        {
            StartCoroutine(CalibrationCoroutine(TrackerNum.Controller_Cartridge));
            HaveSetControllerCartridge = true;
        }
        if (ShouldSetControllerCartridge)
        {
            ControllerCartridge.transform.position = pos;
            ControllerCartridge.transform.rotation = rot;
        }
    }

    public void RecieveGunCartridge(Vector3 pos, Quaternion rot)
    {
        Debug.Log("Shield");

        if (!HaveSetGunCartridge)
        {
            StartCoroutine(CalibrationCoroutine(TrackerNum.Gun_Cartridge));
            HaveSetGunCartridge = true;
        }
        if (ShouldSetGunCartridge)
        {
            GunCartridge.transform.position = pos;
            GunCartridge.transform.rotation = rot;
        }
    }

    public void RecieveShieldCartridge(Vector3 pos, Quaternion rot)
    {

        Debug.Log("Change Shield Cartrige transform");
        if (!HaveSetShieldCartridge)
        {
            StartCoroutine(CalibrationCoroutine(TrackerNum.Shield_Cartridge));
            HaveSetShieldCartridge = true;
        }
        if (ShouldSetShieldCartridge)
        {
            ShieldCartridge.transform.position = pos;
            ShieldCartridge.transform.rotation = rot;
        }
    }

    public void RecieveSword(Vector3 pos, Quaternion rot)
    {
        Sword.transform.position = pos;
        Sword.transform.rotation = rot;
    }

    public void RecieveLeftLightSaber(Vector3 pos, Quaternion rot)
    {
        LeftLightSaber.transform.position = pos;
        LeftLightSaber.transform.rotation = rot;
    }
    public void RecieveRightLightSaber(Vector3 pos, Quaternion rot)
    {
        RightLightSaber.transform.position = pos;
        RightLightSaber.transform.rotation = rot;
    }
    public void RecieveGun(Vector3 pos, Quaternion rot)
    {
        Gun.transform.position = pos;
        Gun.transform.rotation = rot;
    }

    public void RecieveShield(Vector3 pos, Quaternion rot)
    {
        Debug.Log("Change shield transform");
        Shield.transform.position = pos;
        Shield.transform.rotation = rot;
    }

    public void RecieveOtherPlayer(Vector3 pos, Quaternion rot)
    {
        OtherPlayer.transform.position = pos;
        OtherPlayer.transform.rotation = rot;
    }


    private void Awake()
    {

        deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.HC_Origin] += RecieveHCOrigin;
        deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.Shifty_Cartridge] += RecieveShiftyCartridge;
        deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.Panel] += RecievePanel;
        deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.Controller_Cartridge] += RecieveControllerCartridge;
        deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.Gun_Cartridge] += RecieveGunCartridge;
        deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.Shield_Cartridge] += RecieveShieldCartridge;

        deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.Shifty] += RecieveSword;
        deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.Vive_Controller_Left] += RecieveLeftLightSaber;
        deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.Vive_Controller_Right] += RecieveRightLightSaber;
        deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.Gun] += RecieveGun;
        deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.Shield] += RecieveShield;
        if (GeneralManager.instance.UserID == 2)
        {
            deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.Player3] += RecieveOtherPlayer;
        }
        else
        { 
            deviceManager.OnRecieveTrackerTransforms[(int)TrackerNum.Player2] += RecieveOtherPlayer;
        }
    }

}
