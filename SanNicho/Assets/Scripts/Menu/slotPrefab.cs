using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <Documentacion>
/// Resumen:
///     Este script controla el prefab de slot en la tienda.
/// 
/// Creación:
///     01/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     21/07/2020 Calvelo Nicolás
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
        int itemLevel = progressManager.Instance.progressData.shopItems.Find(i => i.name == thisItem.itemName).nivel;
        icon.sprite = thisItem.icon;
        nombre.text = thisItem.itemName;

        if(thisItem.mejorable == true)
        {
            mejorablePanel.SetActive(true);
            nivel.text = "Nvl " + itemLevel.ToString();
            mejorarText.text = "mejorar";
            mensaje.text = thisItem.mensajeDeMejora;
            if (itemLevel > 1)
            {
                lvl2.color = mejorado;
                if (itemLevel > 2)
                {
                    lvl3.color = mejorado;
                    if (itemLevel > 3)
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
      
        costo.text = (thisItem.costoInicial + (thisItem.costoInicial * (thisItem.porcentajeAumentoCostoPorNivel * itemLevel))).ToString("F0");

    }

}
