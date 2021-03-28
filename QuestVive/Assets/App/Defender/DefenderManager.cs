using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DefenderManager : MonoBehaviour
{
    // Common across apps
    public Transform RotationalOffset;
    public Transform SceneTransform;
    public List<Transform> SceneOffsets; // [User2SceneOFfset, User3SceneOFfset] 
    public Transform EnvironmentTransform; // Actual scene seen by the user
    public List<GameObject> UserScenes; // [User2Scene, User3Scene] 
    public event Action<int> OnInitialize;
    public event Action OnUserEnterTarget;
    public event Action OnOtherPlayerShoot;
    public bool HaveInitialized = false;

    // Specific for this app
    //Material GlowMat;
    //public float GlowingSpeed;
    //float GlowIntensity = 0;
    public Transform OtherPlayerTransform;
    

    public List<GameObject> TargetPositions;
    int currentActivePosition = 0;

    public static DefenderManager instance;


    public void OnEnterTargetPosition()
    {

        // Notify the other player to spawn an enemy

        // Start Indicator and glowing star
        OnUserEnterTarget.Invoke();

    }


    // On Other Player Shoot --> stop check direction --> animate explosion --> NextSpawnPoint
    public void NextSpawningPoint()
    {
        // Hook the following actions
        // Close Indicator
        // Animate
        OnOtherPlayerShoot.Invoke();

        TargetPositions[currentActivePosition].SetActive(false);
        currentActivePosition++;
        TargetPositions[currentActivePosition].SetActive(true);

    }


    public void InitializeScene(int userID)
    {
        //SceneTransform.SetParent(RotationalOffset, false); // Uncomment this when playing
        EnvironmentTransform.SetParent(SceneOffsets[userID - 2], false);
        UserScenes[userID - 2].SetActive(true);
        TargetPositions[currentActivePosition].SetActive(true);
        OnInitialize(userID);
        HaveInitialized = true;
    }

    //IEnumerator CircleGlowing ()
    //{
    //    while (true)
    //    {
            
    //        yield return null;
    //    }
    //}

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


        // Testing
        if (Input.GetKeyDown(KeyCode.S))
        {
            InitializeScene(2);

        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextSpawningPoint();
        }

    }
}
