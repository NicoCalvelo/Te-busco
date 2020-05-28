using System.Collections;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Controla a un NPC que pasa por los cuatro estados de movimiento.
/// 
/// Creación:
///     18/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     21/05/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class NPC_Complete : NPC_States
{
    [SerializeField]
    private Transform phone;
    [SerializeField]
    private GameObject shootPrefab;
    [SerializeField]
    private float idleTime = 10f, patrolSpeed = 8, pursueSpeed = 13, visibilityDistance = 45, attackDistance = 20, timeToShoot = 5;
    Rigidbody2D rb2D;

    float idleTimeLeft, shootTimeLeft;
    Vector2 patrolTarget;
    float distanceToPlayer;

    public override void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        idleTimeLeft = idleTime;

        base.Awake();
    }

    public override void FixedUpdate()
    {
        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        base.FixedUpdate();
    }

    public override void enterIdle()
    {
        idleTimeLeft = idleTime;
        base.enterIdle();
    }
    public override void idle()
    {
        idleTimeLeft -= Time.deltaTime;
        if (idleTimeLeft < 0)
        {
            nextState = State.PATROL;
        }

        base.idle();
    }

    public override void enterPatrol()
    {
        patrolTarget = new Vector2(Random.Range(-350.0f, 600.0f), 0);

        base.enterPatrol();
    }
    public override void patrol()
    {
        if (distanceToPlayer < visibilityDistance)
        {
            nextState = State.PURSUE;
        }

        float distanceToTarget = Vector2.Distance(transform.position, patrolTarget);

        if (distanceToTarget < 5)
        {
            nextState = State.IDLE;
        }
        else if (patrolTarget.x < transform.position.x)
        {
            rb2D.velocity = new Vector2(patrolSpeed * -1, rb2D.velocity.y);
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
        }
        else if (patrolTarget.x > transform.position.x)
        {
            rb2D.velocity = new Vector2(patrolSpeed, rb2D.velocity.y);
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
        }

        base.patrol();
    }
    public override void exitPatrol()
    {
        rb2D.velocity = Vector2.zero;
        base.exitPatrol();
    }

    public override void enterPursue()
    {
        base.enterPursue();
    }
    public override void pursue()
    {
        base.pursue();

        if (distanceToPlayer < attackDistance)
            nextState = State.ATTACK;
        else if (distanceToPlayer > visibilityDistance)
            nextState = State.IDLE;

        if (playerTransform.position.x < transform.position.x)
        {
            rb2D.velocity = new Vector2(pursueSpeed * -1, rb2D.velocity.y);
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
        }
        else if (playerTransform.position.x > transform.position.x)
        {
            rb2D.velocity = new Vector2(pursueSpeed, rb2D.velocity.y);
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
        }

    }
    public override void exitPursue()
    {
        rb2D.velocity = Vector2.zero;
        base.exitPursue();
    }

    public override void enterAttack()
    {
        shootTimeLeft = timeToShoot;
        base.enterAttack();
    }
    public override void attack()
    {
        shootTimeLeft -= Time.deltaTime;

        if (shootTimeLeft < 0)
        {
            playNpcSound();
            Instantiate(shootPrefab, phone.position, Quaternion.identity);
            shootTimeLeft = timeToShoot;

            if (distanceToPlayer > attackDistance)
                nextState = State.IDLE;
        }
        base.attack();
    }
}

