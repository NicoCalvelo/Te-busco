using System.Collections;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Controla a un NPC estatico.
/// 
/// Creación:
///     26/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     26/06/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class NPC_Static : NPC_States
{
    [SerializeField]
    private Transform phone;
    [SerializeField]
    private GameObject shootPrefab;
    [SerializeField]
    private float hideTime = 2f, attackDistance = 20, timeToShoot = 5;

    public override void Awake()
    {
        hideTime = progressManager.Instance.nextDayAttribute.NPC_02_hideTime;
        attackDistance = progressManager.Instance.nextDayAttribute.NPC_02_attackDistance;
        timeToShoot = progressManager.Instance.nextDayAttribute.NPC_02_timeToShoot;

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
        yield return new WaitForSeconds(hideTime);

        if (Vector2.Distance(transform.position, playerTransform.position) < attackDistance)
        {
            anim.SetBool("show", true);
            audioManager.Instance.playSound("bush");

            yield return new WaitForSeconds(timeToShoot);
            Instantiate(shootPrefab, phone.position, Quaternion.identity);
            audioManager.Instance.playSound("NPCshoot");
            yield return new WaitForSeconds(1);
            anim.SetBool("show", false);
            yield return new WaitForSeconds(2);
        }

    }

}
