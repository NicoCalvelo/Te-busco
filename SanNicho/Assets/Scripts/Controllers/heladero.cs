using System.Collections;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de controlar al heladero.
/// 
/// Creación:
///     22/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     22/07//2020 Calvelo Nicolás
///     
/// </Documentacion>

public class heladero : MonoBehaviour
{

    Animator anim;
    AudioSource audio;
    [SerializeField]
    GameObject heladoCollectable;

    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        StartCoroutine(spawnItems(progressManager.Instance.nextDayAttribute.heladosAgenerar));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("collision");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.ResetTrigger("collision");
        }
    }

    IEnumerator spawnItems(int cantidadAspawnear)
    {
        for (int i = 0; i <= cantidadAspawnear ; i++)
        {
            float tiempoDeIntervalo = (progressManager.Instance.nextDayAttribute.duracionDelDia * 60) / cantidadAspawnear;
            float primerTiempo = Random.Range(tiempoDeIntervalo / 4, tiempoDeIntervalo);
            yield return new WaitForSeconds(primerTiempo);
            if (GetComponent<SpriteRenderer>().isVisible)
            {
                audio.Play();
                anim.SetTrigger("generate");
                audioManager.Instance.playSound("horn");
                yield return new WaitForSeconds(2);

            }

            GameObject newItem = Instantiate(heladoCollectable, transform.position, Quaternion.identity);
            newItem.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);

            yield return new WaitForSeconds(tiempoDeIntervalo - primerTiempo);
            anim.ResetTrigger("generate");
        }
    }

    private void OnBecameInvisible()
    {
        anim.enabled = false;

    }
    private void OnBecameVisible()
    {
        anim.enabled = true;

    }
}
