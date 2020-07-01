using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Controla a una camara de seguridad.
/// 
/// Creación:
///     30/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     30/06/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class NPC_Camera : NPC_States
{
    [SerializeField]
    private Transform phone;
    [SerializeField]
    private GameObject shootPrefab;
    [SerializeField]
    private float  attackDistance = 20, timeToShoot = 2;

    public override void Awake()
    {
        attackDistance = progressManager.Instance.nextDayAttribute.NPC_03_attackDistance;
        timeToShoot = progressManager.Instance.nextDayAttribute.NPC_03_timeToShoot;

        base.Awake();
    }

    private void OnBecameInvisible()
    {
        StopAllCoroutines();
        anim.enabled = false;
    }

    private void OnBecameVisible()
    {
        StartCoroutine(toAttack());
        anim.enabled = true;
    }

    IEnumerator toAttack()
    {

        yield return new WaitUntil(() => Vector2.Distance(transform.position, playerTransform.position) < attackDistance);
        anim.SetBool("attack", true);
        //Sonido de la camara de que va a atacar
        yield return new WaitForSeconds(timeToShoot);
        Instantiate(shootPrefab, phone.position, Quaternion.identity);
        audioManager.Instance.playSound("NPCshoot");
        anim.SetBool("attack", false);
        yield return new WaitForSeconds(3);
        StartCoroutine(toAttack());
    }
}
