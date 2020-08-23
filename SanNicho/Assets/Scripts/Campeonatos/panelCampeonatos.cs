using UnityEngine;
using TMPro;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de controlar el panel de campeonatos.
/// 
/// Creación:
///     19/08/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     23/08//2020 Calvelo Nicolás
///     
/// </Documentacion>

public class panelCampeonatos : MonoBehaviour
{
    #region singleton
    private static panelCampeonatos _instance;
    public static panelCampeonatos Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("The Manager is NULL");
            }

            return _instance;
        }
    }
    #endregion

    [SerializeField]
    Animator anim;
    int parameterToChamp = Animator.StringToHash("CampToChamp");
    int parameterToCamp = Animator.StringToHash("ChampToCamp");

    [SerializeField]
    TextMeshProUGUI monedasText;
    [SerializeField]
    GameObject tarjetaPrefab, tarjetasContent, littleLoad;

    private void Start()
    {
        littleLoad.SetActive(true);
        setMonedasText();
    }

    //Se setean las tarjetas de los campeonatos
    public void setTarjetasCampeonatos()
    {
        littleLoad.SetActive(false);
        //setear Tarjetas
        foreach (campeonatoAttribute champ in campeonatosManager.Instance.campeonatosData.campeonatosList)
        {
            GameObject newTarjeta = Instantiate(tarjetaPrefab, Vector3.zero, Quaternion.identity, tarjetasContent.transform);
            newTarjeta.GetComponent<tarjetaDePresentacion>().thisCampeonato = champ;
            newTarjeta.GetComponent<tarjetaDePresentacion>().setTarjeta();
        }
    }


    //El jugador se une al torneo
    public void onClickUnirse()
    {
        anim.ResetTrigger(parameterToCamp);
        anim.SetTrigger(parameterToChamp);
        panelChamp.Instance.setPanel();
        setMonedasText();
    }
    //Se vuelve a la pagina con todos los campeonatos
    public void returnToCampeonatos()
    {
        anim.ResetTrigger(parameterToChamp);
        anim.SetTrigger(parameterToCamp);
    }
    //Se retorna al menu principal
    public void onClickMenu()
    {
        audioManager.Instance.playSound("clickSelect");
        StartCoroutine(sceneLoader.Instance.loadScene(0));
    }

    //Se setea el texto de las monedas
    void setMonedasText()
    {
        monedasText.text = progressManager.Instance.progressData.totalPuntos.ToString();
    }
}
