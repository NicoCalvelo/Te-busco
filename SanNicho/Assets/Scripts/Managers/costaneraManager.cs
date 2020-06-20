using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de controlar la escena acorde al nivel del dia que se esta jugando y a los eventos
///     enviados por el gameManager.
/// 
/// Creación:
///     07/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     20/06/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class costaneraManager : MonoBehaviour
{

    public GameObject[] nubePrefabs;

    void Start()
    {
        FindObjectOfType<audioManager>().playSound("backgroundMusic");
        setScene();
    }

    void setScene()
    {

        //Nubes
        int cantidadDeNubes = Random.Range(7, 15);

        for (int i = 0; i <= cantidadDeNubes; i++)
        {
            Vector3 pos = new Vector3 (Random.Range(npcManager.Instance.sceneLimitLeft, npcManager.Instance.sceneLimitRigth), Random.Range(55.0f, 65.0f), Random.Range(4.0f, 6.0f));
            Instantiate(nubePrefabs[Random.Range(0, nubePrefabs.Length)], pos, Quaternion.identity, transform);
        }

    }
}
