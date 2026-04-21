using UnityEngine;

public class Joba : NPCScript
{
    public static Joba Instance;
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

    public override void CheckEventTriggerName(string eventName)
    {
        string newEventName = eventName;

        if (Player.Instance.eventNameTrigger == "Day3OldMan")
        {
            Player.Instance.eventNameTrigger = "GiveWhip"; // consumed after talking to Joba
        }

        if (Player.Instance.eventNameTrigger == "Day3")
        {
            Player.Instance.eventNameTrigger = "EndOfDay3";
        }
    }

    private void Update()
    {
        if (SceneSystem.Instance.currentPlayerLocation != SceneSystem.SceneIndex.Island)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
