using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PuzzleGameState
{ 
    TurnStick,
    InputDigit,
    PressButton,
    GameFinish,
}


public class PuzzleManager : MonoBehaviour
{
    //public GameObject ButtonLeft;
    //public Transform ButtonReplace;
    //Transform modelTransform;

    public static PuzzleManager instance;

    public  GameObject InputCanvas;
    public GameObject Shield;
    public TMP_Text StatusText;
    public List<TMP_Text> DigitTexts;

    public Transform Stick;
    public List<Transform> Sliders;
    public Transform Wheel;
    
    
    public int CountShouldPress = 14;
    public PuzzleGameState GameState;

    public float MaxStickRotation_X = 60f; //   >0: down
    public float MaxStickRotation_Z = 90f; // >0: right

    public float MaxSliderTranslation_Z = -.07f;

    public int MaxDegreeDelta = 5;
    public int DegreeThreshold = 360; 
    public int TotalRotationDegree = 0;

    public int code;
    public int lastDegree;
    public bool haveStartRotate;


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

        Debug.Log($"recieve panel data {data}");

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
            Sliders[i].localPosition = new Vector3(0, 0, MaxSliderTranslation_Z/ 9 * (float)Digits[i]);
        }
        myInput += Digits[0];
        myInput += Digits[1] * 10;
        myInput += Digits[1] * 100;
        myInput += Digits[1] * 1000;
        Debug.Log($"My input {myInput}, answer: {code}");

        // Maintain the current game state
        // Button
        if (GameState == PuzzleGameState.TurnStick)
        {
            if (data.BlueBtn == 1)
            {
                Debug.Log("Right btn pressed");
                CountShouldPress--;
                if (CountShouldPress == 0)
                {
                    // Testing     
                    RecieveCode(1792);
                }
            }

            if (data.RedBtn == 1)
            {
                Debug.Log("left btn pressed");
                CountShouldPress--;
                if (CountShouldPress == 0)
                {
                    // Testing     
                    RecieveCode(1792);
                }

            }
        }

        if (GameState == PuzzleGameState.InputDigit)
        {
            if (myInput == code)
            {
                OnEnterCorrectCode();
            }
        }
        else if (GameState == PuzzleGameState.PressButton)
        {
            //if (CheckRotateFinish(data.Degree))
            //{
            //    OnFinishTurnWheel();
            //}
            if (data.RedBtn == 1)
            {
                OnFinishGame();
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
        GameState = PuzzleGameState.PressButton;
        InputCanvas.SetActive(false);
        StatusText.text = "Press the blue button";


    }

    void OnFinishGame()
    { 
        GameState = PuzzleGameState.GameFinish;
        StatusText.text = "The force-field shield is open.\nGo back to portal";
        Shield.SetActive(true);
        GeneralManager.instance.OnGameEnd();

    }

    void Initialize()
    {
        GameState = PuzzleGameState.TurnStick;
        StatusText.text = "Turn and hold the stick. Then, press one button";
        InputCanvas.SetActive(false);
        haveStartRotate = false;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;

        // hook to devcice manager
        DeviceManager.instance.OnRecievePanelData += RecievePanelData; 

        InputCanvas = transform.Find("Input Canvas").gameObject;
        Shield = transform.Find("Force Field Shield").gameObject;
        StatusText = transform.Find("Status Canvas/StatusText").gameObject.GetComponent<TMP_Text>();

        DigitTexts.Add(InputCanvas.transform.Find("Code4").gameObject.GetComponent<TMP_Text>());
        DigitTexts.Add(InputCanvas.transform.Find("Code3").gameObject.GetComponent<TMP_Text>());
        DigitTexts.Add(InputCanvas.transform.Find("Code2").gameObject.GetComponent<TMP_Text>());
        DigitTexts.Add(InputCanvas.transform.Find("Code1").gameObject.GetComponent<TMP_Text>());


        StatusText.text = "Waiting for other player to join";

 
    }
    void Start()
    {
        if (GeneralManager.instance.OtherEnterGame)
        {
            Initialize();
        }
        else
        {
            GeneralManager.instance.OnOtherEnterGame += Initialize;


        }

        // Testing
        //Initialize();

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDisable()
    {
        GeneralManager.instance.OnOtherEnterGame -= Initialize;
        DeviceManager.instance.OnRecievePanelData -= RecievePanelData;

    }
}

