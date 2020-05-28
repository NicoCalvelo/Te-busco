using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Controla la animación de plantas cuando collisiona con algun objeto.
///     
/// Creación:
///     09/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     16/05/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class animatedPlantController : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetFloat("otherVelocity", collision.attachedRigidbody.velocity.x);
        anim.SetTrigger("collisionEnter");     
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.ResetTrigger("collisionEnter");
    }
}
