using System.Collections.Generic;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script especifica la informacion que sera guradada del jugador.
/// 
/// Creación:
///     05/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     10/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

[System.Serializable]
public class infoJugador 
{
    public bool primeraVez = true;

    public int totalEstrellas = 0;
    public int totalPuntos = 0;

    public List<item> shopItems;

    public Dictionary<int, nivel> diasInfo;

    public List<logro> logros;

    [System.Serializable]
    public class nivel
    {
        public bool completado = false;
        public int estrellas = 0;
        public int intentos = 0;
    }

    [System.Serializable]
    public class item
    {
        public string name;
        public int nivel = 1;
    }

    [System.Serializable]
    public class logro
    {
        public string titulo, descripcion;
        public bool completado = false, reclamado = false;
        [Range(0.0f, 1.0f)]
        public float porcentajeCompletado;

        public int monedasDeRecompensa;
    }
}
