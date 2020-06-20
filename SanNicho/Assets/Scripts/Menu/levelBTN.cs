using UnityEngine;
using UnityEngine.UI;

/// <Documentacion>
/// Resumen:
///     Este script controla los botones de los niveles.
/// 
/// Creación:
///     05/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     17/06/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class levelBTN : MonoBehaviour
{
    public int diaIndx;
    public Image star1, star2, star3;

    public Sprite star;

    public void Start()
    {
        transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = (diaIndx + 1).ToString("00");

        if(progressManager.Instance.progressData.diasInfo[diaIndx + 1].estrellas > 0)
        {
            star1.sprite = star;
            if (progressManager.Instance.progressData.diasInfo[diaIndx + 1].estrellas > 1)
            {
                star2.sprite = star;
                if (progressManager.Instance.progressData.diasInfo[diaIndx + 1].estrellas > 2)
                {
                    star3.sprite = star;
                }
            }
        }
    }

    public void onClick()
    {
        levelsSceneController.Instance.setDia(diaIndx);
        FindObjectOfType<audioManager>().playSound("clickSelect");
    }
}
