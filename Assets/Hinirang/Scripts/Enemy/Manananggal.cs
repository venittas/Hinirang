using System.Collections;
using Unity.VisualScripting; // Needed for Coroutines
using UnityEngine;

public class Manananggal : Enemy
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
    public float yOffset = 4f;
    private bool isDead = false;

    public static Manananggal Instance;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        health = 250f;
        speed = 2f;
        damage = 33f;
        attackRange = 10f;
        attackCooldown = 2f;
        chaseRange = 15f;
        roamRange = 10f;
        roamOrigin = transform.position; 
        UpdateRoamCoordinates();
        
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Check()
    {
        if (Player.Instance.eventNameTrigger != "Day7")
        {
            gameObject.SetActive(false);
        }
    }

    public void FixedUpdate()
    {
        Check();
        if (!Move) return;
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
        if (jumpChaseTimer > 0) jumpChaseTimer -= Time.deltaTime;
        if (roamTimer > 0) roamTimer -= Time.deltaTime;

        if (Player.Instance.currentState != Player.PlayerState.Moving && 
            Player.Instance.currentState == Player.PlayerState.Dead) return;

        Vector2 adjustedPos = new Vector2(transform.position.x, transform.position.y + yOffset);
        float distanceToPlayer = Vector2.Distance(adjustedPos, Player.Instance.transform.position);
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
        return;
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

        Vector2 dirToPlayer = (Player.Instance.transform.position - transform.position).normalized;
        lastLookDirection = GetDominantDirection(dirToPlayer);

        animator.SetTrigger("Soar");
        float soarHeight = 3f; 
        Vector2 soarTarget = (Vector2)transform.position + Vector2.up * soarHeight;

        float soarDuration = 0.92f;
        float elapsed = 0f;
        Vector2 startPos = transform.position;

        while (elapsed < soarDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, soarTarget, elapsed / soarDuration);
            yield return null;
        }

        animator.SetTrigger("Claw");
        attackCollider.enabled = true;

        float clawDuration = 1.50f - 0.92f; 
        elapsed = 0f;
        startPos = transform.position;
        Vector2 clawTarget = Player.Instance.transform.position; 
        while (elapsed < clawDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, clawTarget, elapsed / clawDuration);
            yield return null;
        }

        attackCollider.enabled = false;


    }

    private Vector2 GetDominantDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return dir.x > 0 ? Vector2.right : Vector2.left;
        else
            return dir.y > 0 ? Vector2.up : Vector2.down;
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
        //DirectionAnimation(direction.x, direction.y);

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
        if (isDead) return;
        Debug.Log("Manananggal took damage: " + damage);
        health -= damage;
        Knockback();
        StartCoroutine(Flash());
        if (health <= 0)
        {
            isDead = true;
            bool check = QuestSystem.Instance.CheckActiveObjective("Manananggal");
            if (check) Debug.LogWarning("Manananggal objective completed!");
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + yOffset), attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + yOffset), chaseRange);
    }
}
