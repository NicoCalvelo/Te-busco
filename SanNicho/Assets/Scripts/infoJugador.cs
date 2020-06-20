using System.Collections.Generic;

/// <Documentacion>
/// Resumen:
///     Este script especifica la informacion que sera guradada del jugador.
/// 
/// Creación:
///     05/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     17/06/2020 Calvelo Nicolás
///     
/// </Documentacion>

[System.Serializable]
public class infoJugador 
{
    public bool primeraVez = true;

    public int totalEstrellas = 0;

    public Dictionary<int, nivel> diasInfo;

    [System.Serializable]
    public class nivel
    {
        public bool completado = false;
        public int estrellas = 0;
        public int intentos = 0;
    }
}
