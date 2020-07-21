using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <Documentacion>
/// Resumen:
///     Este script controla las interacciones del usuario con la escena.
/// 
/// Creación:
///     01/07/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     21/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class shopController : MonoBehaviour
{
    public float waitChar, waitAfterDot;

    public List<shopItem> itemsList;

    public GameObject slotPrefab, content;

    public TextMeshProUGUI dialog, monedasText;

    public string[] sentences;

    private void Start()
    {
        audioManager.Instance.changeBackgroundMusic(0);

        StartCoroutine(typeSentence(sentences[Random.Range(0, sentences.Length)]));

        monedasText.text = progressManager.Instance.progressData.totalPuntos.ToString("F0");

        foreach(shopItem item in itemsList)
        {
            if(progressManager.Instance.progressData.diasInfo[item.disponibleAPartirDelDia].completado == true)
            {
                GameObject newItem = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, content.transform);
                newItem.GetComponent<slotPrefab>().thisItem = item;
                newItem.GetComponent<slotPrefab>().setPrefab();
            }

        }
    }

    IEnumerator typeSentence(string sentece)
    {
        dialog.text = "";

        yield return new WaitForSeconds(2);

        foreach (char letter in sentece.ToCharArray())
        {
            dialog.text += letter;
            if (letter.ToString() == "." || letter.ToString() == "!" || letter.ToString() == "?")
                yield return new WaitForSeconds(waitAfterDot);
            else
            {
                FindObjectOfType<audioManager>().playSound("voice" + Random.Range(1, 9).ToString());
                yield return new WaitForSeconds(waitChar);
            }
        }
    }

    public void playSlideAudio()
    {
        audioManager.Instance.playSound("slideMenu");
    }

    public void onClickMenu()
    {
        FindObjectOfType<audioManager>().playSound("clickSelect");
        StartCoroutine(sceneLoader.Instance.loadScene(0));

    }
}
