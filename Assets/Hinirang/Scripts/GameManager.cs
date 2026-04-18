using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject FadeInCanvas;
    public GameObject FadeOutCanvas;
    public GameObject DeathCanvas;
    public event EventHandler IntroHelper1Finished;
    GameObject tempFadeIn;
    GameObject tempFadeOut;
    Vector2 teleportPoint;


    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public bool IsNewGame = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Instantiate(FadeOutCanvas);
        Debug.Log("GameManager started. IsNewGame: " + IsNewGame);
        if (IsNewGame)
        {
            Invoke("PlayIntro", 1f);
        }
    }

    private void PlayIntro()
    {
        Narrator.Instance.Interact("NarratorDay1");
        Debug.Log("Playing intro...");
        Player.Instance.SetAnimationBools(false, false, false, false);

    }

    public void IntroHelper()
    {
        Player.Instance.SetAnimationBools(false, false, false, true);
        StartCoroutine(IntroHelperRoutine());
    }
    private IEnumerator IntroHelperRoutine()
    {
        Player.Instance.SetAnimationBools(false, false, false, true);
        yield return StartCoroutine(MangJuan.Instance.MoveOne()); 
        MangJuan.Instance.Interact("IntroHelper1");               
    }


    public void IntroHelper2()
    {
        Player.Instance.currentState = Player.PlayerState.Interacting;
        Player.Instance.SetAnimationBools(true, false, false, false);
        Player.Instance.SetAnimationBools(false, false, false, false);
        Debug.Log("Helper 2");
        MangJuan.Instance.Scene3();
        IsNewGame = false;
        Invoke("IntroFinished", 1f);
    }

    public void IntroFinished()
    {
        Player.Instance.currentState = Player.PlayerState.Moving;
    }

    public void Day3Intro()
    {
        Player.Instance.currentState = Player.PlayerState.Interacting;
        StartCoroutine(Day3IntroHelper());
    }
    public IEnumerator Day3IntroHelper()
    {
        yield return new WaitForSeconds(4f);
        Narrator.Instance.Interact("");
    }
    public void Day7Intro()
    {
        Player.Instance.currentState = Player.PlayerState.Interacting;
        StartCoroutine(Day7IntroHelper());
    }
    public IEnumerator Day7IntroHelper()
    {
        yield return new WaitForSeconds(1f);
        Narrator.Instance.Interact("Day7");
    }



    public void UpdateQuestUI()
    {
        QuestSystem.Instance.UpdateQuestUI();
    }

    public void ShowDeathUI()
    {
        if (DeathCanvas != null)
        {
            DeathCanvas.SetActive(true);
        }
    }

    public void MoveDialogueToDay3()
    {
        AlingNena.Instance.MoveDialogue();
        MangEnko.Instance.MoveDialogue();
        Joba.Instance.MoveDialogue();
        Narrator.Instance.MoveDialogue();
    }
    public void MoveDialogueToDay7()
    {
        AlingNena.Instance.MoveDialogue();
        MangEnko.Instance.MoveDialogue();
        Joba.Instance.MoveDialogue();
        Narrator.Instance.MoveDialogue();
    }

    public void TeleportPlayer(float x, float y)
    {
        teleportPoint = new Vector2(x, y);
        tempFadeIn = Instantiate(FadeInCanvas);
        Invoke("TeleportPlayerHelper", 1.5f);
    }

    private void TeleportPlayerHelper()
    {
        Player.Instance.transform.position = teleportPoint;
        Player.Instance.SetAnimationBools(true, false, false, false, false);
        Player.Instance.SetAnimationBools(false, false, false, false, false);
        Invoke("TeleportPlayerHelper2", 1f);
    }

    private void TeleportPlayerHelper2()
    {
        tempFadeOut = Instantiate(FadeOutCanvas);
        Invoke("TeleportPlayerHelper3", 0.5f);
        Destroy(tempFadeIn);
    }
    private void TeleportPlayerHelper3()
    {
        Destroy(tempFadeOut);
        Player.Instance.currentState = Player.PlayerState.Moving;
    }
}
