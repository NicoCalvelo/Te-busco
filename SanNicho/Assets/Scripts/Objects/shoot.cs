using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Controla el movimiento del disparo y las colisiones.
/// 
/// Creación:
///     18/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     21/05/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class shoot : MonoBehaviour
{
    Vector3 targetPos;
    [SerializeField]
    float shootSpeed = 10, timeToDestroy = -1;

    private void Start()
    {
        targetPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        targetPos = new Vector3(targetPos.x, targetPos.y + 2); 

        Vector2 diference = targetPos - transform.position;
        float sign = (transform.position.x < targetPos.x) ? -1.0f : 1.0f;
        float angle = Vector2.Angle(Vector2.up, diference) * sign;

        transform.rotation = Quaternion.Euler(0, 0, angle);


        if (timeToDestroy > 0)
            Destroy(gameObject, timeToDestroy);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * shootSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("Te vieron wachin");
            Destroy(gameObject);
        }
        else if(col.gameObject.tag == "NPC")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col.collider);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
