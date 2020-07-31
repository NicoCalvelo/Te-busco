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
///     30/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class slotPrefab : MonoBehaviour
{
    public shopItem thisItem;

    public GameObject mejorablePanel;

    public Button mejorarBTN;
    public Image icon;
    public TextMeshProUGUI nombre, nivel, mensaje, costo, descripcion, mejorarText;
    public Image lvl2, lvl3, lvl4;

    Color mejorado = new Color32(69, 69, 77, 255);

    Animator anim;
    int parameterNo = Animator.StringToHash("dineroInsuficiente");
    int parameterYes = Animator.StringToHash("comprado");

    float costoDeMejora;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

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
                        lvl4.color = mejorado;
                    }
                }
            }

            if (itemLevel == thisItem.cantidadDeNiveles)
            {
                mejorarBTN.interactable = false;
                mejorarText.text = "maximo";
                mejorarText.alignment = TextAlignmentOptions.Midline;
                costo.transform.parent.gameObject.SetActive(false);
            }

        }
        else
        {
            mejorablePanel.SetActive(false);
            descripcion.enabled = true;
            descripcion.text = thisItem.descripcionCompra;
            mejorarText.text = "comprar";
        }
      

        costoDeMejora = (thisItem.costoInicial + (thisItem.costoInicial * thisItem.porcentajeAumentoCostoPorNivel) * itemLevel);
        costo.text = costoDeMejora.ToString("F0");
    }


    public void onClickMejorar()
    {
        if(progressManager.Instance.progressData.totalPuntos > costoDeMejora)
        {
            progressManager.Instance.progressData.totalPuntos -= Mathf.RoundToInt(costoDeMejora);
            progressManager.Instance.progressData.shopItems.Find(i => i.name == thisItem.itemName).nivel++;
            //Animacion de compra
            anim.SetTrigger(parameterYes);
            //Sonido de compra
            audioManager.Instance.playSound("comprado");


            setPrefab();

        }
        else
        {
            //Animacion de que no alcanza el dinero
            anim.SetTrigger(parameterNo);
            //Sonido de que falta dinero
            audioManager.Instance.playSound("insuficiente");

        }
        Invoke("resetTriggers", .5f);
    }

    void resetTriggers()
    {
        anim.ResetTrigger(parameterNo);
        anim.ResetTrigger(parameterYes);
    }
}
