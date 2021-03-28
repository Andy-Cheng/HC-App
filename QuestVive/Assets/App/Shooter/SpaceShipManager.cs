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
    public Transform PlayerTransform;
    public GameObject Enemy;
    public float InitialEnemyDistance = 40f;
    public EnemyBehavior activeEnemy;
    public List<GameObject> DisplayText;
    public List<GameObject> DangerousZones;
    public List<int> EnemyDirections;
    public List<int> EnemyDirections2;

    public int CountDownDuration = 10;

    //public List<GameObject> TargetPositions;
    int currentActivePosition = 0;
    public Transform RotationalOffset;
    public Transform SceneTransform;

    public Transform User2SceneOffset;
    public Transform User3SceneOffset;
    public Transform SpaceShipTransform;

    public List<Transform> LandMarkCentrals;
    public GameObject Landmark;
    

    // fire (keep users from distance) 



    public IEnumerator CountDown(int userID)
    {
        int seq;

        if (userID == 2)
        {
            seq = EnemyDirections[0];
            EnemyDirections.RemoveAt(0);
        }
        else
        { 
            seq = EnemyDirections2[0];
            EnemyDirections2.RemoveAt(0);
        }

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
        //TargetPositions[currentActivePosition].SetActive(false);
        //currentActivePosition++;
        StartCoroutine(CountDown(GeneralManager.instance.UserID));
        Vector3 initialPosition;
        initialPosition = RoomCenter.position + Vector3.Normalize(OtherPlayerTransform.position - RoomCenter.position) * InitialEnemyDistance;
        GameObject e = Instantiate(Enemy, initialPosition, Quaternion.identity);
        activeEnemy = e.GetComponent<EnemyBehavior>();

    }

    //public void NextSpawningPoint()
    //{ 
    //    TargetPositions[currentActivePosition].SetActive(true);

    //}

    public void SetSceneByUserID(int id)
    {
        if (id == 2)
        {
            SpaceShipTransform.SetParent(User2SceneOffset, false);
        }
        else
        { 
            SpaceShipTransform.SetParent(User3SceneOffset, false);

        }
        Landmark.transform.localPosition = Vector3.zero;
        Landmark.transform.rotation = Quaternion.identity;
        Landmark.transform.SetParent(LandMarkCentrals[id-2], false);

    }


    public void InitializeScene(int userID)
    { 
        SceneTransform.SetParent(RotationalOffset, false);
        //TargetPositions[currentActivePosition].SetActive(true);
        SetSceneByUserID(3);
        foreach (GameObject text in DisplayText)
        {
            text.SetActive(false);
        }

    }

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
        //GeneralManager.instance.OnStageStart += InitializeScene;

    }



    // Update is called once per frame
    void Update()
    {
        
        


    }
}
