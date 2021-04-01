using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceShipManager : MonoBehaviour
{
    public static SpaceShipManager instance;

    public Transform RoomCenter;
    public Transform GunTracker;
    public Transform OtherPlayerTransform;
    public Transform TargetTransform;
    public Transform PlayerTransform; // assign
    public GameObject Enemy;
    public float InitialEnemyDistance = 40f;
    public EnemyBehavior activeEnemy;
    public List<GameObject> DisplayText;
    public List<GameObject> DangerousZones;
    public List<int> EnemyDirections;
    public int EnemyCount = 7;

    public int CountDownDuration = 10;

    
    public IEnumerator CountDown(int userID)
    {
        int seq;
        seq = EnemyDirections[0];
        EnemyDirections.RemoveAt(0);

        TMP_Text textComponent = DisplayText[seq].GetComponent<TMP_Text>();
        Color textColor = textComponent.color;

        DisplayText[seq].SetActive(true);

        for (int i = CountDownDuration; i > 0; --i)
        {
            textComponent.text = i.ToString();
            yield return new WaitForSeconds(1);
        }


        textComponent.text = "Shoot";
        textComponent.color = new Color32(255, 0, 0, 255); ;
        activeEnemy.canExplode = true;
        yield return new WaitForSeconds(5);
        DisplayText[seq].SetActive(false);
        textComponent.color = textColor;
    }

    // Other player hit target collider --> spawn enemy
    public void SpawnEnemy()
    {
        StartCoroutine(CountDown(GeneralManager.instance.UserID));
        Vector3 initialPosition;
        initialPosition = RoomCenter.position + Vector3.Normalize(OtherPlayerTransform.position - RoomCenter.position) * InitialEnemyDistance;
        GameObject e = Instantiate(Enemy, initialPosition, Quaternion.identity);
        activeEnemy = e.GetComponent<EnemyBehavior>();

    }

    public void OnEnemyDie()
    {
        EnemyCount--;
        if (EnemyCount < 1)
        {
            GeneralManager.instance.OnGameEnd();
        }
    }

    public void Initialize()
    { 


    }

    private void Awake()
    {
        if (instance != null)
        { 
            Destroy(instance);
        }
        instance = this;

        foreach (GameObject text in DisplayText)
        {
            text.SetActive(false);
        }
        GunTracker = GeneralManager.instance.propManager.Gun.transform;
        OtherPlayerTransform = GeneralManager.instance.propManager.OtherPlayer.transform;
        TargetTransform = GeneralManager.instance.propManager.Shield.transform;

    }

    // Start is called before the first frame update
    void Start()
    {
        //GeneralManager.instance.OnStageStart += InitializeScene;
        if (GeneralManager.instance.OtherEnterGame)
        {
            Initialize();
        }
        else
        {
            GeneralManager.instance.OnOtherEnterGame += Initialize;

        }
        GeneralManager.instance.InGame = true;
    }



    // Update is called once per frame
    void Update()
    {
    }

    private void OnDisable()
    {
        GeneralManager.instance.OnOtherEnterGame -= Initialize;
    }


}
