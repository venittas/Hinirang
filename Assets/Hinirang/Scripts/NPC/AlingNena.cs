using Unity.VisualScripting;
using UnityEngine;

public class AlingNena : NPCScript
{
    public static AlingNena Instance;
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

        if (Player.Instance.eventNameTrigger == "AlingNena1Quest1")
        {
            Player.Instance.eventNameTrigger = "Albularyo1Quest1";
            newEventName = "Albularyo1Quest1";
        }
    }
}
