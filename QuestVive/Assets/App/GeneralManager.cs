using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using OculusSampleFramework;

public enum DeviceNum
{
    Shifty = 40,
    Panel = 50,
    Controller = 60,
    Gun = 70,
    Shield = 80,
}

public enum GameState
{ 
    NONE,
    CALIBRATION,
    BRANCH,
    ARENA,
    PLANET,
    COOPERATE

}

public enum StageState
{
    NONE,
    BRANCH,
    DISTRIBUTE_DEVICE,
    WAITING_DEVICE_READY,
    STAGE_START,
    STAGE_END
}


public class GeneralManager : MonoBehaviour
{
    public static GeneralManager instance;
    public PropManager propManager;

    public int UserID;
    
    public GameState CurrentGameState;
    public StageState CurrentStageState;

    public int myChoiceDeviceID;

    public Transform RotationOffset;

    //public event Action<int> OnStageStart; // Hook this event to every specific scene manager
    public event Action OnEnterTunnelCollider;
    public event Action OnLeaveTunnel;
    public event Action OnInitialize;

    public bool StartWalking = false; // turn on when both the users choose and waiting for the device, turn off when device ready
    public bool DeviceReady = false; // turn on when device ready
    
    public bool OtherEnterGame = false;
    public event Action OnOtherEnterGame; // ToDo if !OtherEnterGame and meisenter then hook a game scene's initialize to the action and unook



    GameObject ConfirmGrabBoard;
    GameObject ConfirmReturnBoard;
    [Space]
    [Header("Testing")]
    public GameObject TestUI;

    GameObject HCRoot;
    GameObject Scene;
    GameObject SelectionRoom;
    GameObject GrabObjectRoom;
    GameObject Portal;
    GameObject PropSelection;

    // Each game scene
    GameObject Arena;
    GameObject BeatSaber;
    GameObject Puzzle;
    GameObject Defender;
    GameObject Shooter;

    GameObject currentGameScene;

    Transform ArenaPortal;

    public Collider PortalCollider;
    public GameObject CurrentOtherProp;

    Dictionary<int, TrainButtonVisualController> deviceNumToBtn;
    GameObject CurrentPropCartridge;
    GameObject CurrentProp;

    //List<int> DeviceNums; // [left device num, right device num]


    public void WhenOtherEnterGame()
    {
        OtherEnterGame = true;
        OnOtherEnterGame?.Invoke();
    }


    public void OnGameEnd()
    {
        Portal.transform.localRotation = Quaternion.Euler(0, 180, 0);
        Portal.SetActive(true);


    }


    public void OnLeavePortal()
    { 
        Portal.SetActive(false);
    
    }


    public void EnterTunnelCollider()
    {
        
        OnEnterTunnelCollider.Invoke();
        SelectionRoom.SetActive(false);
    }
    public void LeaveTunnel()
    {
        OnLeaveTunnel.Invoke();

    }

    public void OnLeaveLastTunnel() { 
        GeneralManager.instance.propManager.OtherPlayer.SetActive(false);

    }


    public void OnEnterPortal()
    {
        if (CurrentStageState == StageState.WAITING_DEVICE_READY)
        {
            GrabObjectRoom.SetActive(false);
            if (CurrentGameState == GameState.ARENA)
            {
                Arena.SetActive(true);
                currentGameScene = Arena;
                
            }
            OnStageStateChange((int)StageState.STAGE_START);
        }
        else
        {
            currentGameScene.SetActive(false);
            GrabObjectRoom.SetActive(true);
            ConfirmReturnBoard.SetActive(true);

        }

    }


    // Call when users grab props
    public void OnGrabProp(TrainButtonVisualController dummy, int dummyInt)
    {
        if (CurrentGameState == GameState.ARENA)
        { 
            Portal.transform.SetParent(ArenaPortal, false);
        }
        Portal.SetActive(true);
        ConfirmGrabBoard.SetActive(false);

    }

    // Call when users return props
    public void OnReturnProp(TrainButtonVisualController dummy, int dummyInt)

    {
        ConfirmReturnBoard.SetActive(false);
        CurrentProp.SetActive(false);
        CurrentPropCartridge.SetActive(false);
        OnStageStateChange((int)StageState.STAGE_END);
    }

    public void OnSelectProp(TrainButtonVisualController btnController, int deviceNum)
    {
        Debug.Log($"User select {deviceNum}");
        myChoiceDeviceID = deviceNum;
        btnController.OnClick -= OnSelectProp;
        btnController.enabled = false;

        GameObject selectionText = PropSelection.transform.Find("Canvas/My Selection").gameObject;
        TMP_Text text = selectionText.GetComponent<TMP_Text>();
        DeviceNum id = (DeviceNum)deviceNum;
        text.text = id.ToString("f");
        deviceNumToBtn.Remove(deviceNum);
        // Remove the other btn from the dict. to prevent the user modify the selection
        int key = 0;
        int count = 0;
        foreach (KeyValuePair<int, TrainButtonVisualController> btn in deviceNumToBtn)
        {
            btn.Value.OnClick -= OnSelectProp;
            btn.Value.enabled = false;
            key = btn.Key;
            count++;
        }
        if (count == 1)
        { 
            deviceNumToBtn.Remove(key);
        }

        ClientSend.SendPlayerChoice(deviceNum);

    }

