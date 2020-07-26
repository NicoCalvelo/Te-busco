using System.Collections;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de controlar a firulais.
/// 
/// Creación:
///     23/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     23/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class firulais : MonoBehaviour
{
    [SerializeField]
    float speed;

    Animator anim;
    Rigidbody2D rb;
    Vector2 nextPos;
    AudioSource audio;

    enum states { idle, run}
    states actualState;

    int pos = 1;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        actualState = states.idle;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(setPos());
    }

    IEnumerator setPos()
    {
        float waitTime = Random.Range(5.0f, 11.0f);
        yield return new WaitForSeconds(waitTime);
        nextPos = new Vector2(Random.Range(transform.position.x - 120, transform.position.x + 120), transform.position.y);
        if(nextPos.x > gameManager.Instance.sceneLimitRigth || nextPos.x < gameManager.Instance.sceneLimitLeft)
        {
            nextPos = Vector2.zero;
        }

        if(nextPos.x > transform.position.x)
        {
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
            pos = 1;
        }

        else
        {
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
            pos = -1;
        }

        actualState = states.run;
        anim.SetBool("running", true);
        yield return new WaitUntil(() => Vector2.Distance(transform.position, nextPos) < 6);
        audio.Play();
        actualState = states.idle;
        anim.SetBool("running", false);
        yield return new WaitForSeconds(3);
        StartCoroutine(setPos());
    }

    private void FixedUpdate()
    {
        switch (actualState)
        {
            case states.idle:
                break;
            case states.run:
                rb.velocity = (Vector2.right * pos) * speed * Time.deltaTime;
                break;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.layer == 9 || collision.gameObject.tag == "NPC")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
    }

    private void OnBecameInvisible()
    {
        Debug.Log("Se tescapo");
        gameObject.SetActive(false);
        progressManager.Instance.progressData.logros[3].porcentajeCompletado = gameManager.Instance.hora - 8 / 16;
    }
}
