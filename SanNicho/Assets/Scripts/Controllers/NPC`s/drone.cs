using System.Collections;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Controlla al npc dorne.
/// 
/// Creación:
///     26/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     26/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class drone : NPC_States
{

    [SerializeField]
    private Transform phone;
    [SerializeField]
    private GameObject shootPrefab;
    [SerializeField]
    private float attackDistance = 20, timeToShoot = 2, speed = 10;

    AudioSource audio;

    Vector2 startPos;
    Vector2 targetPos;

    public override void Awake()
    {
        base.Awake();
        attackDistance = progressManager.Instance.nextDayAttribute.NPC_04_attackDistance;
        timeToShoot = progressManager.Instance.nextDayAttribute.NPC_04_timeToShoot;
        audio = GetComponent<AudioSource>();
        anim.enabled = false;
        startPos = new Vector2(Random.Range(-200, 200), 85);
        transform.position = startPos;
        targetPos = Vector2.zero;
        StartCoroutine(pursuePlayer());
        InvokeRepeating("setTargetPos", 4.5f, 5);
    }

    void setTargetPos()
    {
        targetPos = new Vector2(Random.Range(playerTransform.position.x - 5, playerTransform.position.x + 5), Random.Range(playerTransform.position.y + 20, playerTransform.position.y + 30));
    }

    IEnumerator pursuePlayer()
    {
        float waitTime = Random.Range(3, 9);
        yield return new WaitForSeconds(waitTime);
        nextState = State.PATROL;
        yield return new WaitUntil(() => Vector2.Distance(transform.position, targetPos) < 2);
        if(Vector2.Distance(transform.position, playerTransform.position) < attackDistance)
        {
            nextState = State.ATTACK;
            CancelInvoke("setTargetPos");
        }
        else
        {
            nextState = State.IDLE;
        }
    }

    IEnumerator toAttack()
    {
        yield return new WaitForSeconds(timeToShoot);
        Instantiate(shootPrefab, phone.position, Quaternion.identity);
        audioManager.Instance.playSound("NPCshoot");
        yield return new WaitForSeconds(1);
        StartCoroutine(reset());
    }

    IEnumerator reset()
    {
        targetPos = new Vector2(Random.Range(transform.position.x + 50, transform.position.x - 50), Random.Range(transform.position.y + 5, transform.position.y + 25));
        nextState = State.PATROL;
        yield return new WaitUntil(() => Vector2.Distance(transform.position, targetPos) < 2);
        nextState = State.IDLE;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void enterIdle()
    {
        StartCoroutine(pursuePlayer());
        InvokeRepeating("setTargetPos", 2.5f, 5);
        base.enterIdle();
    }

    public override void enterPatrol()
    {
        base.enterPatrol();
    }
    public override void patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        base.patrol();
    }

    public override void enterAttack()
    {
        StartCoroutine(toAttack());
        base.enterAttack();
    }

    private void OnBecameInvisible()
    {
        anim.enabled = false;
        audio.Stop();
    }
    private void OnBecameVisible()
    {
        anim.enabled = true;
        audio.Play();
    }

}
