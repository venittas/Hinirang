using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 spawnPoint;
    public string eventNameTrigger = string.Empty;
    public GameObject attackCollider;

    public enum PlayerState
    {
        Idle,
        Moving,
        Interacting
    }

    public static Player Instance;

    public PlayerState currentState = PlayerState.Moving;
    private Vector2 input;
    private Vector2 touchInput;

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float interactionRange = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    private Vector2 lastLookDirection = Vector2.down;

    private float Health = 100f;
    private bool isInvulnerable = false;

    Interactable currentInteractable = null;
    private float invulnerabilityDuration = 1f;
    private float flashInterval = 0.1f;

    public bool IsAttacking = false;
    public float attackDuration = 0.5f;

    public RuntimeAnimatorController defaultController;
    public RuntimeAnimatorController stickController;

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
        animator.runtimeAnimatorController = defaultController;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPoint = transform.position;
        attackCollider.SetActive(false);
        DontDestroyOnLoad(gameObject); 
    }

    void Update()
    {
        float haxis = Input.GetAxisRaw("Horizontal");
        float vaxis = Input.GetAxisRaw("Vertical");
        if (currentState == PlayerState.Moving && IsAttacking == false)
        {
            Vector2 currentInput = touchInput;

            if (currentInput == Vector2.zero)
            {
                if (haxis != 0)
                {
                    currentInput = new Vector2(haxis, 0);
                }

                if (vaxis != 0)
                {
                    currentInput = new Vector2(0, vaxis);
                }

                if (haxis == 0 && vaxis == 0)
                {
                    currentInput = Vector2.zero;
                }
            }

            rb.linearVelocity = currentInput * moveSpeed;
            DirectionAnimation(currentInput.x, currentInput.y);
            DetectInteraction();
        }



        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed. State: " + currentState + " | IsWeapon: " + InventorySystem.Instance.IsItemEquippedWeapon());
            if (InventorySystem.Instance.IsItemEquippedWeapon())
            {
                rb.linearVelocity = Vector2.zero;
                StopMove();
                StartCoroutine(AttackRoutine());
            }
        }

    }

    IEnumerator AttackRoutine()
    {
        Debug.Log("Attacking");
        IsAttacking = true;
        rb.linearVelocity = Vector2.zero; // guarantee stop regardless of Update order
        SetAnimationBools(false, false, false, false, false);
        lastLookDirection = GetLastLookDirection();
        switch (lastLookDirection)
        {
            case Vector2 v when v == Vector2.down:
                animator.SetTrigger("AttackDown");
                break;
            case Vector2 v when v == Vector2.up:
                animator.SetTrigger("AttackUp");
                break;
            case Vector2 v when v == Vector2.left:
                animator.SetTrigger("AttackLeft");
                break;
            case Vector2 v when v == Vector2.right:
                animator.SetTrigger("AttackRight");
                break;
            default:
                animator.SetTrigger("TestTrigger");
                break;
        }
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        IsAttacking = false;
        attackCollider.SetActive(false);
    }

    public float GetWeaponDamage()
    {
        if (InventorySystem.Instance.GetEquippedItem().isWeapon)
        {
            return InventorySystem.Instance.GetEquippedItem().damage;
        }else return 0f;
    }


    public void Interact()
    {
        if (currentInteractable != null && currentState != PlayerState.Interacting)
        {
            currentInteractable.Interact(eventNameTrigger);
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

    public void SetAnimationBools(bool down, bool up, bool left, bool right)
    {
        if (currentState == PlayerState.Moving)
        {
            animator.SetBool("isDown", down);
            animator.SetBool("isUp", up);
            animator.SetBool("isLeft", left);
            animator.SetBool("isRight", right);
        }
    }
    public void SetAnimationBools(bool down, bool up, bool left, bool right, bool keme)
    {
        animator.SetBool("isDown", down);
        animator.SetBool("isUp", up);
        animator.SetBool("isLeft", left);
        animator.SetBool("isRight", right);
    }

    private void DetectInteraction()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastLookDirection, interactionRange, interactableLayer);
        Debug.DrawRay(transform.position, lastLookDirection * interactionRange, Color.red, 1f);

        Interactable newInteractable = null;

        if (hit.collider != null)
        {
            //Debug.Log("Hit: " + hit.collider.name);
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

    public void EquipItem(InventoryItem item)
    {
        Debug.Log("Equipped item");
        item.isPickedUp = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + (Vector3)(lastLookDirection * interactionRange)
        );

    }

    public void MoveUp() => touchInput = new Vector2(0, 1);
    public void MoveDown() => touchInput = new Vector2(0, -1);
    public void MoveRight() => touchInput = new Vector2(1, 0);
    public void MoveLeft() => touchInput = new Vector2(-1, 0);
    public void StopMove() => touchInput = Vector2.zero;

    public void TakeDamage(float damage)
    {
        if (isInvulnerable)
        {
            return;
        }
        else 
        { 
            Health -= damage;
            StartCoroutine(BecomeInvulnerable());
            if (Health <= 0)
            {
                gameObject.SetActive(false);
                GameManager.Instance.ShowDeathUI();
            }
        }
    }

    private IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true; 
        float elapsed = 0;

        while (elapsed < invulnerabilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Visual effect
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    public void Respawn()
    {
        transform.position = spawnPoint;
        Health = 100f;
        isInvulnerable = false;
        spriteRenderer.enabled = true;
        gameObject.SetActive(true);
        currentState = PlayerState.Moving;
    }
}