using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Contiene las bases de los diferentes modos de NPC.
/// 
/// Creación:
///     18/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     16/06/2020 Calvelo Nicolás
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


    protected Transform playerTransform;
    


    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        playerTransform = gameManager.Instance.playerTransfrorm;
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
        actualState = State.IDLE;
    }
    public virtual void idle()
    {


        if(nextState != State.IDLE)
            exitIdle();
    }
    public virtual void exitIdle()
    {
        actualState = nextState;
    }

    public virtual void enterPatrol()
    {
        actualState = State.PATROL;
    }
    public virtual void patrol()
    {


        if (nextState != State.PATROL)
            exitPatrol();
    }
    public virtual void exitPatrol()
    {
        actualState = nextState;
    }

    public virtual void enterPursue()
    {
        actualState = State.PURSUE;
    }
    public virtual void pursue()
    {



        if (nextState != State.PURSUE)
            exitPursue();
    }
    public virtual void exitPursue()
    {
        actualState = nextState;
    }

    public virtual void enterAttack()
    {
        actualState = State.ATTACK;
    }
    public virtual void attack()
    {



        if (nextState != State.ATTACK)
            exitAttack();
    }
    public virtual void exitAttack()
    {
        actualState = nextState;
    }

}
