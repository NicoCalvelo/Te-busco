using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

/// <Documentacion>
/// Resumen:
///     Este script controla las interacciones del usuario con la escena.
/// 
/// Creación:
///     05/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     17/06//2020 Calvelo Nicolás
///     
/// </Documentacion>

public class levelsSceneController : MonoBehaviour
{
    #region singleton
    private static levelsSceneController _instance;
    public static levelsSceneController Instance
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
    infoJugador.nivel nivelSelected;

    public GameObject grillaContent, diaPrefab, diaBloqPrefab, infoPanel;

    [Header("UI Elements")]
    public TextMeshProUGUI diaText, intentosText;


    DateTime datevalue1, datevalue2;
    TextMeshProUGUI cronometer;

    private void Awake()
    {
        _instance = this;
        setPage();
    }

    private void Start()
    {
        FindObjectOfType<audioManager>().playSound("mainMusic");
    }

    void setPage()
    {
        bool bloqueado = false;
        bool setHabilitado = false;

        for (int i = 0; i < 25; i++)
        {
            if(progressManager.Instance.daysAttributes[i].habilitado == false && setHabilitado == true)
                bloqueado = true;


            if (bloqueado == false)
            {
                GameObject d = Instantiate(diaPrefab, Vector3.zero, Quaternion.identity, grillaContent.transform);
                d.GetComponent<levelBTN>().diaIndx = i;
                d.GetComponent<levelBTN>().setBTN();
                if (progressManager.Instance.daysAttributes[i + 1].habilitado == false)
                {
                    setHabilitado = true;
                    bloqueado = true;
                    setDia(i);
                }

                if (progressManager.Instance.progressData.diasInfo[i + 1].completado == false && setHabilitado == false)
                {
                    bloqueado = true;
                    setDia(i);
                }

            }
            else
            {

                GameObject d = Instantiate(diaBloqPrefab, Vector3.zero, Quaternion.identity, grillaContent.transform);
                if( setHabilitado == true)
                {
                    d.transform.Find("Dialog").gameObject.SetActive(true);
                    cronometer = d.transform.Find("Dialog").gameObject.GetComponentInChildren<TextMeshProUGUI>();
                    int day = DateTime.Today.Day + 1;
                    if(DateTime.Now.Hour < 8)
                    {
                        day--;
                    }
                    datevalue1 = new DateTime(2020, DateTime.Now.Month, day, 08, 0, 0);
                    setHabilitado = false;
                    InvokeRepeating("setCronometer",0, 1);
                }
                d.GetComponent<levelBTN>().diaIndx = i;
                d.GetComponent<Button>().interactable = false;
            }
        }
    }

    void setCronometer()
    {
        datevalue2 = DateTime.Now;
        TimeSpan timeDifference = datevalue1 - datevalue2;
        string niceTime = string.Format("{0:00}:{1:00}:{2:00}", timeDifference.Hours, timeDifference.Minutes, timeDifference.Seconds);
        cronometer.text = "Disponible en: " + niceTime;
    }

    public void setDia(int lvlIndx)
    {
        nivelSelected = progressManager.Instance.progressData.diasInfo[lvlIndx + 1];
        diaText.text = (lvlIndx + 1).ToString("00");

        infoPanel.SetActive(false);

        progressManager.Instance.nextDayAttribute = progressManager.Instance.daysAttributes[lvlIndx];
    }

    public void onClickMenu()
    {
        FindObjectOfType<audioManager>().playSound("clickSelect");
        StartCoroutine(sceneLoader.Instance.loadScene(0));

    }

    public void onClickJugar()
    {
        FindObjectOfType<audioManager>().playSound("clickConfirm");
        StartCoroutine(sceneLoader.Instance.loadScene(sceneLoader.Instance.indxNoticiero));
    }

    public void onClickInfo()
    {
        infoPanel.SetActive(!infoPanel.activeSelf);
        intentosText.text = "Intentos: " + nivelSelected.intentos.ToString();
        FindObjectOfType<audioManager>().playSound("clickSelect");
    }
}
