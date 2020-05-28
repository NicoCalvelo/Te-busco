﻿using System.Collections;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Contiene todo lo referido al control de las mecanicas del jugador.
/// 
/// Creación:
///     01/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     15/05/2020 Calvelo Nicolás
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
    private float jumpFoce = .5f;
    [SerializeField]
    private float speed = 10f;
    float initialSpeed;


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
    }

    #region Collisions

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "NPC")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col.otherCollider);
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {

    }

    private void OnCollisionExit2D(Collision2D col)
    {

    }

    #endregion

    #region Moving State
    void MoveState()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        //Saltar
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            audioManager.Instance.playSound(audioManager.gameObjectSource.playerSound, "Jumping");
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpFoce);
            isGrounded = false;
            resetJumpNedded = true;
            StartCoroutine(ResetJumpNeededRoutine());

            playerAnimController.jump(true);
        }
        //Agacharse
        if(Input.GetKey(KeyCode.S))
        {
            playerAnimController.crouch(true);
            speed = initialSpeed / 2.5f;
            
        }else if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnimController.crouch(false);
            speed = initialSpeed;
        }

        rb2D.velocity = new Vector2(xAxis * speed, rb2D.velocity.y);
        if (xAxis == 1)
            playerAnimController.flip(false);
        if (xAxis == -1)
            playerAnimController.flip(true);
        playerAnimController.move(xAxis);
        
    }

    void checkGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, floorDistance, groundLayerMask.value);

        if (rb2D.velocity.y < -10 && resetLandingAudio == true)
            playLandingAudio = true;

        if (hitInfo.collider != null && resetJumpNedded == false)
        {
            isGrounded = true;
            playerAnimController.jump(false);

            if (playLandingAudio == true && resetLandingAudio == true)
            {
                audioManager.Instance.playSound(audioManager.gameObjectSource.playerSound, "Jumping");
                playLandingAudio = false;
                resetLandingAudio = false;
                StartCoroutine(resetAudioLandingNeededRoutine());
            }
        }
    }

    IEnumerator ResetJumpNeededRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        resetJumpNedded = false;
    }
    IEnumerator resetAudioLandingNeededRoutine()
    {
        yield return new WaitForSeconds(.5f);
        resetLandingAudio = true;
    }
    #endregion


}
