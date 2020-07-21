using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footBall : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "NPC")
        {
            rb.AddForce(Vector2.up * 14, ForceMode2D.Impulse);
        }
        if (collision.gameObject.layer == 8)
            return;
        else if (collision.gameObject.layer == 9)
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        else
        audioManager.Instance.playSound("kickFootball");


    }
}
