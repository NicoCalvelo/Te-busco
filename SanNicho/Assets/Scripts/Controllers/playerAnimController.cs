using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Contiene todo lo referido al control de las animaciones del jugador.
/// 
/// Creación:
///     01/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     15/05/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class playerAnimController : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void flip(bool facingLeft)
    {
        spriteRenderer.flipX = facingLeft;
    }
    public void move(float move)
    {
        anim.SetFloat("Move", Mathf.Abs(move));
    }
    public void footStep()
    {
        audioManager.Instance.playSound(audioManager.gameObjectSource.playerSound, "Step");
    }
    public void crouchFootStep()
    {
        audioManager.Instance.playSound(audioManager.gameObjectSource.playerSound, "CrouchStep");
    }

    public void jump(bool jumping)
    {
        anim.SetBool("Jumping", jumping);
    }
    public void crouch(bool crouching)
    {
        anim.SetBool("Crouch", crouching);
    }
}
