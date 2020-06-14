using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script contiene la informacion para crear un scriptable object
///     que sirve para setear la dificultad del dia que se esta jugando.
/// 
/// Creación:
///     05/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     14/06/2020 Calvelo Nicolás
///     
/// </Documentacion>

[CreateAssetMenu(fileName = "New Day", menuName = "Day Attribute")]
public class dayAttributes : ScriptableObject
{

    public float duracionDelDia;
    public int diaNumero, cantidadNpc_01;


    public noticia[] noticiasList;

    [System.Serializable]
    public class noticia
    {
        public Sprite tvImage;

        [TextArea(3, 10)]
        public string noticiaSentence;
    }

}
