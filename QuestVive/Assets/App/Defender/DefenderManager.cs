using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DefenderManager : MonoBehaviour
{
    public event Action OnUserEnterTarget;
    public event Action OnOtherPlayerShoot;

    public Transform OtherPlayerTransform;
    public GameObject Indicator;
    public GameObject Explosion;
    public GameObject Asteroid;


    public List<GameObject> TargetPositions;
    int currentActivePosition = 0;

    public static DefenderManager instance;
    bool canExplode; // only allow the asteroid to explode when the user enter the new target position


    public void OnEnterTargetPosition()
    {

        // Notify the other player to spawn an enemy
        ClientSend.SendPlayerArrived();
        // Start Indicator and glowing star
        OnUserEnterTarget.Invoke();
        canExplode = true;
    }



    // On Other Player Shoot --> stop check direction --> animate explosion --> NextSpawnPoint
    public void NextSpawningPoint()
    {
        TargetPositions[currentActivePosition].SetActive(false);

        if (currentActivePosition < TargetPositions.Count - 1)
        {
            currentActivePosition++;

            TargetPositions[currentActivePosition].SetActive(true);
        }
        else
        { 
          GeneralManager.instance.OnGameEnd();
        }

    }

    public void OtherPlayerShoot()
    {
        if (canExplode)
        {
            OnOtherPlayerShoot.Invoke();
            NextSpawningPoint();
            canExplode = false;
        }
    }


    public void Initialize()
    {
        //SceneTransform.SetParent(RotationalOffset, false); // Uncomment this when playing
        TargetPositions[currentActivePosition].SetActive(true);
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        OtherPlayerTransform = GeneralManager.instance.propManager.OtherPlayer.transform;
        Asteroid.SetActive(false);
        canExplode = false;
    }

    // Start is called before the first frame update
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
        Indicator.SetActive(true);
        Explosion.SetActive(true);
        GeneralManager.instance.InGame = true;
    }

    // Update is called once per frame
    void Update()
    {

        // Testing
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Initialize();

        //}

        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    NextSpawningPoint();
        //}

    }
    private void OnDisable()
    {
        Indicator.SetActive(false);
        Explosion.SetActive(false);
        GeneralManager.instance.OnOtherEnterGame -= Initialize;
    }

}
