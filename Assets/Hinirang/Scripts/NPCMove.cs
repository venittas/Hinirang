using UnityEngine;

[System.Serializable]
public class NPCMove
{
    public float moveTime = 0.1f;
    public NPCScript.Direction direction = NPCScript.Direction.Down;

    public NPCMove(NPCScript.Direction direction, float moveTime)
    {
        this.direction = direction;
        this.moveTime = moveTime;
    }
}
