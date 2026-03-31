using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    public enum PlayerState
    {
        Idle,
        Moving,
        Interacting
    }

    public static Player Instance;

    public PlayerState currentState = PlayerState.Moving;

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float interactionRange = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    private Vector2 lastLookDirection = Vector2.down;

    Interactable currentInteractable = null;

    public Vector2 GetLastLookDirection()
    {
        return lastLookDirection;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float haxis = Input.GetAxisRaw("Horizontal");
        float vaxis = Input.GetAxisRaw("Vertical");
        if (currentState == PlayerState.Moving)
        {
            rb.linearVelocity = new Vector2(haxis * moveSpeed, vaxis * moveSpeed);
            DirectionAnimation(haxis, vaxis);
            DetectInteraction();
        }


        
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null && currentState != PlayerState.Interacting)
        {
            currentInteractable.Interact();
            currentState = PlayerState.Interacting;
        }
    }

    private void DirectionAnimation(float haxis, float vaxis)
    {
        if (haxis != 0 || vaxis != 0)
        {
            lastLookDirection = new Vector2(haxis, vaxis).normalized;
        }

        if (vaxis > 0)
        {
            SetAnimationBools(false, true, false, false);
            lastLookDirection = Vector2.up;
        }
        else if (vaxis < 0)
        {
            SetAnimationBools(true, false, false, false);
            lastLookDirection = Vector2.down;
        }
        else if (haxis > 0)
        {
            SetAnimationBools(false, false, false, true);
            lastLookDirection = Vector2.right;
        }
        else if (haxis < 0)
        {
            SetAnimationBools(false, false, true, false);
            lastLookDirection = Vector2.left;
        }
        else
        {
            SetAnimationBools(false, false, false, false);
        }
    }

    void SetAnimationBools(bool down, bool up, bool left, bool right)
    {
        if (currentState == PlayerState.Moving)
        {
            animator.SetBool("isDown", down);
            animator.SetBool("isUp", up);
            animator.SetBool("isLeft", left);
            animator.SetBool("isRight", right);
        }
    }

    private void DetectInteraction()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastLookDirection, interactionRange, interactableLayer);
        Debug.DrawRay(transform.position, lastLookDirection * interactionRange, Color.red, 1f);

        Interactable newInteractable = null;

        if (hit.collider != null)
        {
            newInteractable = hit.collider.GetComponent<Interactable>();

        }
        if (newInteractable != currentInteractable)
        {
            if (currentInteractable != null)
            {
                currentInteractable.HideIndicator();
            }
            if (newInteractable != null)
            {
                newInteractable.ShowIndicator();
            }
            currentInteractable = newInteractable;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + (Vector3)(lastLookDirection * interactionRange)
        );

    }
}