using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <Documentacion>
/// Resumen:
///     Este script controla el prefab de slot en la tienda.
/// 
/// Creación:
///     01/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     01/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class slotPrefab : MonoBehaviour
{
    public shopItem thisItem;

    public GameObject mejorablePanel;

    public Image icon;
    public TextMeshProUGUI nombre, nivel, mensaje, costo, descripcion, mejorarText;
    public Image lvl2, lvl3, lvl4;

    Color mejorado = new Color(69, 69, 77, 255); 

    //Se setea el prefab
    public void setPrefab()
    {
        int itemLevle = progressManager.Instance.progressData.shopItems.Find(i => i.name == thisItem.name).nivel;
        icon.sprite = thisItem.icon;
        nombre.text = thisItem.itemName;

        if(thisItem.mejorable == true)
        {
            mejorablePanel.SetActive(true);
            nivel.text = "Nvl " + itemLevle.ToString();
            mejorarText.text = "mejorar";
            mensaje.text = thisItem.mensajeDeMejora;
            if (itemLevle > 1)
            {
                lvl2.color = mejorado;
                if (itemLevle > 2)
                {
                    lvl3.color = mejorado;
                    if (itemLevle > 3)
                    {
                        lvl3.color = mejorado;
                    }
                }
            }
        }
        else
        {
            mejorablePanel.SetActive(false);
            descripcion.enabled = true;
            descripcion.text = thisItem.descripcionCompra;
            mejorarText.text = "comprar";
        }
      
        costo.text = (thisItem.costoInicial + (thisItem.costoInicial * (thisItem.porcentajeAumentoCostoPorNivel * itemLevle))).ToString("F0");

    }

}
