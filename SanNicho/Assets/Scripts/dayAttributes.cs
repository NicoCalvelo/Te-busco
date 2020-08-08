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
///     03/08/2020 Calvelo Nicolás
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

    [Space]
    public bool heladero;
    public int heladosAgenerar;

    [Space]
    public bool elFamoso;
    public int pasadas;

    [Space]
    [Header("Npc_Complete")]
    public int inicioNpc_01;
    public int agregarNpc01Tarde, agregarNpc01Noche;
    [Range(0, 65)]
    public float NPC_01_visibilityDistance;
    [Range(1.5f, 3.0f)]
    public float NPC_01_timeToShoot;

    [Space]
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

    [Space]
    [Header("Npc_Camera")]
    [Range(0, 10)]
    public int cantidadNPC_03;
    [Range(0, 35)]
    public float NPC_03_attackDistance;
    [Range(1.0f, 3.0f)]
    public float NPC_03_timeToShoot;

    [Space]
    [Header("Npc_Drone")]
    [Range(0, 4)]
    public int cantidadNPC_04;
    public int agregarNpc04Tarde, agregarNpc04Noche;
    [Range(0, 55)]
    public float NPC_04_attackDistance;
    [Range(.7f, 2.0f)]
    public float NPC_04_timeToShoot;

    [Space]
    public int cantidadNPC_Background;

    [Space]
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
