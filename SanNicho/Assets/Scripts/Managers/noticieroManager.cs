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
///     06/06//2020 Calvelo Nicolás
///     
/// </Documentacion>

public class noticieroManager : MonoBehaviour
{
    public Image playImg;
    public Button playBTN;

    private void Start()
    {
        FindObjectOfType<audioManager>().playSound("mainMusic");
        StartCoroutine(sceneLoader.Instance.loadSceneAsync());
    }

}
