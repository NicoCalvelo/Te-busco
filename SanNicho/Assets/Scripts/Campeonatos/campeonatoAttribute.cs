using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script contiene la informacion para crear un scriptable object
///     que sirve para setear un campeonato.
/// 
/// Creación:
///     10/08/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     11/08/2020 Calvelo Nicolás
///     
/// </Documentacion>

[CreateAssetMenu(fileName = "New Campeonato", menuName = "Campeonato")]
public class campeonatoAttribute : ScriptableObject
{
    public string campeonatoID;
    public int mesInicio, diaInicio, mesFinalizacion, diaFinalizacion;
    public string nombrePatrocinio, descripcionPatrocinio, instagramPatrocinio;
    public Texture logo, backgroundTarjeta;

    public enum campeonatoTipo { express, campeonato, megaCampeonato};
    public campeonatoTipo tipoDeCampeonato;
    public string[] premios;
}
