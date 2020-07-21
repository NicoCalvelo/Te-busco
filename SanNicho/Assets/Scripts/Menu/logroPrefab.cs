using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <Documentacion>
/// Resumen:
///     Este script controla el prefab de un logro en el juego.
/// 
/// Creación:
///     06/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     07/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class logroPrefab : MonoBehaviour
{
    public infoJugador.logro thisLogro;

    public TextMeshProUGUI titulo, descripcion, monedasText;
    public Image frontImg, medallaImg;
    public Button reclamarBTN;

    public Animator anim;


    //Se setea el prefab en base a la informacion
    public void setPrefab()
    {
        titulo.text = thisLogro.titulo;
        descripcion.text = thisLogro.descripcion;
        frontImg.fillAmount = thisLogro.porcentajeCompletado;

        if (thisLogro.completado) // El logro se completo
        {
            frontImg.fillAmount = 1;

            if (thisLogro.reclamado) // El logro se reclamo
            {
                medallaImg.color = Color.white;
                reclamarBTN.GetComponentInChildren<TextMeshProUGUI>().text = "reclamado";
                reclamarBTN.interactable = false;
                return;
            }

            reclamarBTN.interactable = true;
        }
    }

    public void onclickReclacmar()
    {
        monedasText.gameObject.SetActive(true);

        //Animacion de monedas ganadas
        anim.SetTrigger("Reclamando");
        anim.SetBool("completado", true);
        reclamarBTN.GetComponentInChildren<TextMeshProUGUI>().text = "reclamado";
        audioManager.Instance.playSound("recompensa");
        reclamarBTN.interactable = false;

        progressManager.Instance.progressData.logros.Find(l => l.titulo == thisLogro.titulo).reclamado = true;
        monedasText.text = "+ " + thisLogro.monedasDeRecompensa.ToString();
        progressManager.Instance.progressData.totalPuntos += thisLogro.monedasDeRecompensa;
    }

}
