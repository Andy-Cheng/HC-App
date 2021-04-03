using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public struct PanelData
{
    public int RedBtn;
    public int BlueBtn;
    public int Slider1;
    public int Slider2;
    public int Slider3;
    public int Slider4;
    public int X;
    public int Y;
    public int Degree;

    public PanelData(int redBtn, int blueBtn, int slider1, int slider2, int slider3, int slider4, int x, int y, int degree)
    {
        RedBtn = redBtn;
        BlueBtn = blueBtn;
        Slider1 = slider1;
        Slider2 = slider2;
        Slider3 = slider3;
        Slider4 = slider4;
        X = x;
        Y = y;
        Degree = degree;
    }
}

public enum TrackerNum
{
    HC_Origin = 0,
    Player2 = 2,
    Player3 = 3,
    Shifty = 40,
    Shifty_Cartridge = 41,
    Panel = 50,
    Vive_Controller_Left = 60,
    Vive_Controller_Right = 61,
    Controller_Cartridge = 62,
    Gun = 70,
    Gun_Cartridge = 71,
    Shield = 80,
    Shield_Cartridge = 81,
}


// provides actions for each game manger to hook
public class DeviceManager : MonoBehaviour
{
    public static DeviceManager instance;
    public Dictionary<int, Action<Vector3, Quaternion>> OnRecieveTrackerTransforms;
    // OnRecieveTrackerTransform actions                              // Tracker  Num
    public Action<Vector3, Quaternion> OnRecieveHCOrigin;             // 0
    public Action<Vector3, Quaternion> OnRecievePlayer2;              // 2
    public Action<Vector3, Quaternion> OnRecievePlayer3;              // 3
    public Action<Vector3, Quaternion> OnRecieveShifty;               // 40
    public Action<Vector3, Quaternion> OnRecieveShiftyCartridge;      // 41
    public Action<Vector3, Quaternion> OnRecievePanel;                // 50
    public Action<Vector3, Quaternion> OnRecieveControllerLeft;       // 60
    public Action<Vector3, Quaternion> OnRecieveControllerRight;      // 61
    public Action<Vector3, Quaternion> OnRecieveControllerCartridge;  // 62
    public Action<Vector3, Quaternion> OnRecieveGun;                  // 70
    public Action<Vector3, Quaternion> OnRecieveGunCartridge;         // 71
    public Action<Vector3, Quaternion> OnRecieveShield;               // 80
    public Action<Vector3, Quaternion> OnRecieveShieldCartridge;      // 81
    public Action<PanelData> OnRecievePanelData;
    public Action OnGunFire;

    public void RecieveTrackerTransform(int trackerNum, Vector3 pos, Quaternion rot)
    {
        // call OnRecieveTrackerTransforms
        Debug.Log($"device manager recieve {trackerNum} transform");
        OnRecieveTrackerTransforms[trackerNum]?.Invoke(pos, rot);


    }

    public void RecievePanelData(PanelData panelData)
    {
        Debug.Log($"Decieve Manager recieve panel data {panelData}");

        OnRecievePanelData?.Invoke(panelData);
    }

    public void RecieveGunSignal()
    {
        OnGunFire?.Invoke();
    }

    void Initialize()
    {
        OnRecieveTrackerTransforms = new Dictionary<int, Action<Vector3, Quaternion>>();
        OnRecieveTrackerTransforms.Add(0, OnRecieveHCOrigin);
        OnRecieveTrackerTransforms.Add(2, OnRecievePlayer2);
        OnRecieveTrackerTransforms.Add(3, OnRecievePlayer3);
        OnRecieveTrackerTransforms.Add(40, OnRecieveShifty);
        OnRecieveTrackerTransforms.Add(41, OnRecieveShiftyCartridge);
        OnRecieveTrackerTransforms.Add(50, OnRecievePanel);
        OnRecieveTrackerTransforms.Add(60, OnRecieveControllerLeft);
        OnRecieveTrackerTransforms.Add(61, OnRecieveControllerRight);
        OnRecieveTrackerTransforms.Add(62, OnRecieveControllerCartridge);
        OnRecieveTrackerTransforms.Add(70, OnRecieveGun);
        OnRecieveTrackerTransforms.Add(71, OnRecieveGunCartridge);
        OnRecieveTrackerTransforms.Add(80, OnRecieveShield);
        OnRecieveTrackerTransforms.Add(81, OnRecieveShieldCartridge);

    }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        Initialize();
    }



    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
