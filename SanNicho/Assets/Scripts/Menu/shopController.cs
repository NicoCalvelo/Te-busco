using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script controla las interacciones del usuario con la escena.
/// 
/// Creación:
///     01/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     01/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class shopController : MonoBehaviour
{
    public List<shopItem> itemsList;

    public GameObject slotPrefab, content;

    private void Start()
    {
        audioManager.Instance.changeBackgroundMusic(0);

        foreach(shopItem item in itemsList)
        {
            if(progressManager.Instance.progressData.diasInfo[item.disponibleAPartirDelDia].completado == true)
            {
                GameObject newItem = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, content.transform);
            }

        }
    }

    public void playSlideAudio()
    {
        audioManager.Instance.playSound("slideMenu");
    }
}
