using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de los eventos del jugador independientes del escenario. Tambien
///     se encarga de setear el UI acorde se avanza en el nivel.
/// 
/// Creación:
///     12/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     14/06//2020 Calvelo Nicolás
///     
/// </Documentacion>

public class gameManager : MonoBehaviour
{
    #region singleton
    private static gameManager _instance;
    public static gameManager Instance
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

    [Header("----Player-Stats----")]
    public int starsLeft = 3;

    [Header("----Game-UI----")]
    [SerializeField]
    private Animator canvasAnim;
    [SerializeField]
    private TextMeshProUGUI diaText, horaText, minutosText;
    [SerializeField]
    private CanvasGroup movementBTNs;
    [SerializeField]
    private GameObject pausePanel;

    int hora = 8;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        float invokeTime = (progressManager.Instance.nextDayAttribute.duracionDelDia * 60) / 32;
        InvokeRepeating("setTime", invokeTime, invokeTime);

        //agregar un intento de nivel al progress manager
        progressManager.Instance.progressData.diasInfo[progressManager.Instance.nextDayAttribute.diaNumero].intentos++;
    }

    //Se ejecuta repetidamente cada vez que pasan 30 minutos en tiempo del juego
    void setTime()
    {
        if(minutosText.text == "00")
        {
            minutosText.text = "30";
            canvasAnim.ResetTrigger("hora");
            canvasAnim.SetTrigger("minutes");
            audioManager.Instance.playSound("zipClick");

        }
        else
        {
            minutosText.text = "00";
            canvasAnim.ResetTrigger("minutes");
            canvasAnim.SetTrigger("hora");
            hora++;
            horaText.text = hora.ToString("00");
            audioManager.Instance.playSound("hourPass");
        }


        //a determinada hora cambiar el momento del dia en la escena
        if(horaText.text == "12")
        {
            //Se cambia a despues del medio dia
        }
        else if(horaText.text == "16")
        {
            //Se cambia a tarde
        }
        else if(horaText.text == "20")
        {
            //Se cambia a noche
        }
        else if(horaText.text == "24")
        {
            //se termina el juego
            horaText.text = "00";
            levelCompeted();
        }
    }

    public void setStars(int adicion)
    {
        starsLeft += adicion;
        canvasAnim.SetInteger("starsLeft", starsLeft);

        if (adicion < 0)
            audioManager.Instance.playSound("loseStar");


        if(starsLeft == 0)
        {
            lose();
        }
    }

    //Cuando al jugador se le acaban las estrellas
    void lose()
    {
        audioManager.Instance.stopSound("backgroundMusic");
        Time.timeScale = 0;
        audioManager.Instance.playSound("levelLose");
        canvasAnim.SetTrigger("levelLose");
    }

    //Cuando el jugador llega a la hora 24
    void levelCompeted()
    {
        audioManager.Instance.stopSound("backgroundMusic");
        Time.timeScale = 0;
        audioManager.Instance.playSound("levelCompleted");
        canvasAnim.SetTrigger("dayCompleted");
    }

    public void pauseGame()
    {
        audioManager.Instance.playSound("click01");
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        movementBTNs.alpha = 0;
        movementBTNs.interactable = false;
    }
    public void resumeGame()
    {
        audioManager.Instance.playSound("click01");
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        movementBTNs.alpha = 1;
        movementBTNs.interactable = true;
    }
    public void jugarDeNuevo()
    {
        audioManager.Instance.playSound("click02");
        Time.timeScale = 1;
        StartCoroutine(sceneLoader.Instance.loadScene(sceneLoader.Instance.indexCostanera));
    }
    public void jugarSiguienteDia()
    {
        audioManager.Instance.playSound("click02");
        Time.timeScale = 1;

        progressManager.Instance.nextDayAttribute = progressManager.Instance.daysAttributes[progressManager.Instance.nextDayAttribute.diaNumero];
        StartCoroutine(sceneLoader.Instance.loadScene(sceneLoader.Instance.indexCostanera));
    }
    public void menu()
    {
        audioManager.Instance.playSound("click03");
        Time.timeScale = 1;
        StartCoroutine(sceneLoader.Instance.loadScene(0));
    }
}
