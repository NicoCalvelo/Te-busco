using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de los eventos del jugador independientes del escenario. Tambien
///     se encarga de setear el UI acorde se avanza en el nivel.
/// 
/// Creación:
///     12/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     21/07//2020 Calvelo Nicolás
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

    public float sceneLimitLeft = -336.0f, sceneLimitRigth = 314.0f;
    [HideInInspector]
    public Transform playerTransfrorm;


    [Header("----Player-Stats----")]
    public int starsLeft = 3;
    public float bubbleDefaultTime = 6.0f, chicleDefaultTime = 15.0f;

    [Header("----Collectables----")]
    public GameObject bublePrefab;
    public GameObject chiclePrefab;

    [Header("----Game-UI----")]
    [SerializeField]
    private Animator canvasAnim;
    [SerializeField]
    private TextMeshProUGUI diaText, horaText, minutosText;
    [SerializeField]
    private CanvasGroup movementBTNs;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private Button siguienteDiaBTN;

    public int hora = 8;

    bool isTransition = false;

    private void Awake()
    {
        _instance = this;
        itemsToSpawn = new List<GameObject>();
        playerTransfrorm = GameObject.FindGameObjectWithTag("Player").transform;

        diaText.text = progressManager.Instance.nextDayAttribute.diaNumero.ToString("00");

        for (int i = 0; i < progressManager.Instance.nextDayAttribute.bubblesToSpawn; i++)
        {
            itemsToSpawn.Add(bublePrefab);
        }
        for (int i = 0; i < progressManager.Instance.nextDayAttribute.chiclesToSpawn; i++)
        {
            itemsToSpawn.Add(chiclePrefab);
        }
    }

    private void Start()
    {
        float invokeTime = (progressManager.Instance.nextDayAttribute.duracionDelDia * 60) / 32;
        InvokeRepeating("setTime", invokeTime, invokeTime);



        StartCoroutine(spawnCollectables());

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


        float i = Random.Range(0.1f, 1.0f);
        if (i < .6f)
            StartCoroutine(playAmbienceSound());

        //a determinada hora cambiar el momento del dia en la escena
        if(horaText.text == "16" && minutosText.text == "00")
        {
            //Se cambia a tarde
            costaneraManager.Instance.setTarde();
            npcManager.Instance.onChangeTarde();
        }
        else if(horaText.text == "20" && minutosText.text == "00")
        {
            //Se cambia a noche
            costaneraManager.Instance.setNoche();
            npcManager.Instance.onChangeNoche();
        }
        else if(horaText.text == "24")
        {
            //se termina el juego
            horaText.text = "00";
            levelCompleted();
        }
    }
    IEnumerator playAmbienceSound()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 18.0f));
        audioManager.Instance.playRandomAmbienceSound();
    }

    public void setStars(int adicion)
    {
        starsLeft += adicion;
        canvasAnim.SetInteger("starsLeft", starsLeft);

        if (adicion < 0)
            audioManager.Instance.playSound("loseStar");


        if(starsLeft == 0 && isTransition == false)
        {
            lose();
        }
    }

    void lose()
    {
        audioManager.Instance.stopSound("backgroundMusic");
        Time.timeScale = 0;
        audioManager.Instance.playSound("levelLose");
        canvasAnim.SetTrigger("levelLose");
        npcManager.Instance.StopAllCoroutines();
    }
    void levelCompleted()
    {
        audioManager.Instance.stopSound("backgroundMusic");
        Time.timeScale = 0;
        audioManager.Instance.playSound("levelCompleted");
        canvasAnim.SetTrigger("dayCompleted");
        npcManager.Instance.StopAllCoroutines();
        progressManager.Instance.progressData.diasInfo[progressManager.Instance.nextDayAttribute.diaNumero].completado = true;

        //puntaje
        int earned = (int)Mathf.Round(starsLeft * 10 + (progressManager.Instance.nextDayAttribute.diaNumero * (starsLeft * .1f)) * 15);
        Debug.Log("Se ganaron " + earned.ToString());
        costaneraCanvas.Instance.levelCompleted(earned);

        if (progressManager.Instance.daysAttributes[progressManager.Instance.nextDayAttribute.diaNumero].habilitado == false)
            siguienteDiaBTN.interactable = false;

        if (starsLeft > progressManager.Instance.progressData.diasInfo[progressManager.Instance.nextDayAttribute.diaNumero].estrellas)
            progressManager.Instance.progressData.diasInfo[progressManager.Instance.nextDayAttribute.diaNumero].estrellas = starsLeft;


    }

    #region Collectables
    List<GameObject> itemsToSpawn;

    public enum collectables { Burbuja, Helado, Chicle }

    public IEnumerator spawnCollectables()
    {
        float waitTime = (progressManager.Instance.nextDayAttribute.duracionDelDia * 60) / itemsToSpawn.Count;
        for (int i = 0; i < itemsToSpawn.Count; i++)
        {
            float waitRandom = Random.Range(0.1f, waitTime);
            yield return new WaitForSeconds(waitRandom);

            int objIndx = Random.Range(0, itemsToSpawn.Count);
            GameObject newItem = Instantiate(itemsToSpawn[objIndx], new Vector2(Random.Range(sceneLimitLeft, sceneLimitRigth), itemsToSpawn[objIndx].transform.position.y), Quaternion.identity, transform);
            itemsToSpawn.RemoveAt(objIndx);

            yield return new WaitForSeconds(waitTime - waitRandom);
        }
    }

    public GameObject bubbleShield;

    public void onCollect(collectables itemCollected)
    {
        Debug.Log("Se recolecto " + itemCollected.ToString());
        if(itemCollected == collectables.Burbuja)
        {
            bubbleShield.SetActive(true);
            playerAnimController.Instance.setBubble(1);
            costaneraCanvas.Instance.agregarCollectable(collectables.Burbuja);
        }

        if(itemCollected == collectables.Chicle)
        {
            FindObjectOfType<playerController>().onGetChicle(true);
            audioManager.Instance.playSound("chicloso");
            costaneraCanvas.Instance.agregarCollectable(collectables.Chicle);
        }
    }
    #endregion

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
        isTransition = true;
        audioManager.Instance.playSound("click02");
        Time.timeScale = 1;
        StartCoroutine(sceneLoader.Instance.loadScene(sceneLoader.Instance.indexCostanera));
    }
    public void jugarSiguienteDia()
    {
        isTransition = true;
        audioManager.Instance.playSound("click02");
        Time.timeScale = 1;

        progressManager.Instance.nextDayAttribute = progressManager.Instance.daysAttributes[progressManager.Instance.nextDayAttribute.diaNumero];
        StartCoroutine(sceneLoader.Instance.loadScene(sceneLoader.Instance.indxNoticiero));
    }
    public void menu()
    {
        isTransition = true;
        audioManager.Instance.playSound("click03");
        Time.timeScale = 1;
        StartCoroutine(sceneLoader.Instance.loadScene(1));
    }
}
