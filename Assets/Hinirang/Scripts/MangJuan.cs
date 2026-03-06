using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using static Player;

public class MangJuan : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float moveSpeed = 5f;
    public enum MangJuanDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(Scene3());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Scene3()
    {
        MoveWithAnimation(MangJuanDirection.Down);
        yield return new WaitForSeconds(2f);
        MoveWithAnimation(MangJuanDirection.Left);
        yield return new WaitForSeconds(2f);
        StopMovement();
    }

    private void DirectionHelper(MangJuanDirection direction, bool hasAnimation)
    {
        if(direction == MangJuanDirection.Left)
        {
            rb.linearVelocity = new Vector2(-moveSpeed * 1f, 0);
            if (hasAnimation)
            {
                SetAnimationBools(false, false, true, false);
            }
        }
        else if(direction == MangJuanDirection.Right)
        {
            rb.linearVelocity = new Vector2(moveSpeed * 1f, 0);
            if (hasAnimation)
            {
                SetAnimationBools(false, false, false, true);
            }
        }
        else if(direction == MangJuanDirection.Up)
        {
            rb.linearVelocity = new Vector2(0, moveSpeed * 1f);
            if (hasAnimation)
            {
                SetAnimationBools(false, true, false, false);
            }
        }
        else if(direction == MangJuanDirection.Down)
        {
            rb.linearVelocity = new Vector2(0, -moveSpeed * 1f);
            if (hasAnimation)
            {
                SetAnimationBools(true, false, false, false);
            }
        }
    }

    void SetAnimationBools(bool down, bool up, bool left, bool right)
    {
        animator.SetBool("isDown", down);
        animator.SetBool("isUp", up);
        animator.SetBool("isLeft", left);
        animator.SetBool("isRight", right);
    }

    public void MoveWithAnimation(MangJuanDirection direction)
    {
        DirectionHelper(direction, true);
    }

    public void MoveWithoutAnimation(MangJuanDirection direction)
    {
        DirectionHelper(direction, false);
    }

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        SetAnimationBools(false, false, false, false);
    }
}
