using UnityEngine;
using System.Collections; // Needed for Coroutines

public class Tiyanak : Enemy
{
    public Rigidbody2D rb;
    public float jumpChaseTimer = 0;
    public float jumpChaseCooldown = 5f;

    public float roamTimer = 0;
    public float roamInterval = 2f; // How often it chooses a new spot
    public CircleCollider2D attackCollider;

    public Vector2 topRight;
    public Vector2 topLeft;
    public Vector2 bottomRight;
    public Vector2 bottomLeft;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackCollider = GetComponent<CircleCollider2D>();
        health = 100f;
        speed = 3.5f;
        damage = 20f;
        attackRange = 3f;
        attackCooldown = 2f;
        chaseRange = 15f;
        roamRange = 10f;
    }

    public void FixedUpdate()
    {
        // Keep timers running
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
        if (jumpChaseTimer > 0) jumpChaseTimer -= Time.deltaTime;
        if (roamTimer > 0) roamTimer -= Time.deltaTime;

        if (Player.Instance.currentState != Player.PlayerState.Moving) return;

        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);

        if (distanceToPlayer <= attackRange && attackTimer <= 0)
        {
            Attack();
        }
        else if (distanceToPlayer <= chaseRange && jumpChaseTimer <= 0)
        {
            Chase();
        }
        else if (roamTimer <= 0)
        {
            Roam();
        }
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
        attackCollider.enabled = true;
        StartCoroutine(StopAttack(attackDuration));
        rb.linearDamping = 0;
        rb.AddForce((Player.Instance.transform.position - transform.position).normalized * speed * 1.5f, ForceMode2D.Impulse);
        attackTimer = attackCooldown;
        speed = 2f;
        StartCoroutine(ApplyDrag(0.25f, 5f));
        StartCoroutine(NormalSpeed(1f));

        /*
        //alternative for attack

        
        */
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
        transform.position = Vector2.MoveTowards(transform.position, Player.Instance.transform.position, speed * Time.deltaTime);

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

        float randomX = Random.Range(bottomLeft.x, bottomRight.x);
        float randomY = Random.Range(bottomLeft.y, topLeft.y);
        Vector2 targetPosition = new Vector2(randomX, randomY);

        Vector2 moveDir = (targetPosition - (Vector2)transform.position).normalized;
        rb.MovePosition(rb.position + moveDir * (speed * 0.5f) * Time.fixedDeltaTime);

        roamTimer = roamInterval;

    }

    private IEnumerator ApplyDrag(float delay, float dragValue)
    {
        yield return new WaitForSeconds(delay);
        rb.linearDamping = dragValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(damage);
        }
    }
}
