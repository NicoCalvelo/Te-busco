using System.Collections;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Contiene todo lo referido al control de las mecanicas del jugador.
/// 
/// Creación:
///     01/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     30/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class playerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private playerAnimController playerAnimController;
    [SerializeField]
    private LayerMask groundLayerMask;

    float xAxis = 0;
    private bool resetJumpNedded = false;
    bool playLandingAudio = false;
    bool resetLandingAudio = true;

    [SerializeField]
    private bool isGrounded = false;
    public float floorDistance;

    [Header("Movement Facts")]
    [SerializeField]
    private float jumpFoce = 31.0f;
    [SerializeField]
    private float speed = 10f;
    float initialSpeed;


    public enum Platform
    {
        PC, Mobile
    }
    public Platform actualPlatform = Platform.Mobile;

    enum State
    {
        Moving
    }
    State state = State.Moving;


    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerAnimController = FindObjectOfType<playerAnimController>();
        initialSpeed = speed;
        transform.position = new Vector2(-110, 1.5f);
    }

    void Update()
    {
        switch (state)
        {
            case State.Moving:
                MoveState();
                checkGrounded();
                break;
        }
        switch (actualPlatform)
        {
            case Platform.PC:
                PCinput();
                break;
        }

    }

    #region Collisions

    public void setCollider(float xOffset)
    {
        GetComponent<Collider2D>().offset = new Vector2(xOffset, GetComponent<Collider2D>().offset.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "NPC")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col.collider);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Photo")
        {
            gameManager.Instance.setStars(-1);
        }
    }

    #endregion

    #region Cross Platform Controller
    void PCinput()
    {
        //Saltar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
        //Agacharse
        if (Input.GetKeyDown(KeyCode.S))
        {
            crouch();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            standUp();
        }

        xAxis = Input.GetAxisRaw("Horizontal");
    }

    public void mobileInput(int i)
    {
        xAxis = i;
    }
    #endregion

    #region Moving State

    RaycastHit2D hitInfo;

    void MoveState()
    {
        if(isGrounded == false)
            hitInfo = Physics2D.CapsuleCast(transform.position, new Vector2(6 * transform.localScale.x, 15 * transform.localScale.y), CapsuleDirection2D.Vertical, 0, Vector2.down, floorDistance, groundLayerMask);

        rb2D.velocity = new Vector2(xAxis * speed, rb2D.velocity.y);

        if (xAxis == 1 && playerAnimController.spriteRenderer.flipX == true)
        {
            playerAnimController.flip(false);
            setCollider(.7f);
        }

        if (xAxis == -1 && playerAnimController.spriteRenderer.flipX == false)
        {
            playerAnimController.flip(true);
            setCollider(-.8f);
        }


        playerAnimController.move(xAxis);      
    }

    public void jump()
    {
        if (isGrounded == false)
            return;
        audioManager.Instance.playSound("Jumping");
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpFoce);
        isGrounded = false;
        resetJumpNedded = true;
        StartCoroutine(checkGrounded());

        playerAnimController.jump(true);
    }
    public void crouch()
    {
        playerAnimController.crouch(true);
        speed = initialSpeed / 2.5f;
    }
    public void standUp()
    {
        playerAnimController.crouch(false);
        speed = initialSpeed;
    }


    IEnumerator checkGrounded()
    {
        yield return new WaitForSeconds(1.1f);

        yield return new WaitUntil(() => hitInfo.collider != null);

        isGrounded = true;
        playerAnimController.jump(false);
        audioManager.Instance.playSound("playerLanding");

    }

    #endregion

    bool heladoActualState = false;
    bool chicleState = false;

    public bool onGetHelado
    {
        get
        {
            return heladoActualState;
        }
        set
        {
            if(heladoActualState == false)
            {
                gameObject.transform.localScale = gameObject.transform.localScale / 3;
                heladoActualState = true;
                audioManager.Instance.playSound("heladoIn");
            }
            else
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + 3);
                gameObject.transform.localScale = gameObject.transform.localScale * 3;
                heladoActualState = false;
                audioManager.Instance.playSound("heladoOut");
            }
        }
    }
    public void onGetResorte(bool enter)
    {
        if (enter)
        {
            jumpFoce = jumpFoce * 1.8f;
            //Animacion  de resortes
        }
        else
        {
            jumpFoce = jumpFoce / 1.8f;
        }
    }
    public void onGetChicle(bool enter)
    {
        if(enter == true && chicleState == false)
        {
            jumpFoce = jumpFoce / 2;
            playerAnimController.changeColor( new Color32(255, 175, 201, 255));
        }
        else if(chicleState == true)
        {
            jumpFoce = jumpFoce * 2;
            playerAnimController.changeColor(Color.white);
            chicleState = false;
        }
    }

}
