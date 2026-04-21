using UnityEngine;

public class Narrator : NPCScript
{
    public static Narrator Instance;

    public override void CheckEventTriggerName(string eventName)
    {

    }

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
        base.Start();
    }
}
