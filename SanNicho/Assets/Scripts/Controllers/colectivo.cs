using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <Documentacion>
/// Resumen:
///     Este script controla al colectivo.
/// 
/// Creación:
///     21/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     21/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class colectivo : MonoBehaviour
{
    Animator anim;
    AudioSource audio;
    [SerializeField]
    float speed;

    [SerializeField]
    Vector2[] positions;
    bool canMove = false;
    Queue<Vector2> pos;
    Vector2 nextPos;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        pos = new Queue<Vector2>();
        for (int i = 0; i < positions.Length; i++)
        {
            pos.Enqueue(positions[i]);
        }
        StartCoroutine(setPositions());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            canMove = true;
            audio.Play();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            canMove = false;
            audio.Stop();
        }
    }

    IEnumerator setPositions()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            nextPos = pos.Dequeue();
            yield return new WaitUntil(() => Vector2.Distance(transform.position, nextPos) < 5);
            progressManager.Instance.progressData.logros[2].porcentajeCompletado = 1.0f * positions.Length / pos.Count;
        }

        progressManager.Instance.progressData.logros[2].completado = true;
        anim.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        audio.enabled = false;
        audioManager.Instance.playSound("horn");
        canMove = false;
    }

    private void FixedUpdate()
    {
        if (canMove)
            transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }
}
