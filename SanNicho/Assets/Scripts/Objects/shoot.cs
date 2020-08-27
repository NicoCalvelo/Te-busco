using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Controla el movimiento del disparo y las colisiones.
/// 
/// Creación:
///     18/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     26/08/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class shoot : MonoBehaviour
{
    Vector3 targetPos;
    [SerializeField]
    float shootSpeed = 17;

    bool stored = false;

    [SerializeField]
    private AudioSource audio;

    private void Start()
    {
        setPhoto();
    }

    private void OnEnable()
    {
        Invoke("setPhoto", .01f);
    }

    void setPhoto()
    {
        targetPos = gameManager.Instance.playerTransfrorm.position;
        targetPos = new Vector3(targetPos.x, targetPos.y + 3.5f); 

        Vector2 diference = targetPos - transform.position;
        float sign = (transform.position.x < targetPos.x) ? -1.0f : 1.0f;
        float angle = Vector2.Angle(Vector2.up, diference) * sign;

        transform.rotation = Quaternion.Euler(0, 0, angle);
        audio.Play();

        stored = false;

        if (progressManager.Instance.nextDayAttribute.shootLife > 0)
            Invoke("storeBullet", progressManager.Instance.nextDayAttribute.shootLife);
    }

    void storeBullet()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * shootSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bubble")
        {
            audioManager.Instance.playSound("hitBubble");
        }

        if (col.gameObject.tag != "NPC")
        {
            gameObject.SetActive(false);
        }
    }

    private void OnBecameInvisible()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        npcManager.Instance.storePhoto(gameObject);

    }
}
