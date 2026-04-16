using UnityEngine;

public class TeleportPoint : Interactable
{
    public CircleCollider2D teleportArea;
    public SceneSystem.SceneIndex targetScene;
    public override void Interact(string teleportLocation)
    {
        SceneSystem.LoadScene((int)targetScene);
    }
}
