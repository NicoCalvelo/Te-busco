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
///     18/06/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class menuSceneController : MonoBehaviour
{

    [SerializeField]
    private Button torneoBtn;

    [SerializeField]
    GameObject torneoDialog;

    private void Start()
    {
        FindObjectOfType<audioManager>().playSound("mainMusic");

        setMenu();
    }

    void setMenu()
    {
        if (progressManager.Instance.progressData.diasInfo[8].completado == true) //Significa que ya se desbloquearon los torneos
        {
            torneoBtn.interactable = true;
            torneoBtn.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void onClickPlayBTN()
    {
        FindObjectOfType<audioManager>().playSound("confirmClick");
        StartCoroutine(sceneLoader.Instance.loadScene(1));
    }

    public void onclickTorneo()
    {

        if(progressManager.Instance.progressData.diasInfo[9].intentos < 1)
        {
            FindObjectOfType<audioManager>().playSound("click");
            torneoDialog.SetActive(!torneoDialog.activeSelf);
        }
        else
        {
            //Transicion a la escena de torneos
        }
    }

}