    public void OnOtherSelect(int deviceID)
    {
        if (deviceID != myChoiceDeviceID)
        {
            // check if the deviceID matches then disable
            Debug.Log($"Other user select {deviceID}");
            GameObject selectionText = PropSelection.transform.Find("Canvas/Other Selection").gameObject;
            TMP_Text text = selectionText.GetComponent<TMP_Text>();
            DeviceNum id = (DeviceNum)deviceID;
            text.text = id.ToString("f");

            if (deviceNumToBtn.ContainsKey(deviceID))
            {
                TrainButtonVisualController btnController = deviceNumToBtn[deviceID];
                btnController.OnClick -= OnSelectProp;
                btnController.enabled = false;
                deviceNumToBtn.Remove(deviceID);

            }

        }

    }


    public void SetPropSelection(int gameID)
    {
        PropSelection.SetActive(true);
        deviceNumToBtn = new Dictionary<int, TrainButtonVisualController>();
        Transform canvas = PropSelection.transform.Find("Canvas");
        Debug.Log($"{canvas.gameObject.name}");
        Debug.Log($"{canvas.transform.Find("Right Text").gameObject}");


        GameObject mySelectionObj = PropSelection.transform.Find("Canvas/My Selection").gameObject;
        TMP_Text mySelectionText = mySelectionObj.GetComponent<TMP_Text>();
        mySelectionText.text = "";
        GameObject otherSelectionObj = PropSelection.transform.Find("Canvas/Other Selection").gameObject;
        TMP_Text otherSelectionText = otherSelectionObj.GetComponent<TMP_Text>();
        otherSelectionText.text = "";

        GameObject leftTextObj = PropSelection.transform.Find("Canvas/Left Text").gameObject;
        TMP_Text leftText = leftTextObj.GetComponent<TMP_Text>();
        GameObject rightTextObj = PropSelection.transform.Find("Canvas/Right Text").gameObject;
        TMP_Text rightText = rightTextObj.GetComponent<TMP_Text>();
        GameObject levelTextObj = PropSelection.transform.Find("Canvas/Level Text").gameObject;
        TMP_Text levelText = levelTextObj.GetComponent<TMP_Text>();

        GameObject leftButton = PropSelection.transform.Find("Left Button/ButtonView").gameObject;
        GameObject rightButton = PropSelection.transform.Find("Right Button/ButtonView").gameObject;
        TrainButtonVisualController leftBtnController = leftButton.GetComponent<TrainButtonVisualController>();
        TrainButtonVisualController rightBtnController = rightButton.GetComponent<TrainButtonVisualController>();
        leftBtnController.OnClick += OnSelectProp;
        leftBtnController.enabled = true;

        rightBtnController.OnClick += OnSelectProp;
        rightBtnController.enabled = true;


        if (gameID == 4)
        {
            PropSelection.transform.Find("Props/Shield").gameObject.SetActive(true);
            PropSelection.transform.Find("Props/Gun").gameObject.SetActive(true);
            leftText.text = "Shield";
            rightText.text = "Gun";
            levelText.text = "Planet";
            leftBtnController.DeviceNum = (int)DeviceNum.Shield;
            rightBtnController.DeviceNum = (int)DeviceNum.Gun;
            deviceNumToBtn.Add((int)DeviceNum.Shield, leftBtnController);
            deviceNumToBtn.Add((int)DeviceNum.Gun, rightBtnController);


        }
        else if(gameID == 3)
        {
            PropSelection.transform.Find("Props/Shield").gameObject.SetActive(true);
            PropSelection.transform.Find("Props/Sword").gameObject.SetActive(true);
            leftText.text = "Shield";
            rightText.text = "Sword";
            levelText.text = "Arena";
            leftBtnController.DeviceNum = (int)DeviceNum.Shield;
            rightBtnController.DeviceNum = (int)DeviceNum.Shifty;
            deviceNumToBtn.Add((int)DeviceNum.Shield, leftBtnController);
            deviceNumToBtn.Add((int)DeviceNum.Shifty, rightBtnController);

        }


    }

