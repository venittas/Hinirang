using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject FadeInCanvas;
    public GameObject FadeOutCanvas;
    public event EventHandler IntroHelper1Finished;

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
        Player.Instance.SetAnimationBools(true, false, false, false);
        Player.Instance.SetAnimationBools(false, false, false, false);
        Debug.Log("Helper 2");
        MangJuan.Instance.Scene3();
        IsNewGame = false;
    }

    public void UpdateQuestUI()
    {
        QuestSystem.Instance.UpdateQuestUI();
    }















    }
