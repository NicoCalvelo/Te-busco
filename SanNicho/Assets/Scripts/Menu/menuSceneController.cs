using UnityEngine;
using UnityEngine.UI;

/// <Documentacion>
/// Resumen:
///     Este script controla las interacciones del usuario con la escena.
/// 
/// Creación:
///     05/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     14/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class menuSceneController : MonoBehaviour
{

    [SerializeField]
    private Button torneoBtn, shopBtn;

    [SerializeField]
    GameObject torneoDialog, shopDialog, logroPrefab, logroContent, logrosPanel;

    bool campeonatosHabilitados;
    string mensajeCampeonatos;

    private void Start()
    {
        FindObjectOfType<audioManager>().playSound("mainMusic");

        campeonatosHabilitados = RemoteSettings.GetBool("campeonatosHabilitados");
        mensajeCampeonatos = RemoteSettings.GetString("mensajeCampeonatosDeshabilitados");

        setMenu();
        setLogros();
    }

    void setMenu()
    {
        torneoBtn.interactable = campeonatosHabilitados;
        torneoBtn.transform.GetChild(0).gameObject.SetActive(!campeonatosHabilitados);
        torneoDialog.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = mensajeCampeonatos;

        if(progressManager.Instance.progressData.diasInfo[4].completado == true)
        {
            shopBtn.interactable = true;
            shopBtn.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    void setLogros()
    {
        logrosPanel.SetActive(true);
        foreach (infoJugador.logro logro in progressManager.Instance.progressData.logros)
        {
            GameObject newLogro = Instantiate(logroPrefab, Vector2.zero, Quaternion.identity, logroContent.transform);

            newLogro.GetComponent<logroPrefab>().thisLogro = logro;
            newLogro.GetComponent<logroPrefab>().setPrefab();
        }
        logrosPanel.SetActive(false);
    }

    public void onClickPlayBTN()
    {
        FindObjectOfType<audioManager>().playSound("confirmClick");

        if(progressManager.Instance.progressData.primeraVez == true)
        {
            StartCoroutine(sceneLoader.Instance.loadScene(6));
            return;
        }

        StartCoroutine(sceneLoader.Instance.loadScene(1));
    }
    public void onclickTorneo()
    {
        torneoDialog.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = mensajeCampeonatos;
        if (campeonatosHabilitados == false)
        {
            FindObjectOfType<audioManager>().playSound("click");
            torneoDialog.SetActive(!torneoDialog.activeSelf);
            shopDialog.SetActive(false);
        }
        else
        {
            FindObjectOfType<audioManager>().playSound("confirmClick");
            StartCoroutine(sceneLoader.Instance.loadScene(5));
        }
    }
    public void onClickTienda()
    {
        if(progressManager.Instance.progressData.diasInfo[4].completado == false)
        {
            FindObjectOfType<audioManager>().playSound("click");
            shopDialog.SetActive(!shopDialog.activeSelf);
            torneoDialog.SetActive(false);
        }
        else
        {
            FindObjectOfType<audioManager>().playSound("confirmClick");
            StartCoroutine(sceneLoader.Instance.loadScene(4));
        }

    }

    public void onClickLogros()
    {
        logrosPanel.SetActive(!logrosPanel.activeSelf);
        FindObjectOfType<audioManager>().playSound("click");
    }
    public void onClickInsta(string instagram)
    {
        if(instagram != "")
        Application.OpenURL(instagram);
    }

    public void habilitarCinematica()
    {
        progressManager.Instance.progressData.primeraVez = true;
    }
}
