using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <Documentacion>
/// Resumen:
///     Este script setea la tarjeta de campeonato;
/// 
/// Creación:
///     10/08/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     23/08/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class tarjetaDePresentacion : MonoBehaviour
{

    public campeonatoAttribute thisCampeonato;
    public TextMeshProUGUI tipo_Text, coste_Text;
    public TextMeshProUGUI unirseText;
    public GameObject costoImg;
    public RawImage BackgroundTarjeta;

    int costoDeUnirse;
    Animator anim;
    int parameterDineroInsuficiente = Animator.StringToHash("dineroInsuficiente");

    bool participando = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void setTarjeta()
    {
        switch (thisCampeonato.tipoDeCampeonato)
        {
            case campeonatoAttribute.campeonatoTipo.express:
                tipo_Text.text = "campeonato express";
                tipo_Text.fontSize = 25;
                tipo_Text.fontStyle = FontStyles.LowerCase;
                tipo_Text.color = new Color(55, 55, 55);
                costoDeUnirse = 28;
                coste_Text.text = costoDeUnirse.ToString();
                break;
            case campeonatoAttribute.campeonatoTipo.campeonato:
                tipo_Text.text = "Campeonato";
                tipo_Text.fontSize = 27;
                tipo_Text.fontStyle = FontStyles.SmallCaps;
                tipo_Text.color = new Color(44, 115, 106);
                costoDeUnirse = 50;
                coste_Text.text = costoDeUnirse.ToString();
                break;
            case campeonatoAttribute.campeonatoTipo.megaCampeonato:
                tipo_Text.text = "Mega Campeonato";
                tipo_Text.fontSize = 29;
                tipo_Text.fontStyle = FontStyles.UpperCase;
                tipo_Text.color = new Color(255, 82, 0);
                costoDeUnirse = 85;
                coste_Text.text = costoDeUnirse.ToString();
                break;
        }

        BackgroundTarjeta.texture = thisCampeonato.backgroundTarjeta;

        if(campeonatosManager.Instance.campeonatosData.campeonatosInscripto.ContainsKey(thisCampeonato.campeonatoID) == true)
        {
            participando = true;
            costoImg.SetActive(false);
            unirseText.text = "Jugar";
        }
    }

    //Cuando el jugador quiere unirse a determinado campeonato
    public void onClickUnirse()
    {
        panelChamp.Instance.thisChamp = thisCampeonato;
        panelChamp.Instance.thisProgress = campeonatosManager.Instance.campeonatosData.userCampeonatoProgress[thisCampeonato.campeonatoID];
        //Ya se habia inscripto
        if(participando)
        {
            panelCampeonatos.Instance.onClickUnirse();
            return;
        }

        if(costoDeUnirse > progressManager.Instance.progressData.totalPuntos)  //No alcanza el dinero para unirse
        {
            anim.SetTrigger(parameterDineroInsuficiente);
            Invoke("resetTrigger", .3f);
        }
        else //Uniendose
        {
            progressManager.Instance.progressData.totalPuntos -= costoDeUnirse;
            panelCampeonatos.Instance.onClickUnirse();
            campeonatosManager.Instance.campeonatosData.campeonatosInscripto.Add(thisCampeonato.campeonatoID, thisCampeonato);
        }
    }

    void resetTrigger()
    {
        anim.ResetTrigger(parameterDineroInsuficiente);
    }
}
