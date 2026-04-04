using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
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
        Debug.Log("GameManager started. IsNewGame: " + IsNewGame);
        if (IsNewGame)
        {
            PlayIntro();
        }
    }

    private void PlayIntro()
    {
        Debug.Log("Playing intro...");
        Narrator.Instance.Interact();
        IsNewGame = false;
        Invoke("UpdateQuestUI", 2f); // Delay to ensure dialogue starts before updating quest UI)
    }

    private void UpdateQuestUI()
    {
        QuestSystem.Instance.UpdateQuestUI();
    }















    }
