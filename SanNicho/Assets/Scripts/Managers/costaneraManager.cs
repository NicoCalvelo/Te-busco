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
///     29/06/2020 Calvelo Nicolás
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

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        audioManager.Instance.changeBackgroundMusic(0);
        audioManager.Instance.playRandomAmbienceSound();
        setScene();
    }

    void setScene()
    {

        //Nubes
        int cantidadDeNubes = Random.Range(7, 15);

        for (int i = 0; i <= cantidadDeNubes; i++)
        {
            Vector3 pos = new Vector3 (Random.Range(gameManager.Instance.sceneLimitLeft, gameManager.Instance.sceneLimitRigth), Random.Range(55.0f, 65.0f), Random.Range(4.0f, 6.0f));
            Instantiate(nubePrefabs[Random.Range(0, nubePrefabs.Length)], pos, Quaternion.identity, transform);
        }

    }

    public void setTarde()
    {
        GetComponent<Animator>().SetTrigger("tarde");
        audioManager.Instance.changeBackgroundMusic(1);
    }

    public void setNoche()
    {
        GetComponent<Animator>().SetTrigger("noche");
        audioManager.Instance.changeBackgroundMusic(2);
    }
}
