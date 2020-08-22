using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Controla a un NPC que pasa por los cuatro estados de movimiento.
/// 
/// Creación:
///     18/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     11/08/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class NPC_Complete : NPC_States
{
    [SerializeField]
    private Transform phone;
    [SerializeField]
    private float idleTime = 2f, patrolSpeed = 8, pursueSpeed = 13, visibilityDistance = 45, attackDistance = 20, timeToShoot = 5;

    Rigidbody2D rb2D;
    Coroutine outOfScreen;

    float idleTimeLeft, shootTimeLeft;
    Vector2 patrolTarget;
    float distanceToPlayer;
    float sign;

    //Animations parameters
    int parameterIdle = Animator.StringToHash("isIdle");
    int parameterPatrol = Animator.StringToHash("isPatroling");
    int parameterPursue = Animator.StringToHash("isPursuing");
    int parameterAttack = Animator.StringToHash("isAttacking");


    public override void Awake()
    {
        idleTime = Random.Range(1.4f, 2.6f);
        visibilityDistance = progressManager.Instance.nextDayAttribute.NPC_01_visibilityDistance;
        timeToShoot = progressManager.Instance.nextDayAttribute.NPC_01_timeToShoot;

        rb2D = GetComponent<Rigidbody2D>();
        idleTimeLeft = idleTime;

        base.Awake();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void enterIdle()
    {
        anim.SetTrigger(parameterIdle);

        idleTimeLeft = idleTime;
        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > 105 && outOfScreen == null)
            outOfScreen = StartCoroutine(npcManager.Instance.exitPlayerView(transform));

        base.enterIdle();
    }
    public override void idle()
    {
        idleTimeLeft -= Time.deltaTime;
        if (idleTimeLeft < 0)
        {
            nextState = State.PATROL;
            anim.ResetTrigger(parameterIdle);
        }

        base.idle();
    }

    public override void enterPatrol()
    {
        anim.SetTrigger(parameterPatrol);
        patrolTarget = new Vector2(Random.Range(transform.position.x - 80, transform.position.x + 80), 0);

        if (patrolTarget.x < gameManager.Instance.sceneLimitLeft || patrolTarget.x > gameManager.Instance.sceneLimitRigth)
            patrolTarget = transform.position + Vector3.left;

        sign = (transform.position.x > patrolTarget.x) ? -1.0f : 1.0f;

        if (patrolTarget.x > transform.position.x)
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
        else
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);

        base.enterPatrol();
    }
    public override void patrol()
    {
        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer < visibilityDistance)
        {
            nextState = State.PURSUE;
        }
        if(distanceToPlayer > 130)
        {
            nextState = State.IDLE;
        }

        float distanceToTarget = Vector2.Distance(transform.position, patrolTarget);

        if (distanceToTarget < 4)
        {
            nextState = State.IDLE;
        }

        rb2D.velocity = new Vector2(patrolSpeed * sign, rb2D.velocity.y);

        base.patrol();
    }
    public override void exitPatrol()
    {
        rb2D.velocity = Vector2.zero;
        anim.ResetTrigger(parameterPatrol);
        base.exitPatrol();
    }

    public override void enterPursue()
    {
        anim.SetTrigger(parameterPursue);

        if (playerTransform.position.x < transform.position.x)
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
        else
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);


        base.enterPursue();
    }
    public override void pursue()
    {
        base.pursue();
        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer < attackDistance)
            nextState = State.ATTACK;
        else if (distanceToPlayer > visibilityDistance)
            nextState = State.IDLE;

        sign = (transform.position.x > playerTransform.position.x) ? -1.0f : 1.0f;
        rb2D.velocity = new Vector2(pursueSpeed * sign, rb2D.velocity.y);
    }
    public override void exitPursue()
    {
        rb2D.velocity = Vector2.zero;
        anim.ResetTrigger(parameterPursue);
        base.exitPursue();
    }

    public void playPursueSound()
    {
        audioManager.Instance.playSound("NPCstep");
    }

    public override void enterAttack()
    {
        shootTimeLeft = timeToShoot;
        anim.SetTrigger(parameterAttack);
        base.enterAttack();
    }
    public override void attack()
    {
        shootTimeLeft -= Time.deltaTime;

        if (shootTimeLeft < 0)
        {

            npcManager.Instance.requestPhoto(phone.position);
            shootTimeLeft = timeToShoot;

            distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer > attackDistance)
            {
                anim.ResetTrigger(parameterAttack);
                nextState = State.IDLE;
            }

        }
        base.attack();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "NPC" || collision.gameObject.layer == 9)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
    }

    private void OnBecameInvisible()
    {
        outOfScreen = StartCoroutine(npcManager.Instance.exitPlayerView(transform));
        anim.enabled = false;
    }

    private void OnBecameVisible()
    {
        anim.enabled = true;

        if (outOfScreen != null)
        {
            StopCoroutine(outOfScreen);
            outOfScreen = null;
        }

    }
}

