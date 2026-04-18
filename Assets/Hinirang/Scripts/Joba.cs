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
    }

    public override void CheckEventTriggerName(string eventName)
    {
        string newEventName = eventName;

        if (Player.Instance.eventNameTrigger == "Day3")
        {
            Player.Instance.eventNameTrigger = "EndOfDay3";
        }
    }
}
