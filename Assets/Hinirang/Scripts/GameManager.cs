using System;
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
        Narrator.Instance.Interact("");
        Debug.Log("Playing intro...");
        Player.Instance.SetAnimationBools(false, false, false, true);
        Invoke("IntroHelper", 0.01f);

    }

    public void IntroHelper()
    {
        Player.Instance.SetAnimationBools(false, false, false, false);
        MangJuan.Instance.MoveOne();
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

    public void MoveDialogueToDay2()
    {
        AlingNena.Instance.MoveDialogue();
        MangEnko.Instance.MoveDialogue();
        Joba.Instance.MoveDialogue();
    }

    public void TeleportPlayer(float x, float y)
    {
        teleportPoint = new Vector2(x, y);
        tempFadeIn = Instantiate(FadeInCanvas);
        Invoke("TeleportPlayerHelper", 1f);
        tempFadeOut = Instantiate(FadeOutCanvas);
    }

    private void TeleportPlayerHelper()
    {
        Destroy(tempFadeIn);
        Player.Instance.transform.position = teleportPoint;
        Invoke("TeleportPlayerHelper2", 1f);
    }

    private void TeleportPlayerHelper2()
    {
        Destroy(tempFadeOut);
        Player.Instance.currentState = Player.PlayerState.Moving;
    }
}
