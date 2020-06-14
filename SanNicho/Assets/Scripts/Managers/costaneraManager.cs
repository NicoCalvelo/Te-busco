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
///     14/06/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class costaneraManager : MonoBehaviour
{



    void Start()
    {
        FindObjectOfType<audioManager>().playSound("backgroundMusic");
    }


}
