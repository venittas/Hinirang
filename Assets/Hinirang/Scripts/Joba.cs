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
}
