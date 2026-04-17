using Unity.VisualScripting;
using UnityEngine;

public class AlingNena : NPCScript
{
    public override void CheckEventTriggerName(string eventName)
    {
        string newEventName = eventName;

        if (Player.Instance.eventNameTrigger == "AlingNena1Quest1")
        {
            Player.Instance.eventNameTrigger = "Albularyo1Quest1";
            newEventName = "Albularyo1Quest1"; // Gamitin itong event name na ito para sa dialogue system
            Debug.Log("TANGINA MO BINAGO KO NA: " + Player.Instance.eventNameTrigger);
        }
    }
}
