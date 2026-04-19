using System.Collections;
using Unity.VisualScripting; // Needed for Coroutines
using UnityEngine;

public class Tiyanak : Enemy
{
    public Rigidbody2D rb;
    public float jumpChaseTimer = 0;
    public float jumpChaseCooldown = 5f;

    public float roamTimer = 0;
    public float roamInterval = 2f; 
    public CircleCollider2D attackCollider;

    public Vector2 topRight;
    public Vector2 topLeft;
    public Vector2 bottomRight;
    public Vector2 bottomLeft;

    public Animator animator;

    private Vector2 lastLookDirection = Vector2.down;

    private Vector2 roamTarget;
    private bool hasRoamTarget = false;
    private Vector2 roamOrigin;

    public bool Move = true;
    public bool isKnockedBack = false;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        health = 10f;
        speed = 3.5f;
        damage = 20f;
        attackRange = 3f;
        attackCooldown = 2f;
        chaseRange = 15f;
        roamRange = 10f;
        roamOrigin = transform.position; 
        UpdateRoamCoordinates();
    }



    public void FixedUpdate()
    {
        if (!Move) return;
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
        if (jumpChaseTimer > 0) jumpChaseTimer -= Time.deltaTime;
        if (roamTimer > 0) roamTimer -= Time.deltaTime;

        if (Player.Instance.currentState != Player.PlayerState.Moving) return;

        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            if(attackTimer <= 0)
            {
                Attack();
            }
        }
        else if (distanceToPlayer <= chaseRange && jumpChaseTimer <= 0 && distanceToPlayer >= attackRange)
        {
            if (!isKnockedBack) Chase();
        }
        else if (roamTimer <= 0)
        {
            Roam();
        }

    }

    private void DirectionAnimation(float haxis, float vaxis)
    {
        if (Mathf.Abs(haxis) > Mathf.Abs(vaxis))
        {
            if (haxis > 0)
            {
                SetAnimationBools(false, false, true);
                lastLookDirection = Vector2.right;
            }
            else
            {
                SetAnimationBools(false, true, false);
                lastLookDirection = Vector2.left;
            }
        }
        else if (vaxis != 0)
        {
            SetAnimationBools(true, false, false);
            lastLookDirection = vaxis > 0 ? Vector2.up : Vector2.down;
        }
        else
        {
            SetAnimationBools(false, false, false);
        }
    }

    public void SetAnimationBools(bool upDown, bool left, bool right)
    {
        animator.SetBool("isUpDown", upDown);
        animator.SetBool("isLeft", left);
        animator.SetBool("isRight", right);
    }



    public void UpdateRoamCoordinates()
    {
        Vector2 currentPos = transform.position;

        topLeft = new Vector2(currentPos.x - roamRange, currentPos.y + roamRange);
        topRight = new Vector2(currentPos.x + roamRange, currentPos.y + roamRange);
        bottomLeft = new Vector2(currentPos.x - roamRange, currentPos.y - roamRange);
        bottomRight = new Vector2(currentPos.x + roamRange, currentPos.y - roamRange);

    }

    public override void Attack()
    {
        StartCoroutine(AttackRoutine());
    }

    public IEnumerator AttackRoutine()
    {

        attackTimer = attackCooldown;
        rb.linearVelocity = Vector2.zero; 
        SetAnimationBools(false, false, false); 

        switch (lastLookDirection)
        {
            case Vector2 v when v == Vector2.down:
                animator.SetTrigger("AttackUpDown");
                break;
            case Vector2 v when v == Vector2.up:
                animator.SetTrigger("AttackUpDown");
                break;
            case Vector2 v when v == Vector2.left:
                animator.SetTrigger("AttackLeft");
                break;
            case Vector2 v when v == Vector2.right:
                animator.SetTrigger("AttackRight");
                break;
        }

        rb.linearDamping = 0;
        rb.AddForce((Player.Instance.transform.position - transform.position).normalized * speed * 1.5f, ForceMode2D.Impulse);
        StartCoroutine(ApplyDrag(0.25f, 5f));
        StartCoroutine(NormalSpeed(1f));

        yield return new WaitForSeconds(attackDuration);
    }

    public IEnumerator NormalSpeed(float delay)
    {
        yield return new WaitForSeconds(delay);
        speed = 3.5f;
    }

    public IEnumerator StopAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        attackCollider.enabled = false;
    }

    public override void Chase()
    {
        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
        DirectionAnimation(direction.x, direction.y);

        /*
        //alternative for moving
        Debug.Log("Hopping");
        rb.linearDamping = 0;
        rb.AddForce((Player.Instance.transform.position - transform.position).normalized * speed, ForceMode2D.Impulse);
        jumpChaseTimer = jumpChaseCooldown;
        StartCoroutine(ApplyDrag(0.3f, 5f));
        
        */
    }

    public override void Roam()
    {
        if (!hasRoamTarget || Vector2.Distance(transform.position, roamTarget) < 0.2f)
        {
            float randomX = Random.Range(roamOrigin.x - roamRange, roamOrigin.x + roamRange);
            float randomY = Random.Range(roamOrigin.y - roamRange, roamOrigin.y + roamRange);
            roamTarget = new Vector2(randomX, randomY);
            hasRoamTarget = true;
            roamTimer = roamInterval; 
            rb.linearVelocity = Vector2.zero; 
            SetAnimationBools(false, false, false); 
            return; 
        }

        Vector2 moveDir = (roamTarget - (Vector2)transform.position).normalized;
        rb.linearVelocity = moveDir * (speed * 0.5f);
        DirectionAnimation(moveDir.x, moveDir.y);
    }


    private IEnumerator ApplyDrag(float delay, float dragValue)
    {
        yield return new WaitForSeconds(delay);
        rb.linearDamping = dragValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Player.Instance.gameObject)
        {
            Player.Instance.TakeDamage(damage);
        }
    }

    public void Knockback()
    {
        Vector2 knockbackDir = (transform.position - Player.Instance.transform.position).normalized;
        rb.linearVelocity = Vector2.zero; 
        rb.AddForce(knockbackDir * 20f, ForceMode2D.Impulse);
        StartCoroutine(KnockbackRoutine());
    }

    private IEnumerator KnockbackRoutine()
    {
        isKnockedBack = true;
        yield return new WaitForSeconds(0.3f); 
        isKnockedBack = false;
    }


    public override void TakeDamage(float damage)
    {
        Debug.Log("Tiyanak took damage: " + damage);
        health -= damage;
        Knockback();
        StartCoroutine(Flash());
        if (health <= 0)
        {
            QuestSystem.Instance.CheckActiveObjective("Tiyanak");
            Destroy(gameObject);
        }
    }
}
