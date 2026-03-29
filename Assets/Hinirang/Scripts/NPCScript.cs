using System.Collections;
using UnityEngine;

public class NPCScript : Interactable
{
    [SerializeField] private DialogueLine[] dialogueLines;
    [SerializeField] private NPCMove[] moves;
    [SerializeField] private float moveSpeed = 5f;
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down, 
        Wait
    }
    void Start()
    {
        base.Start();
        
    }
    

    public override void Interact()
    {
        if(dialogueLines != null && dialogueLines.Length > 0)
        {
            DialogueSystem.Instance.StartDialogue(dialogueLines);
        }
    }

    private void DirectionHelper(Direction direction, bool hasAnimation)
    {
        if (direction == Direction.Left)
        {
            rb.linearVelocity = new Vector2(-moveSpeed * 1f, 0);
            if (hasAnimation)
            {
                SetAnimationBools(false, false, true, false);
            }
        }
        else if (direction == Direction.Right)
        {
            rb.linearVelocity = new Vector2(moveSpeed * 1f, 0);
            if (hasAnimation)
            {
                SetAnimationBools(false, false, false, true);
            }
        }
        else if (direction == Direction.Up)
        {
            rb.linearVelocity = new Vector2(0, moveSpeed * 1f);
            if (hasAnimation)
            {
                SetAnimationBools(false, true, false, false);
            }
        }
        else if (direction == Direction.Down)
        {
            rb.linearVelocity = new Vector2(0, -moveSpeed * 1f);
            if (hasAnimation)
            {
                SetAnimationBools(true, false, false, false);
            }
        }else if (direction == Direction.Wait)
        {
            SetAnimationBools(false, false, false, false);
        }
    }

    void SetAnimationBools(bool down, bool up, bool left, bool right)
    {
        animator.SetBool("isDown", down);
        animator.SetBool("isUp", up);
        animator.SetBool("isLeft", left);
        animator.SetBool("isRight", right);
    }

    public void MoveWithAnimation(Direction direction)
    {
        DirectionHelper(direction, true);
    }

    public void MoveWithoutAnimation(Direction direction)
    {
        DirectionHelper(direction, false);
    }

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        SetAnimationBools(false, false, false, false);
    }

    public IEnumerator Move(int index)
    {
        MoveWithAnimation(moves[index].direction);
        Debug.Log("Moving " + moves[index].direction + " for " + moves[index].moveTime + " seconds.");
        yield return new WaitForSeconds(moves[index].moveTime);
        StopMovement();
    }
    public IEnumerator MoveNoAnim(int index)
    {
        MoveWithoutAnimation(moves[index].direction);
        Debug.Log("Moving " + moves[index].direction + " for " + moves[index].moveTime + " seconds.");
        yield return new WaitForSeconds(moves[index].moveTime);
        StopMovement();
    }

    public void Test()
    {
        Debug.Log("RB: " + rb);
        Debug.Log("Animator: " + animator);
    }
}
