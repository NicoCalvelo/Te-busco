using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de controlar todos los npc's de la escena.
/// 
/// Creación:
///     16/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     26/08/2020 Calvelo Nicolás
///     
/// </Documentacion>
/// 

public class npcManager : MonoBehaviour
{
    #region singleton
    private static npcManager _instance;
    public static npcManager Instance
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

    Dictionary<string, GameObject> npcDictionary;
    int nextNpcNumber = 0;
    Transform player;


    public Queue<GameObject> photoQueue;
    public GameObject shootPrefab;

    [SerializeField]
    private GameObject npc01Prefab, npc02Prefab, npc03Prefab, npc04Prefab, npcBackground;

    [SerializeField]
    private List<Transform> npc02Positions, npc03Positions;


    private void Awake()
    {
        _instance = this;

        player = Camera.main.GetComponent<Transform>();
        npcDictionary = new Dictionary<string, GameObject>();
    }

    private void Start()
    {
        photoQueue = new Queue<GameObject>();

        for (int i = 0; i < progressManager.Instance.nextDayAttribute.inicioNpc_01; i++)
        {
            Invoke("instantiateNewNpc", Random.Range(0.1f, 3.0f));
        }

        for (int i = 0; i < progressManager.Instance.nextDayAttribute.cantidadNPC_02; i++)
        {
            if (npc02Positions.Count == 0)
                return;

            Transform elected = npc02Positions[Random.Range(0, npc02Positions.Count)];
            instantiateNewNpc02(elected);
            npc02Positions.Remove(elected);
        }

        for (int i = 0; i < progressManager.Instance.nextDayAttribute.cantidadNPC_03; i++)
        {
            if (npc03Positions.Count == 0)
                return;

            Transform elected = npc03Positions[Random.Range(0, npc03Positions.Count)];
            instantiateNewNpc03(elected);
            npc03Positions.Remove(elected);
        }

        for (int i = 0; i < progressManager.Instance.nextDayAttribute.cantidadNPC_04; i++)
        {
            instantiateNewNpc04();
        }

        for (int i = 0; i < progressManager.Instance.nextDayAttribute.cantidadNPC_Background; i++)
        {
            instantiateNewNpcBackground();
        }
    }

    public void onChangeTarde()
    {
        for (int i = 0; i < progressManager.Instance.nextDayAttribute.agregarNpc01Tarde; i++)
        {
            Invoke("instantiateNewNpc", Random.Range(0.1f, 3.0f));
        }

        for (int i = 0; i < progressManager.Instance.nextDayAttribute.agregarNpc02Tarde; i++)
        {
            if (npc02Positions.Count == 0)
                return;

            Transform elected = npc02Positions[Random.Range(0, npc02Positions.Count)];
            instantiateNewNpc02(elected);
            npc02Positions.Remove(elected);
        }
        for (int i = 0; i < progressManager.Instance.nextDayAttribute.agregarNpc04Tarde; i++)
        {
            instantiateNewNpc04();
        }
    }
    public void onChangeNoche()
    {



        for (int i = 0; i < progressManager.Instance.nextDayAttribute.agregarNpc01Noche; i++)
        {
            Invoke("instantiateNewNpc", Random.Range(0.1f, 3.0f));
        }

        for (int i = 0; i < progressManager.Instance.nextDayAttribute.agregarNpc02Noche; i++)
        {
            if (npc02Positions.Count == 0)
                return;

            Transform elected = npc02Positions[Random.Range(0, npc02Positions.Count)];
            instantiateNewNpc02(elected);
            npc02Positions.Remove(elected);
        }

        for (int i = 0; i < progressManager.Instance.nextDayAttribute.agregarNpc04Noche; i++)
        {
            instantiateNewNpc04();
        }
    }

    public IEnumerator exitPlayerView(Transform npcTransform)
    {
        yield return new WaitForSeconds(3);

        npcTransform.position = new Vector2(player.position.x + (85 * (Random.Range(0, 2) * 2 - 1)), 0);

        if (npcTransform.position.x < gameManager.Instance.sceneLimitLeft || npcTransform.position.x > gameManager.Instance.sceneLimitRigth)
            StartCoroutine(exitPlayerView(npcTransform));
    }


    public void storePhoto(GameObject photo)
    {
        photoQueue.Enqueue(photo);
    }
    public void requestPhoto(Vector2 pos)
    {
        GameObject newPhoto;

        if (photoQueue.Count > 0)
        {
            newPhoto = photoQueue.Dequeue();
        }
        else
            newPhoto = Instantiate(shootPrefab, Vector3.zero, Quaternion.identity);

        newPhoto.transform.position = pos;
        newPhoto.SetActive(true);
    }


    void instantiateNewNpc()
    {
        Vector2 spawnPos = new Vector2(player.position.x + (Random.Range(80.0f, 100.0f)  * (Random.Range(0, 2) * 2 - 1)), 0);

        if(spawnPos.x < gameManager.Instance.sceneLimitLeft || spawnPos.x > gameManager.Instance.sceneLimitRigth)
        {
            instantiateNewNpc();
            return;
        }

        GameObject newNpc = Instantiate(npc01Prefab, spawnPos, Quaternion.identity, transform);
        newNpc.name = "npc01_" + nextNpcNumber.ToString();
        nextNpcNumber++;
        npcDictionary.Add(newNpc.name, newNpc);
    }
    void instantiateNewNpc02(Transform npcPos)
    {
        GameObject newNpc = Instantiate(npc02Prefab, npcPos.position, Quaternion.identity, transform);
        newNpc.transform.rotation = npcPos.rotation;
        newNpc.name = "npc02_" + nextNpcNumber.ToString();
        nextNpcNumber++;
        npcDictionary.Add(newNpc.name, newNpc);
    }
    void instantiateNewNpc03(Transform npcPos)
    {
        GameObject newNpc = Instantiate(npc03Prefab, npcPos.position, Quaternion.identity, transform);
        newNpc.transform.rotation = npcPos.rotation;
        newNpc.name = "npc03_" + nextNpcNumber.ToString();
        nextNpcNumber++;
        npcDictionary.Add(newNpc.name, newNpc);
    }
    void instantiateNewNpc04()
    {
        GameObject newNpc = Instantiate(npc04Prefab, Vector2.zero, Quaternion.identity, transform);
        newNpc.name = "npc04_" + nextNpcNumber.ToString();
        nextNpcNumber++;
        npcDictionary.Add(newNpc.name, newNpc);
    }
    void instantiateNewNpcBackground()
    {
        GameObject newNpc = Instantiate(npcBackground, new Vector3(Random.Range(gameManager.Instance.sceneLimitLeft + 75, gameManager.Instance.sceneLimitRigth - 75), 10, .5f), Quaternion.identity, transform);
        newNpc.name = "npcBackground_" + nextNpcNumber.ToString();
        nextNpcNumber++;
        npcDictionary.Add(newNpc.name, newNpc);
    }

    public void destroyAllNpc()
    {
        foreach (GameObject npc in npcDictionary.Values)
        {
            Destroy(npc);
        }
    }
}
