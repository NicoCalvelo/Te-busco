using System.Collections;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script controla a un npc que camina por el fondo.
/// 
/// Creación:
///     28/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     29/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class Npc__Background : MonoBehaviour
{
    Vector2 nextTarget;
    Animator anim;
    float speed = 5;

    enum state{ idle, patrol};
    state estado;

    int parameterPatrol = Animator.StringToHash("isPatrol");

    void Start()
    {      
        anim = GetComponent<Animator>();
        StartCoroutine(setNextTarget());
    }

    IEnumerator setNextTarget()
    {
        estado = state.idle;
        anim.SetBool(parameterPatrol, false);

        yield return new WaitForSeconds(Random.Range(2.0f, 7.0f));

        nextTarget = new Vector2(Random.Range(gameManager.Instance.sceneLimitLeft + 75, gameManager.Instance.sceneLimitRigth - 75), transform.position.y);
        estado = state.patrol;
        if (nextTarget.x < transform.position.x)
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
        else
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);

        anim.SetBool(parameterPatrol, true);

        yield return new WaitUntil(() => Vector2.Distance(transform.position, nextTarget) < 3);

        StartCoroutine(setNextTarget());
    }

    private void FixedUpdate()
    {
        switch (estado)
        {
            case state.patrol:
                transform.position = Vector2.MoveTowards(transform.position, nextTarget, speed * Time.deltaTime);
                break;
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
