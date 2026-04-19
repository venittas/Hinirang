using System;
using System.Collections;
using Unity.Cinemachine;
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
    public Tiyanak tiyanakPrefab;
    public GameObject stickPrefab;
    public GameObject WhipPrefab;

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
        DontDestroyOnLoad(gameObject);
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

    public void GiveStick()
    {
        GameObject stick = Instantiate(stickPrefab, new Vector2(-21.59f, -17.05f), Quaternion.identity);
        SampleWeapon weapon = stick.GetComponent<SampleWeapon>();
        InventorySystem.Instance.AddItem(weapon, 1);
    }

    public void Day1Tiyanak()
    {
        Instantiate(tiyanakPrefab, new Vector2(32.8f, -7.7f), Quaternion.identity);
    }

    public void StartDay3()
    {
        Player.Instance.currentState = Player.PlayerState.Interacting;
        GameManager.Instance.TeleportPlayer(13.56f, 3.94f);
        Player.Instance.ResetPlayer();
        GameManager.Instance.MoveDialogueToDay3();
        Player.Instance.eventNameTrigger = "Day3";
        GameManager.Instance.Day3Intro();
    }

    public void Day3Intro()
    {
        Player.Instance.currentState = Player.PlayerState.Interacting;
        StartCoroutine(Day3IntroHelper());
    }
    public IEnumerator Day3IntroHelper()
    {
        yield return new WaitForSeconds(4f);
        Narrator.Instance.Interact("StartDay3");
    }

    public void GiveWhip()
    {
        GameObject whip = Instantiate(WhipPrefab, new Vector2(-21.59f, -17.05f), Quaternion.identity);
        SampleWhip weapon = whip.GetComponent<SampleWhip>();
        InventorySystem.Instance.AddItem(weapon, 1);
    }

    public void Day3Tiyanak()
    {
        Instantiate(tiyanakPrefab, new Vector2(26.1f, 2.6f), Quaternion.identity);
        Instantiate(tiyanakPrefab, new Vector2(30f, 2.9f), Quaternion.identity);
        Instantiate(tiyanakPrefab, new Vector2(29.1f, 7.2f), Quaternion.identity);
        Instantiate(tiyanakPrefab, new Vector2(29f, 15.6f), Quaternion.identity);
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
        Player.Instance.ResetPlayer();
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

    public void TransitionToScene(int sceneIndex, float x, float y)
    {
        Debug.Log($"Transitioning to scene {sceneIndex} with player position ({x}, {y})");
        Player.Instance.currentState = Player.PlayerState.Interacting;
        StartCoroutine(TransitionRoutine(sceneIndex, x, y));
    }

    private IEnumerator TransitionRoutine(int sceneIndex, float x, float y)
    {
        tempFadeIn = Instantiate(FadeInCanvas);
        yield return new WaitForSeconds(1.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        yield return null;
        CinemachineCamera vcam = FindFirstObjectByType<CinemachineCamera>();
        if (vcam != null)
        {
            vcam.Follow = Player.Instance.transform;
        }
        Player.Instance.transform.position = new Vector2(x, y);
        Player.Instance.currentState = Player.PlayerState.Interacting;
        Destroy(tempFadeIn);
        tempFadeOut = Instantiate(FadeOutCanvas);
        yield return new WaitForSeconds(1f);
        Destroy(tempFadeOut);
        Player.Instance.currentState = Player.PlayerState.Moving;
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
