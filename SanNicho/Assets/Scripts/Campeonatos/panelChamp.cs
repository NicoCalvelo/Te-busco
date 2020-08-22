using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <Documentacion>
/// Resumen:
///     Este script controla el panel que muestra un campeonato luego de unirse.
/// 
/// Creación:
///     17/08/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     17/08/2020 Calvelo Nicolás
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

    public campeonatoAttribute thisChamp;
    public List<campeonatosManager.userDisplay> posicionesList;

    string IDsetted;

    [SerializeField]
    RawImage logo;
    [SerializeField]
    TextMeshProUGUI nombreDeEmpresa, descripcion;
    [SerializeField]
    GameObject posicionPrefab, contentPosiciones, champLoad;


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

    //Se abre el perfil de instagram del patrocinio
    public void onClickInsta()
    {
        Application.OpenURL(instaPage);
    }
}
