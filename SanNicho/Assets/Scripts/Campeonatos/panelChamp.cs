using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <Documentacion>
/// Resumen:
///     Este script controla el panel que muestra un campeonato luego de unirse.
/// 
/// Creación:
///     17/08/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     23/08/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class panelChamp : MonoBehaviour
{
    #region singleton
    private static panelChamp _instance;
    public static panelChamp Instance
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
    private void Awake()
    {
        _instance = this;
    }
    #endregion

    [SerializeField]
    dayAttributes champDayAtributte;

    public campeonatoAttribute thisChamp;
    public List<campeonatosManager.userDisplay> posicionesList;
    public campeonatosManager.userProgress thisProgress;

    string IDsetted;

    [SerializeField]
    RawImage logo;
    [SerializeField]
    TextMeshProUGUI nombreDeEmpresa, descripcion;
    [SerializeField]
    GameObject posicionPrefab, contentPosiciones, champLoad;

    [Header("Progress Panel")]
    [SerializeField]
    private TextMeshProUGUI nombreDeUsuario;
    [SerializeField]
    private TextMeshProUGUI diasActuales, puntosActuales, maximoDias, maximoPuntos;

    [Header("Premios Panel")]
    [SerializeField]
    private GameObject placeHolderPremios;
    [SerializeField]
    private GameObject contentPremios;

    string instaPage;

    //Se setea el panel una vez que se clickea unirse
    public void setPanel()
    {
        if (thisChamp.campeonatoID == IDsetted)
            return;

        //====Strings====
        nombreDeEmpresa.text = thisChamp.nombrePatrocinio;
        descripcion.text = thisChamp.descripcionPatrocinio;

        IDsetted = thisChamp.campeonatoID;
        instaPage = thisChamp.instagramPatrocinio;

        //====Logo====
        logo.texture = thisChamp.logo;

        //====Progreso=Personal====
        setProgressPanel();

        //====Premios====
        setPremiosPanel();

        //====Tabla=De=Posiciones====
        if (campeonatosManager.Instance.campeonatosData.tablaDePosiciones.ContainsKey(thisChamp.campeonatoID) == false) //Se descarga la tabla de posiciones
        {
            descargarTablaPosiciones();
        }
        else
        {
            posicionesList = campeonatosManager.Instance.campeonatosData.tablaDePosiciones[thisChamp.campeonatoID];
            setTablaPosiciones();
        }
    }

    void descargarTablaPosiciones()
    {
        //Se activa el logo de carga
        champLoad.SetActive(true);

        //Se destruyen las posiciones viejas
        GameObject[] oldPos = contentPosiciones.GetComponentsInChildren<GameObject>();
        foreach (GameObject g in oldPos)
        {
            Destroy(g);
        }

        campeonatosManager.Instance.descargarTabalDePosiciones(thisChamp.campeonatoID, (List<campeonatosManager.userDisplay> list) => 
        {
            if(list == null) //Todavia no hay posiciones
            {
                posicionesList = new List<campeonatosManager.userDisplay>();
            }
            posicionesList = list;
        });

        setTablaPosiciones();
        champLoad.SetActive(false);
    }
    void setTablaPosiciones()
    {
        for (int i = 0; i < 10 || i < posicionesList.Count; i++)
        {
            GameObject newPos = Instantiate(posicionPrefab, Vector3.zero, Quaternion.identity, contentPosiciones.transform);
            newPos.GetComponent<posicionPrefab>().thisUser = posicionesList[i];
            newPos.GetComponent<posicionPrefab>().setPrefab(i + 1);
        }
    }

    //Se setea el progreso del jugador acorde al torneo actual
    void setProgressPanel()
    {
        if(thisProgress == null)
        {
            thisProgress = new campeonatosManager.userProgress();
            thisProgress.diaActual = 1;
            thisProgress.puntosActuales = 0;
            thisProgress.cantidadMaximaDeDias = 0;
            thisProgress.cantidadMaximaDePuntos = 0;
            campeonatosManager.Instance.campeonatosData.userCampeonatoProgress.Add(thisChamp.campeonatoID, thisProgress);
        }
        nombreDeUsuario.text = campeonatosManager.Instance.campeonatosData.nombreDeUsuario;
        diasActuales.text = "Día actual:  " + thisProgress.diaActual.ToString();
        puntosActuales.text = "Puntos actuales:  " + thisProgress.puntosActuales.ToString();
        maximoDias.text = "Maximo de días:  " + thisProgress.cantidadMaximaDeDias.ToString();
        maximoPuntos.text = "Maximo de puntos:  " + thisProgress.cantidadMaximaDePuntos.ToString();
    }
    //Se setea el panel de premios acorde a los premiso del torneo
    void setPremiosPanel()
    {
        //Se destruyen los premios viejos
        GameObject[] oldPremios = contentPremios.GetComponentsInChildren<GameObject>();
        foreach (GameObject g in oldPremios)
        {
            Destroy(g);
        }

        //Se setean los nuevos premios
        for (int i = 0; i < thisChamp.premios.Length; i++)
        {
            GameObject newPremio = Instantiate(placeHolderPremios, Vector3.zero, Quaternion.identity, contentPremios.transform);
            newPremio.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            newPremio.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = thisChamp.premios[i];
        }
    }

    //Cuando se quiere jugar el dia de un torneo
    public void onClickJugar()
    {
        dayAttributes nextDay = champDayAtributte;
        nextDay.duracionDelDia += thisProgress.diaActual * .1f;
        nextDay.diaNumero = thisProgress.diaActual;
        //Se setea el progressManager
        progressManager.Instance.nextDayAttribute = nextDay;
        progressManager.Instance.nextModoDeJuego = progressManager.modoDeJuego.campeonato;
        //Se cambia de escena
        StartCoroutine(sceneLoader.Instance.loadScene(3));
    }

    //Se abre el perfil de instagram del patrocinio
    public void onClickInsta()
    {
        Application.OpenURL(instaPage);
    }
}
