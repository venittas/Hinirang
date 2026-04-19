using UnityEngine;

public class TeleportPoint : Interactable
{
    public CircleCollider2D teleportArea;
    public SceneSystem.SceneIndex targetScene;
    public float x;
    public float y;
    public override void Interact(string teleportLocation)
    {
        SceneSystem.LoadScene((int)targetScene, x, y);
    }
}
