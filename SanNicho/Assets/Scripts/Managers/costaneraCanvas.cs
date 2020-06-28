using UnityEngine;
using TMPro;
using System.Collections;

/// <Documentacion>
/// Resumen:
///     Este script se encarga controlar el UI.
/// 
/// Creación:
///     27/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     27/06//2020 Calvelo Nicolás
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
        progressManager.Instance.progressData.totalPuntos += 12;
        coinsEarned -= 12;
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

}
