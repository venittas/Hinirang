using UnityEngine;

public class Narrator : NPCScript
{
    public static Narrator Instance;

    public override void CheckEventTriggerName(string eventName)
    {
        throw new System.NotImplementedException();
    }

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
        base.Start();
    }
}
