using UnityEngine;
using System.Collections;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de controlar a el famosisimo.
/// 
/// Creación:
///     24/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     21/08/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class elFamoso : MonoBehaviour
{
    [SerializeField]
    GameObject resorteColletable;
    AudioSource motorAudio;
    Animator anim;
    SpriteRenderer sp;

    [SerializeField]
    GameObject ligths;

    int parameterConducir = Animator.StringToHash("conducir");


    private void Start()
    {
        motorAudio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        StartCoroutine(pasadas());
    }

    IEnumerator pasadas()
    {
        anim.enabled = false;
        sp.enabled = false;
        float waitTime = (progressManager.Instance.nextDayAttribute.duracionDelDia * 60) / progressManager.Instance.nextDayAttribute.pasadas;
        for (int i = 0; i <= progressManager.Instance.nextDayAttribute.pasadas; i++)
        {
            float time = Random.Range(0.1f, waitTime);
            yield return new WaitForSeconds(time);
            sp.enabled = true;
            anim.enabled = true;
            anim.SetBool(parameterConducir, true);

            float spawnCollectTime = Random.Range(3.0f, 17.0f);
            yield return new WaitForSeconds(spawnCollectTime);
            anim.SetBool(parameterConducir, false);
            GameObject newItem = Instantiate(resorteColletable, transform.position, Quaternion.identity);
            newItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(-.5f, 1) * 6, ForceMode2D.Impulse);
            yield return new WaitForSeconds(21.0f - spawnCollectTime);
            sp.enabled = false;
            anim.enabled = false;
            yield return new WaitForSeconds(waitTime-time);
        }
    }

    private void OnBecameInvisible()
    {
        motorAudio.Stop();
        if (ligths.activeSelf)
            ligths.SetActive(false);
    }
    private void OnBecameVisible()
    {
        motorAudio.Play();
        if (gameManager.Instance.hora >= 20)
            ligths.SetActive(true);
    }

}
