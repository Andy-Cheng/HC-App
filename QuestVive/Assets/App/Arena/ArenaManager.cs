using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum ArenaGameState
{ 
    BattleStart,
    BattleEnd,
}


public class ArenaManager : MonoBehaviour
{
    public static ArenaManager instance;
    public ArenaGameState GameState;
    public TMP_Text StatusText;
    public float TimeLast = 120;

    IEnumerator GameStart()
    {
        GameState = ArenaGameState.BattleStart;
        while (TimeLast > 0)
        {
            TimeLast -= Time.deltaTime;
            yield return null;
        }
        GameState = ArenaGameState.BattleEnd;
        StatusText.text = "Game Over\nGo back to portal";
        GameEnd();
    }

    IEnumerator ChangeTimeText()
    {

        StatusText.text = "Hit your enemy's light saber";
        yield return new WaitForSeconds(5);
        while (GameState == ArenaGameState.BattleStart)
        {
            StatusText.text = "Timer+\n" + Mathf.FloorToInt(TimeLast).ToString();

            yield return new WaitForSeconds(.5f);
        }
    
    }

    void GameEnd()
    {
        // set portal
        GeneralManager.instance.OnGameEnd();
    
    }

    void Initialize()
    {
        // set other and other's prop active

        GeneralManager.instance.propManager.OtherPlayer.SetActive(true);
        GeneralManager.instance.CurrentOtherProp.SetActive(true);
        StartCoroutine(GameStart());
        StartCoroutine(ChangeTimeText());
    
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
        GeneralManager.instance.CurrentOtherProp.SetActive(false);
        GeneralManager.instance.propManager.OtherPlayer.SetActive(false);

    }
}
