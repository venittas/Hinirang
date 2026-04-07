using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject FadeInCanvas;
    public GameObject FadeOutCanvas;

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
        Narrator.Instance.Interact();
        IsNewGame = false;
    }

    public void UpdateQuestUI()
    {
        QuestSystem.Instance.UpdateQuestUI();
    }















    }
