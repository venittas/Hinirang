using UnityEngine;

public class MangEnko : NPCScript
{
    public static MangEnko Instance;
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

        if (Player.Instance.eventNameTrigger == "Albularyo1Quest1")
        {
            Player.Instance.eventNameTrigger = "GiveStick";
            Debug.Log("TANGINA MO BINAGO KO NA: " + Player.Instance.eventNameTrigger);
        }
    }
}
