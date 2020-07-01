using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script contiene la informacion para crear un scriptable object
///     que se muestra en la tienda.
/// 
/// Creación:
///     01/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     01/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

[CreateAssetMenu(fileName = "New shop item", menuName = "Shop Item")]
public class shopItem : ScriptableObject
{
    public string itemName;
    public string mensajeDeMejora;
    public string descripcionCompra;
    public int disponibleAPartirDelDia;

    public Sprite icon;

    public bool mejorable;
    [Range(1, 4)]
    public int cantidadDeNiveles;

    public float costoInicial;
    [Range(0.0f, 2.0f)]
    public float porcentajeAumentoCostoPorNivel;
}
