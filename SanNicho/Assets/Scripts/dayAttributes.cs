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
///     21/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

[CreateAssetMenu(fileName = "New Day", menuName = "Day Attribute")]
public class dayAttributes : ScriptableObject
{

    public float duracionDelDia;
    public int diaNumero;
    public bool habilitado = false;

    [Header("Shoots")]
    [Range(0.0f, 7.0f)]
    public float shootLife = 5.0f;

    [Header("Npc_Complete")]
    public int inicioNpc_01;
    public int agregarNpc01Tarde, agregarNpc01Noche;
    [Range(45, 65)]
    public float NPC_01_visibilityDistance;
    [Range(1.5f, 3.0f)]
    public float NPC_01_timeToShoot;

    [Header("Npc_Static")]
    [Range(0, 7)]
    public int cantidadNPC_02;
    public int agregarNpc02Tarde, agregarNpc02Noche;
    [Range(0.1f, 2.0f)]
    public float NPC_02_hideTime;
    [Range(15, 35)]
    public float NPC_02_attackDistance;
    [Range(1.0f, 3.0f)]
    public float NPC_02_timeToShoot;

    [Header("Npc_Camera")]
    [Range(0, 10)]
    public int cantidadNPC_03;
    [Range(15, 35)]
    public float NPC_03_attackDistance;
    [Range(1.0f, 3.0f)]
    public float NPC_03_timeToShoot;

    [Header("Collectables")]
    [Range(0, 24)]
    public int bubblesToSpawn;
    [Range(0, 12)]
    public int chiclesToSpawn;

    public noticia[] noticiasList;

    [System.Serializable]
    public class noticia
    {
        public Sprite tvImage;

        [TextArea(3, 10)]
        public string noticiaSentence;
    }

}
