using UnityEngine;

public class Narrator : NPCScript
{
    public static Narrator Instance;
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
