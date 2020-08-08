using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de controlar la seccion de campeonatos y setearlos correctamente.
/// 
/// Creación:
///     06/08/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     08/08//2020 Calvelo Nicolás
///     
/// </Documentacion>

public class campeonatosManager : MonoBehaviour
{



    //Se retorna al menu principal
    public void onClickMenu()
    {
        FindObjectOfType<audioManager>().playSound("clickSelect");
        StartCoroutine(sceneLoader.Instance.loadScene(0));
    }


}
