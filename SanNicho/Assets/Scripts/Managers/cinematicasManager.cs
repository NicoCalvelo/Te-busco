using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de controlar las transiciones de las cinematicas.
/// 
/// Creación:
///     06/08/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     08/08//2020 Calvelo Nicolás
///     
/// </Documentacion>

public class cinematicasManager : MonoBehaviour
{
    public GameObject directorInicial, directorFinal;

    int sceneToChange;

    private void Start()
    {

        if (progressManager.Instance == null)
            return;

        //Se muestra la cinematica inicial
        if (progressManager.Instance.progressData.primeraVez)
        {
            directorInicial.SetActive(true);
            directorFinal.SetActive(false);
            sceneToChange = 1;
        }
        else //Cinematica Final
        {
            directorInicial.SetActive(false);
            directorFinal.SetActive(true);
            sceneToChange = 0;
        }
    }

    //Se cambia de escena
    public void changeScene()
    {
        Debug.Log("Cambiando escena");
        StartCoroutine(sceneLoader.Instance.loadScene(sceneToChange));
    }
}
