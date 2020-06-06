using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script controla las interacciones del usuario con la escena.
/// 
/// Creación:
///     05/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     05/06/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class menuSceneController : MonoBehaviour
{

    private void Start()
    {
        FindObjectOfType<audioManager>().playSound("mainMusic");
    }

    public void onClickPlayBTN()
    {
        FindObjectOfType<audioManager>().playSound("confirmClick");
        StartCoroutine(sceneLoader.Instance.loadScene(1));
    }
}
