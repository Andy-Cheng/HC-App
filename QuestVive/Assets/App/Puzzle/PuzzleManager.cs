using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PuzzleGameState
{ 
    TurnStick,
    InputDigit,
    RotateWheel,
    GameFinish,
}


public class PuzzleManager : MonoBehaviour
{
    //public GameObject ButtonLeft;
    //public Transform ButtonReplace;
    //Transform modelTransform;

    public static PuzzleManager instance;

    public GameObject Portal;
    public GameObject Shield;
    public TMP_Text StatusText;
    public GameObject InputCanvas;
    public List<TMP_Text> DigitTexts;

    public Transform Stick;
    public List<Transform> Sliders;
    public Transform Wheel;
    

    public int CountShouldPress = 14;
    public PuzzleGameState GameState;

    public float MaxStickRotation_X = 60f; //   >0: down
    public float MaxStickRotation_Z = 90f; // >0: right
    public int MaxDegreeDelta = 5;
    public int DegreeThreshold = 360; 
    public int TotalRotationDegree = 0;

    int code;
    int lastDegree;
    bool haveStartRotate;


    bool CheckRotateFinish(int newDegree)
    {
        int delta = newDegree - lastDegree;
        if (delta > MaxDegreeDelta)
        {
            delta = -((359 - newDegree) + lastDegree ); 

        }
        else if (delta < -MaxDegreeDelta)
        {
            delta = 359 - lastDegree + newDegree;
        
        }
        TotalRotationDegree += delta;

        lastDegree = newDegree;

        
        
        return (TotalRotationDegree > DegreeThreshold || TotalRotationDegree < -DegreeThreshold)? true : false;
    }


    void RecievePanelData(PanelData data)
    {
        // Modify component's transform based on data
        // Joystick
        float rotation_x = Mathf.Lerp(MaxStickRotation_X, -MaxStickRotation_X, (float)data.Y / 1023f) ;
        float rotation_z = Mathf.Lerp(-MaxStickRotation_Z, MaxStickRotation_Z, (float)data.X / 1023f);
        Stick.transform.localRotation = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(rotation_x, 0f, rotation_z), Vector3.one).rotation;

        // Wheel
        Wheel.localRotation = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, (float)data.Degree, 0f), Vector3.one).rotation;


        // Slider
        List<int> Digits = new List<int>() {data.Slider4, data.Slider3, data.Slider2, data.Slider1};
        int myInput = 0;
        for (int i = 0; i < Digits.Count; ++i)
        {
            DigitTexts[i].text = Digits[i].ToString();
        }
        myInput += Digits[0];
        myInput += Digits[1] * 10;
        myInput += Digits[1] * 100;
        myInput += Digits[1] * 1000;

        // Maintain the current game state
        if (GameState == PuzzleGameState.InputDigit)
        {
            if (myInput == code)
            {
                OnEnterCorrectCode();
            }

        }
        else if (GameState == PuzzleGameState.RotateWheel)
        {
            if (CheckRotateFinish(data.Degree))
            {
                OnFinishTurnWheel();
            }

        }



    }



    public void RecieveCode(int otherScore)
    {
        code = otherScore;
        StatusText.text = "Input the code using sliders";
        InputCanvas.SetActive(true);
        GameState = PuzzleGameState.InputDigit;



    }

    void OnEnterCorrectCode()
    {
        GameState = PuzzleGameState.RotateWheel;
        InputCanvas.SetActive(false);
        StatusText.text = "Turn the wheel.";


    }

    void OnFinishTurnWheel()
    { 
        GameState = PuzzleGameState.GameFinish;
        StatusText.text = "Go back to portal";
        Shield.SetActive(true);
        // Set Portal
    }



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;

        // hook to devcice manager
        //DeviceManager.instance.OnRecievePanelData += RecievePanelData; // uncomment this
        GameState = PuzzleGameState.TurnStick;
        StatusText.text = "Turn and hold the stick. Then, press one button";
        InputCanvas.SetActive(false);
        Shield.SetActive(false);
        haveStartRotate = false;

        // test
        RecieveCode(1792);
    }
    void Start()
    {
        //modelTransform = ButtonReplace.parent;
        //Vector3 center = ButtonLeft.GetComponent<Renderer>().bounds.center;
        //Vector3 centerLocal = modelTransform.worldToLocalMatrix.MultiplyPoint3x4(center);
        //ButtonReplace.localPosition = centerLocal;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDestroy()
    {

    }
}