    public void OnGameStateChange(int id)
    {
        GameState newGameState = (GameState)id;
        if (newGameState == GameState.CALIBRATION)
        {
            Initialize();
        }
        else if (newGameState == GameState.ARENA)
        {
            // enable Selection Prop
            SetPropSelection(id);
        }
        else if (newGameState == GameState.PLANET)
        {
            // enable Selection Prop
            SetPropSelection(id);
            
        }
        else if (newGameState == GameState.COOPERATE)
        {
            // enable Selection Prop
            SetPropSelection(id);
        }


        CurrentGameState = newGameState;
        

    }
    public void OnStageStateChange(int id)
    {
        StageState newStageState = (StageState)id;
        if (newStageState == StageState.WAITING_DEVICE_READY)
        {
            StartWalking = true;
            CurrentStageState = newStageState;
        }
        else if (newStageState == StageState.STAGE_START)
        { 
            ClientSend.NotifyStageStateChange(4);
        }
        else if (newStageState == StageState.STAGE_END)
        {
            ClientSend.NotifyStageStateChange(5);
            CurrentStageState = StageState.BRANCH;
        }
        CurrentStageState = newStageState;
    }


    public void OnDeviceReady()
    {
        ConfirmGrabBoard.SetActive(true);
        // set coressponding prop active
        if (myChoiceDeviceID == (int)DeviceNum.Shield)
        {
            CurrentProp = propManager.Shield;
            CurrentPropCartridge = propManager.ShieldCartridge;
            CurrentOtherProp = propManager.Sword;
            propManager.HaveSetShieldCartridge = false;
        }

        else if (myChoiceDeviceID == (int)DeviceNum.Shifty)
        {
            CurrentProp = propManager.Sword;
            CurrentPropCartridge = propManager.ShiftyCartridge;
            CurrentOtherProp = propManager.Shield;
            propManager.HaveSetShiftyCartridge = false;
        }

        CurrentProp.SetActive(true);
        CurrentPropCartridge.SetActive(true);
        DeviceReady = true;
        GrabObjectRoom.SetActive(true);
        // set tracker's corresponding object ready
    }



    public void Initialize()
    {
        OtherEnterGame = false;
        myChoiceDeviceID = -1;
        StartWalking = false;
        DeviceReady = false;
        SelectionRoom.SetActive(true);
        GrabObjectRoom.SetActive(false);
        Portal.SetActive(false);
        propManager.HaveSetHC_Origin = false;
        User.instance.HaveSetCamera = false;
        Portal.transform.localRotation = Quaternion.Euler(0, 0, 0);
        OnInitialize?.Invoke();



    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        HCRoot = GameObject.Find("HC Root");

        if (UserID == 2)
        {
            Scene = HCRoot.transform.Find("Scene/User2Scene").gameObject;

        }
        else
        {
            Scene = HCRoot.transform.Find("Scene/User3Scene").gameObject;
        }
        Scene.SetActive(true);
        SelectionRoom = Scene.transform.Find("Selection Room").gameObject;
        GrabObjectRoom = Scene.transform.Find("GrabObject Room").gameObject; // set active when device ready
        ConfirmGrabBoard = GrabObjectRoom.transform.Find("Confirm Grab Board").gameObject;
        ConfirmReturnBoard = GrabObjectRoom.transform.Find("Confirm Return Board").gameObject;

        TrainButtonVisualController btnGrab = ConfirmGrabBoard.transform.Find("Confirm Grab Button/ButtonView").gameObject.GetComponent<TrainButtonVisualController>();
        btnGrab.OnClick += OnGrabProp;

        TrainButtonVisualController btnReturn = ConfirmReturnBoard.transform.Find("Confirm Return Button/ButtonView").gameObject.GetComponent<TrainButtonVisualController>();
        btnReturn.OnClick += OnReturnProp;


        Portal = Scene.transform.Find("Portal").gameObject;
        ArenaPortal = Scene.transform.Find("ArenaPortal");
        PortalCollider = Portal.transform.Find("Back Door").gameObject.GetComponent<Collider>();

        PropSelection = SelectionRoom.transform.Find("Prop Selection").gameObject;


        // Assign each level's scene
        Arena = Scene.transform.Find("Arena").gameObject;
        BeatSaber = Scene.transform.Find("Beat Saber").gameObject;
        Puzzle = Scene.transform.Find("Puzzle").gameObject;
        // TODO: Add the following after drag them into the scene.
        //Defender = Scene.transform.Find("Defender").gameObject;
        //Shooter = Scene.transform.Find("Shooter").gameObject;


        Transform allSceneTransform = HCRoot.transform.Find("Scene");
        allSceneTransform.SetParent(RotationOffset, false);

    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

    }

    // Update is called once per frame
    void Update()
    {
        // testing
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    OnGameStateChange(4);
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    OnDeviceReady();
        //}

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    OnStageStateChange(3);
        //}

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    OnOtherSelect((int)DeviceNum.Shield);
        //}
    }
}
