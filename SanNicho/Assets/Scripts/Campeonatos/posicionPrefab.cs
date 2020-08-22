using TMPro;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script controla el display de una posicion en la tabla de posiciones
///     en los campeonatos.
/// 
/// Creación:
///     17/08/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     17/08/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class posicionPrefab : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI posicion, nombreDeUsuario, dias, puntaje;

    public campeonatosManager.userDisplay thisUser;


    //Se setea el prefab de acorde a los valores
    public void setPrefab(int posicionIndx)
    {
        posicion.text = posicionIndx.ToString();
        nombreDeUsuario.text = thisUser.nombreDelUsuario;
        dias.text = this.dias.ToString();
        puntaje.text = this.puntaje.ToString();
    }


}
