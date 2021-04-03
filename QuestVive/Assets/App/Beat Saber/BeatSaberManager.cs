using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OculusSampleFramework;

public class BeatSaberManager : MonoBehaviour
{
    public static BeatSaberManager instance;

    public float WaitBeforeGame = 15;
    public TrainButtonVisualController ButtonController;
    public List<GameObject> CubePrefabs; // left, right, down, up
    public List<Transform> SpawningPoints; // left, right
    public List<Vector2Int> SpawningSequence;
    public float Interval = 2f;

    public float MaxSpeed = 12;
    public float MinSpeed = 6;
    public int ScoreUnit = 20;

    public int Score;
    int DestroyCount;

    public TMP_Text ScoreText;
    public TMP_Text FinishText;
    public GameObject FinishPanel;

    void GenerateCube(int isRight, int direction)
    {
        GameObject Cube = Instantiate(CubePrefabs[direction]);
        Cube.transform.SetParent(SpawningPoints[isRight], false);
        CubeBehavior cubeBehavior = Cube.GetComponent<CubeBehavior>();
        cubeBehavior.isLeft = (isRight == 0);
        cubeBehavior.MoveSpeed = Random.Range(MinSpeed, MaxSpeed);
        cubeBehavior.Initialize();
    
    }

    public IEnumerator GameStart()
    {
        yield return new WaitForSeconds(WaitBeforeGame);

        ButtonController.enabled = true;
        ButtonController.OnClick += OnClickFinishButton;
        DestroyCount = SpawningSequence.Count;
        Score = 0;
        ScoreText.text = "0";
        for (int i = 0; i < SpawningSequence.Count; ++i)
        {
            GenerateCube(SpawningSequence[i].x, SpawningSequence[i].y);
            yield return new WaitForSeconds(Interval);
        
        }
    }

    public void OnDestroyCube()
    {
        DestroyCount -= 1;
        if (DestroyCount == 0)
        {
            FinishGame();
        }
    }

    void FinishGame()
    {
        FinishText.gameObject.SetActive(true);
        FinishText.text = "Game Finished!\nPress the button to confirm";
        FinishPanel.SetActive(true);
        GeneralManager.instance.OnGameEnd();

    }

    void OnClickFinishButton(TrainButtonVisualController btn, int dummy)
    {
        btn.OnClick -= OnClickFinishButton;
        btn.enabled = false;
        FinishText.text = "Return to portal";
    }

    public void OnHitCube()
    {
        Score += ScoreUnit;
        ScoreText.text = Score.ToString();
    }


    void Initialize() 
    {
        FinishText.gameObject.SetActive(false);
        //FinishPanel.SetActive(false);

        StartCoroutine(GameStart());
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
        FinishText.gameObject.SetActive(true);

        FinishText.text = "Wait for other player to join";
        FinishPanel.SetActive(false);

        if (GeneralManager.instance.OtherEnterGame)
        {
            Initialize();
        }
        else
        {
            GeneralManager.instance.OnOtherEnterGame += Initialize;


        }

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
