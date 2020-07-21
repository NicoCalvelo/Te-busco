using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Contiene todo lo referido al control de las animaciones del jugador.
/// 
/// Creación:
///     01/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     29/06/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class playerAnimController : MonoBehaviour
{
    #region singleton
    private static playerAnimController _instance;
    public static playerAnimController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("The Manager is NULL");
            }

            return _instance;
        }
    }
    #endregion

    private Animator anim;
    public SpriteRenderer spriteRenderer;

    //Animations parameters
    int parameterMove = Animator.StringToHash("Move");
    int parameterJump = Animator.StringToHash("Jumping");
    int parameterCrouch = Animator.StringToHash("Crouch");
    int parameterBubble = Animator.StringToHash("bubbleConsumption");

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void changeColor(Color newColor)
    {
        spriteRenderer.color = newColor;
    }

    public void flip(bool facingLeft)
    {
        spriteRenderer.flipX = facingLeft;
    }
    public void move(float move)
    {       
        anim.SetFloat(parameterMove, Mathf.Abs(move));
    }
    public void footStep()
    {
        audioManager.Instance.playSound("Step");
    }
    public void crouchFootStep()
    {
        audioManager.Instance.playSound("CrouchStep");
    }

    public void jump(bool jumping)
    {
        anim.SetBool(parameterJump, jumping);
    }
    public void crouch(bool crouching)
    {
        anim.SetBool(parameterCrouch, crouching);
    }
    public void setBubble(float value)
    {
        anim.SetFloat(parameterBubble, value);
    }
}
