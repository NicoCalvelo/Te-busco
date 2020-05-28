using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Contiene las bases de los diferentes modos de NPC.
/// 
/// Creación:
///     18/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     21/05/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class NPC_States : MonoBehaviour
{
    public enum State
    {
        IDLE, PATROL, PURSUE, ATTACK
    }

    public State actualState = State.IDLE;
    protected State nextState  = State.IDLE, prevState = State.IDLE;
    protected Animator anim;
    protected Transform playerTransform, shootFrom;
    
    public AudioSource idle_ASource, patrol_ASource, pursue_ASource, attack_ASource;


    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public virtual void FixedUpdate()
    {
        if (prevState != actualState)
        {
            if(nextState == State.IDLE)
                enterIdle();
            else if(nextState == State.PATROL)
                enterPatrol();
            else if(nextState == State.PURSUE)
                enterPursue();
            else if(nextState == State.ATTACK)
                enterAttack();

            prevState = actualState;
        }

        switch (actualState)
        {
            case State.IDLE :
                idle();
                break;
            case State.PATROL:
                patrol();
                break;
            case State.PURSUE:
                pursue();
                break;
            case State.ATTACK:
                attack();
                break;
        }
    }

    public virtual void enterIdle()
    {
        anim.SetTrigger("isIdle");
        actualState = State.IDLE;
    }
    public virtual void idle()
    {


        if(nextState != State.IDLE)
            exitIdle();
    }
    public virtual void exitIdle()
    {
        anim.ResetTrigger("isIdle");

        if (idle_ASource != null)
            if (idle_ASource.isPlaying == true)
                idle_ASource.Stop();

        actualState = nextState;
    }

    public virtual void enterPatrol()
    {
        anim.SetTrigger("isPatroling");
        actualState = State.PATROL;
    }
    public virtual void patrol()
    {


        if (nextState != State.PATROL)
            exitPatrol();
    }
    public virtual void exitPatrol()
    {
        anim.ResetTrigger("isPatroling");

        if (patrol_ASource != null)
            if (patrol_ASource.isPlaying == true)
                patrol_ASource.Stop();

        actualState = nextState;
    }

    public virtual void enterPursue()
    {
        anim.SetTrigger("isPursuing");
        actualState = State.PURSUE;
    }
    public virtual void pursue()
    {



        if (nextState != State.PURSUE)
            exitPursue();
    }
    public virtual void exitPursue()
    {
        anim.ResetTrigger("isPursuing");

        actualState = nextState;
    }

    public virtual void enterAttack()
    {
        anim.SetTrigger("isAttacking");
        actualState = State.ATTACK;
    }
    public virtual void attack()
    {



        if (nextState != State.ATTACK)
            exitAttack();
    }
    public virtual void exitAttack()
    {
        anim.ResetTrigger("isAttacking");

        actualState = nextState;
    }

    public virtual void playNpcSound()
    {
        if(actualState == State.IDLE && idle_ASource != null)
            idle_ASource.Play();
        else if (actualState == State.PATROL && patrol_ASource != null)
            patrol_ASource.Play();
        else if (actualState == State.PURSUE && pursue_ASource != null)
            pursue_ASource.Play();
        else if (actualState == State.ATTACK && attack_ASource != null)
            attack_ASource.Play();
    }
}
