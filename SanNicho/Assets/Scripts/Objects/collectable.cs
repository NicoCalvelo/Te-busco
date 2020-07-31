using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script controla a los objetos recolectables y manda la informacion
///     al gameManager sobre el objeto en cuestion.
/// 
/// Creación:
///     29/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     29/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class collectable : MonoBehaviour
{
    public gameManager.collectables item;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnBecameInvisible()
    {
        if (anim != null)
        anim.enabled = true;
    }

    private void OnBecameVisible()
    {
        if (anim != null)
            anim.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            gameManager.Instance.onCollect(item);
            audioManager.Instance.playSound("collectable");
            Destroy(gameObject);
        }
    }
}
