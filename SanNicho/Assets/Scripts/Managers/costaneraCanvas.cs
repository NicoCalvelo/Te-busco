using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <Documentacion>
/// Resumen:
///     Este script se encarga controlar el UI.
/// 
/// Creación:
///     27/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     29/06//2020 Calvelo Nicolás
///     
/// </Documentacion>

public class costaneraCanvas : MonoBehaviour
{
    #region singleton
    private static costaneraCanvas _instance;
    public static costaneraCanvas Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("The Manager is NULL");
            }

            return _instance;
        }
    }
    #endregion

    int starAudio = 1;

    public TextMeshProUGUI monedasTotales, monedasGanadas;

    int coinsEarned = 0;



    [SerializeField]
    private GameObject bubblePrefab, chiclePrefab;
    [SerializeField]
    private Transform collectables_T;

    float bubbleTimeLeft = 0, chicleTimeLeft = 0;   

    private void Awake()
    {
        _instance = this;
    }

    public void playStarAudio()
    {
        audioManager.Instance.playSound("Rise" + starAudio.ToString("00"));
        starAudio++;
    }
    public void levelCompleted(int earned)
    {
        monedasTotales.text = progressManager.Instance.progressData.totalPuntos.ToString();
        coinsEarned = earned;
        monedasGanadas.text = "+  " + coinsEarned.ToString();
    } 
    public void setCoins()
    {
        audioManager.Instance.playSound("Rise02");
        progressManager.Instance.progressData.totalPuntos += 2;
        coinsEarned -= 2;
        monedasTotales.text = progressManager.Instance.progressData.totalPuntos.ToString();
        monedasGanadas.text = "+  " + coinsEarned.ToString();
        if (coinsEarned <= 0)
        {
            progressManager.Instance.progressData.totalPuntos += coinsEarned;
            monedasTotales.text = progressManager.Instance.progressData.totalPuntos.ToString();
            GetComponent<Animator>().SetTrigger("stopEarning");
            monedasGanadas.enabled = false;
        }
    }


    public void agregarCollectable(gameManager.collectables item)
    {
        if(item == gameManager.collectables.Burbuja)
        {
            if(bubbleTimeLeft > 0)
            {
                bubbleTimeLeft = 1;
                return;
            }
            GameObject newItem = Instantiate(bubblePrefab, Vector3.zero, Quaternion.identity, collectables_T);
            StartCoroutine(consumeBubble(newItem, gameManager.Instance.bubbleDefaultTime + ((gameManager.Instance.bubbleDefaultTime / 4) * progressManager.Instance.progressData.shopItems.Find(i => i.name == "burbuja").nivel)));
        }
        else if(item == gameManager.collectables.Chicle)
        {
            if (chicleTimeLeft > 0)
            {
                chicleTimeLeft = 1;
                return;
            }
            GameObject newItem = Instantiate(chiclePrefab, Vector3.zero, Quaternion.identity, collectables_T);
            StartCoroutine(consumeChicle(newItem, gameManager.Instance.chicleDefaultTime - ((gameManager.Instance.chicleDefaultTime * .15f) * progressManager.Instance.progressData.shopItems.Find(i => i.name == "chicle").nivel)));

        }
    }

    IEnumerator consumeBubble(GameObject item, float seconds)
    {
        Image fill = item.GetComponent<Image>();

        for(bubbleTimeLeft = 1; bubbleTimeLeft >= 0.00f; bubbleTimeLeft -= 0.01f)
        {
            fill.fillAmount = bubbleTimeLeft;
            playerAnimController.Instance.setBubble(bubbleTimeLeft);

            if (bubbleTimeLeft <= 0.01f)
            {
                gameManager.Instance.bubbleShield.SetActive(false);
                audioManager.Instance.playSound("destroyBubble");
                Destroy(item);
            }
            yield return new WaitForSeconds(seconds * .01f);
        }
    }
    IEnumerator consumeChicle(GameObject item, float seconds)
    {
        Image fill = item.GetComponent<Image>();

        for (chicleTimeLeft = 1; chicleTimeLeft >= 0.00f; chicleTimeLeft -= 0.01f)
        {
            fill.fillAmount = chicleTimeLeft;


            if (chicleTimeLeft <= 0.01f)
            {
                FindObjectOfType<playerController>().onGetChicle(false);
                audioManager.Instance.stopSound("chicloso");
                Destroy(item);
            }
            yield return new WaitForSeconds(seconds * .01f);
        }
    }

}
