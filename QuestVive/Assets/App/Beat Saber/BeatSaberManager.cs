using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OculusSampleFramework;

public class BeatSaberManager : MonoBehaviour
{
    public static BeatSaberManager instance;

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
        ButtonController.enabled = true;
        ButtonController.OnClick += OnClickFinishButton;
        FinishText.text = "Game Finished!\nPress to confirm";
        DestroyCount = SpawningSequence.Count;
        Score = 0;
        ScoreText.text = "0";
        FinishPanel.SetActive(false);
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
        FinishPanel.SetActive(true);
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
        StartCoroutine(GameStart());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
