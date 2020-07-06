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
///     06/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class menuSceneController : MonoBehaviour
{

    [SerializeField]
    private Button torneoBtn, shopBtn;

    [SerializeField]
    GameObject torneoDialog, shopDialog, logroPrefab, logroContent, logrosPanel;

    private void Start()
    {
        FindObjectOfType<audioManager>().playSound("mainMusic");

        setMenu();
        setLogros();
    }

    void setMenu()
    {
        if (progressManager.Instance.progressData.diasInfo[9].completado == true) //Significa que ya se desbloquearon los torneos
        {
            torneoBtn.interactable = true;
            torneoBtn.transform.GetChild(1).gameObject.SetActive(false);
        }

        if(progressManager.Instance.progressData.diasInfo[4].completado == true)
        {
            shopBtn.interactable = true;
            shopBtn.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    void setLogros()
    {
        logrosPanel.SetActive(true);
        foreach(infoJugador.logro logro in progressManager.Instance.progressData.logros)
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
        StartCoroutine(sceneLoader.Instance.loadScene(1));
    }
    public void onclickTorneo()
    {

        if(progressManager.Instance.progressData.diasInfo[9].completado == false)
        {
            FindObjectOfType<audioManager>().playSound("click");
            torneoDialog.SetActive(!torneoDialog.activeSelf);
            shopDialog.SetActive(false);
        }
        else
        {
            //Transicion a la escena de torneos
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

}
