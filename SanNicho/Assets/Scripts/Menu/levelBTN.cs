using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script controla los botones de los niveles.
/// 
/// Creación:
///     05/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     05/06/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class levelBTN : MonoBehaviour
{
    public int diaIndx;

    public void Start()
    {
        transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = (diaIndx + 1).ToString("00");
    }

    public void onClick()
    {
        levelsSceneController.Instance.setDia(diaIndx);
        FindObjectOfType<audioManager>().playSound("clickSelect");
    }
}
