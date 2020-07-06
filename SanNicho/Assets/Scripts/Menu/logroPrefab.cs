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
///     06/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class logroPrefab : MonoBehaviour
{
    public infoJugador.logro thisLogro;

    public TextMeshProUGUI titulo, descripcion;
    public Image frontImg;
    public Button reclamarBTN;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        reclamarBTN.interactable = false;
    }

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
                anim.SetBool("completado", true);
                reclamarBTN.GetComponent<TextMeshProUGUI>().text = "reclamado";
                return;
            }

            reclamarBTN.interactable = true;
        }
    }

    void onclickReclacmar()
    {
        anim.SetBool("completado", true);
        reclamarBTN.GetComponent<TextMeshProUGUI>().text = "reclamado";

        //Animacion de monedas ganadas
    }

}
