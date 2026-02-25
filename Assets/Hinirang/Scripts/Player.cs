using UnityEngine;

public class Capsule : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private float moveSpeed = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        float haxis = Input.GetAxisRaw("Horizontal");
        float vaxis = Input.GetAxisRaw("Vertical");
        rb.linearVelocity = new Vector2(haxis * moveSpeed, vaxis * moveSpeed);
        animator.SetBool("isDown", vaxis < 0);
        animator.SetBool("isUp", vaxis > 0);
        animator.SetBool("isLeft", haxis < 0);
        animator.SetBool("isRight", haxis > 0);
    }


}
