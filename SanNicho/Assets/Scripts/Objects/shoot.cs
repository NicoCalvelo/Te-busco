using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Controla el movimiento del disparo y las colisiones.
/// 
/// Creación:
///     18/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     24/05/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class shoot : MonoBehaviour
{
    Vector3 targetPos;
    [SerializeField]
    float shootSpeed = 10;

    private void Start()
    {
        targetPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        targetPos = new Vector3(targetPos.x, targetPos.y + 2); 

        Vector2 diference = targetPos - transform.position;
        float sign = (transform.position.x < targetPos.x) ? -1.0f : 1.0f;
        float angle = Vector2.Angle(Vector2.up, diference) * sign;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * shootSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
