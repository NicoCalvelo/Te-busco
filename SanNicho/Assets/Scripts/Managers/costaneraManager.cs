using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de controlar la escena acorde al nivel del dia que se esta jugando y a los eventos
///     enviados por el gameManager.
/// 
/// Creación:
///     07/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     03/08/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class costaneraManager : MonoBehaviour
{
    #region singleton
private static costaneraManager _instance;
    public static costaneraManager Instance
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

    public GameObject[] nubePrefabs;
    public GameObject arcoFutbol, doraemon, colectivo, firulais, heladero, elFamoso, globitos, primavera, niebla;

    Animator anim;

    private void Awake()
    {
        _instance = this;
        anim = GetComponent<Animator>();
        setScene();
    }

    void Start()
    {
        audioManager.Instance.changeBackgroundMusic(0);
        audioManager.Instance.playRandomAmbienceSound();

    }

    void setScene()
    {

        //Nubes
        int cantidadDeNubes = Random.Range(11, 19);

        if(progressManager.Instance.nextDayAttribute.diaNumero == 5)
        {
            cantidadDeNubes += 75;
            Instantiate(gameManager.Instance.bublePrefab, new Vector2(-85, 65), Quaternion.identity, gameManager.Instance.transform);
        }
        else if(progressManager.Instance.nextDayAttribute.diaNumero == 9)
        {
            Instantiate(gameManager.Instance.chiclePrefab, new Vector2(-85, 0), Quaternion.identity, gameManager.Instance.transform);
        }
        else if (progressManager.Instance.nextDayAttribute.diaNumero == 6)
        {
            arcoFutbol.SetActive(true);
        }
        else if (progressManager.Instance.nextDayAttribute.diaNumero == 10)
        {
            niebla.SetActive(true);
        }
        else if(progressManager.Instance.nextDayAttribute.diaNumero == 11)
        {
            colectivo.SetActive(true);
        }
        else if (progressManager.Instance.nextDayAttribute.diaNumero == 12)
        {
            doraemon.SetActive(true);
        }else if (progressManager.Instance.nextDayAttribute.diaNumero == 16)
        {
            firulais.SetActive(true);
        }else if (progressManager.Instance.nextDayAttribute.diaNumero == 18)
        {
            globitos.SetActive(true);
        }else if(progressManager.Instance.nextDayAttribute.diaNumero == 20)
        {
            anim.SetTrigger("preEscenario");
        }

        if (progressManager.Instance.nextDayAttribute.diaNumero >= 21)
        {
            primavera.SetActive(true);
        }

        heladero.SetActive(progressManager.Instance.nextDayAttribute.heladero);

        elFamoso.SetActive(progressManager.Instance.nextDayAttribute.elFamoso);

        for (int i = 0; i <= cantidadDeNubes; i++)
        {
            Vector3 pos = new Vector3 (Random.Range(gameManager.Instance.sceneLimitLeft, gameManager.Instance.sceneLimitRigth), Random.Range(55.0f, 75.0f), Random.Range(3.5f, 6.0f));
            Instantiate(nubePrefabs[Random.Range(0, nubePrefabs.Length)], pos, Quaternion.identity, transform);
        }
    }

    public void setTarde()
    {
        anim.SetTrigger("tarde");
        audioManager.Instance.changeBackgroundMusic(1);
    }
    public void setNoche()
    {
        anim.SetTrigger("noche");
        audioManager.Instance.changeBackgroundMusic(2);
        if (progressManager.Instance.nextDayAttribute.diaNumero != 15)
            Invoke("setLuces", 15);
        else if (progressManager.Instance.nextDayAttribute.diaNumero == 20)
        {
            //Cambiar musica de fondo
        }

    }
    public void setLuces()
    {
        anim.SetTrigger("encenderLuces");
    }
}
