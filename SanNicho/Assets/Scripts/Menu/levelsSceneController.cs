using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <Documentacion>
/// Resumen:
///     Este script controla las interacciones del usuario con la escena.
/// 
/// Creación:
///     05/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     14/06//2020 Calvelo Nicolás
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

    public GameObject grillaContent, diaPrefab, diaBloqPrefab;

    [Header("UI Elements")]
    public TextMeshProUGUI diaText;

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

        for (int i = 0; i < 25; i++)
        {
            if (bloqueado == false)
            {
                GameObject d = Instantiate(diaPrefab, Vector3.zero, Quaternion.identity, grillaContent.transform);
                d.GetComponent<levelBTN>().diaIndx = i;

                if (progressManager.Instance.progressData.diasInfo[i + 1].completado == false)
                {
                    bloqueado = true;
                    setDia(i);
                }

            }
            else
            {
                GameObject d = Instantiate(diaBloqPrefab, Vector3.zero, Quaternion.identity, grillaContent.transform);
                d.GetComponent<levelBTN>().diaIndx = i;
                d.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void setDia(int lvlIndx)
    {
        nivelSelected = progressManager.Instance.progressData.diasInfo[lvlIndx + 1];
        diaText.text = (lvlIndx + 1).ToString("00");


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
}
