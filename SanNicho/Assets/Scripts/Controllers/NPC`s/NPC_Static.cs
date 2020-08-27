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
    private float hideTime = 2f, attackDistance = 20, timeToShoot = 1;

    int parameterShow = Animator.StringToHash("show");

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
        anim.SetBool(parameterShow, false);
    }

    IEnumerator toAttack()
    {
        yield return new WaitForSeconds(hideTime);

        yield return new WaitUntil(() => Vector2.Distance(transform.position, playerTransform.position) < attackDistance);
        anim.enabled = true;
        if (playerTransform.position.x < transform.position.x)                       
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.parent.rotation.w);         
        else if (playerTransform.position.x > transform.position.x)          
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.parent.rotation.w);
            
        anim.SetBool(parameterShow, true);
        audioManager.Instance.playSound("bush");
        yield return new WaitForSeconds(timeToShoot);
        npcManager.Instance.requestPhoto(phone.position);
        yield return new WaitForSeconds(1);
        anim.SetBool(parameterShow, false);
        yield return new WaitForSeconds(2);
        StartCoroutine(toAttack());
    }

}
