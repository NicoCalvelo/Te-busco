using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de controlar todos los npc's de la escena.
/// 
/// Creación:
///     16/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     20/06/2020 Calvelo Nicolás
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

    public float sceneLimitLeft = -336.0f, sceneLimitRigth = 314.0f;

    [SerializeField]
    private GameObject npc01Prefab;

    private void Awake()
    {
        _instance = this;

        player = Camera.main.GetComponent<Transform>();
        npcDictionary = new Dictionary<string, GameObject>();

    }

    private void Start()
    {
        for (int i = 0; i < progressManager.Instance.nextDayAttribute.maxNpc_01; i++)
        {
            Invoke("instantiateNewNpc", Random.Range(0.1f, 3.0f));
        }
    }

    public IEnumerator exitPlayerView(GameObject npc)
    {
        yield return new WaitForSeconds(3);
        npcDictionary.Remove(npc.name);

        Destroy(npc);
        instantiateNewNpc();
    }
    void instantiateNewNpc()
    {
        Vector2 spawnPos = new Vector2(player.position.x + (Random.Range(80.0f, 100.0f)  * (Random.Range(0, 2) * 2 - 1)), 0);

        if(spawnPos.x < sceneLimitLeft || spawnPos.x > sceneLimitRigth)
        {
            instantiateNewNpc();
            return;
        }

        GameObject newNpc = Instantiate(npc01Prefab, spawnPos, Quaternion.identity, transform);
        newNpc.name = "npc" + nextNpcNumber.ToString();
        nextNpcNumber++;
        npcDictionary.Add(newNpc.name, newNpc);
    }

    public void stopNpcCoroutines()
    {
        foreach (GameObject npc in npcDictionary.Values)
        {
            npc.GetComponent<NPC_Complete>().StopAllCoroutines();
        }
    }
}
