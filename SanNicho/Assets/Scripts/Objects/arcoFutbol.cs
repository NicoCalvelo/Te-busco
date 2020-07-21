using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arcoFutbol : MonoBehaviour
{
    int goles = 0;

    [SerializeField]
    GameObject pelota;
    [SerializeField]
    Animator anim;

    Vector2 pelotaStartPos = new Vector2(-325, -6);

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == pelota)
        {
            Debug.Log("GOLLLLLL");
            goles++;
            progressManager.Instance.progressData.logros[0].porcentajeCompletado = goles*1.0f / 3;
            anim.SetTrigger("gol");
            audioManager.Instance.playSound("gol");
            audioManager.Instance.playSound("applause");

            if (goles == 3)
            {
                //Setear el logro
                progressManager.Instance.progressData.logros[0].completado = true;
                return;
            }

            StartCoroutine(onGoal());

        }
    }


    IEnumerator onGoal()
    {
        yield return new WaitForSeconds(.8f);
        pelota.SetActive(false);
        yield return new WaitForSeconds(2);
        anim.ResetTrigger("gol");
        pelota.SetActive(true);
        pelota.transform.position = pelotaStartPos;
    }
}
