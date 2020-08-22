using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script controla a doraemon.
/// 
/// Creación:
///     21/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     21/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class kitty : MonoBehaviour
{
    private Animator anim;
    private AudioSource audio;

    [SerializeField]
    private Vector2[] positions;

    int shots = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        transform.position = positions[Random.Range(0, positions.Length)];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.gameObject.tag == "Photo")
        {
            audio.Play();
            shots++;
            progressManager.Instance.progressData.logros[2].porcentajeCompletado = shots / 3;
            if (shots == 3)
            {
                progressManager.Instance.progressData.logros[2].completado = true;
                anim.SetTrigger("encontrado");
                Destroy(gameObject, 3.5f);
                return;
            }
        }
    }
}
