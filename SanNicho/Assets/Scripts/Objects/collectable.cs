using UnityEngine;

public class collectable : MonoBehaviour
{
    public gameManager.collectables item;

    private void OnBecameInvisible()
    {
        GetComponent<Animator>().enabled = true;
    }

    private void OnBecameVisible()
    {
        GetComponent<Animator>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            gameManager.Instance.onCollect(item);
            audioManager.Instance.playSound("collectable");
            Destroy(gameObject);
        }
    }
}
