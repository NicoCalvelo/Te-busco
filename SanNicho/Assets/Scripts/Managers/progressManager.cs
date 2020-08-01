using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de ir adecuando el juego acorde al progreso del jugador.
/// 
/// Creación:
///     05/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     03/07/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class progressManager : MonoBehaviour
{
    #region singleton
    private static progressManager _instance;
    public static progressManager Instance
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

    public dayAttributes nextDayAttribute;
    public List<dayAttributes> daysAttributes;

    public infoJugador progressData;

    [HideInInspector]
    public int diasHabilitados;

    public void Awake()
    {
        if (progressManager.Instance != null)
        {
            Destroy(gameObject);
            return;
        }


        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        diasHabilitados = RemoteSettings.GetInt("diasHabilitados");
        Debug.Log("dias habilitados = " + diasHabilitados);
        decompressInfoJugador();
    }

    //Se extrae la informacion del archivo que contiene la informacion referente al progreso del jugador
    void decompressInfoJugador()
    {
        if(File.Exists(Application.persistentDataPath + "/progressData.dat") == true)      
        {
            Debug.Log(Application.persistentDataPath + " / progressData.dat");
            FileStream infoPlayer = new FileStream(Application.persistentDataPath + "/progressData.dat", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            byte[] data = null;

            using (StreamReader reader = new StreamReader(infoPlayer))
            {
                using (MemoryStream memory01 = new MemoryStream())
                {
                    var buffer = new byte[512];
                    var byteReads = default(int);

                    while ((byteReads = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memory01.Write(buffer, 0, byteReads);
                    }
                    data = memory01.ToArray();
                }
            }
            MemoryStream memory02 = new MemoryStream(data);
            BinaryFormatter bfo = new BinaryFormatter();

            infoJugador info = (infoJugador)bfo.Deserialize(memory02);

            //Hay nuevos logros
            if(info.logros.Count < progressData.logros.Count)
            {
                for (int i = info.logros.Count; i < progressData.logros.Count; i++)
                {
                    info.logros.Add(progressData.logros[i]);
                }
            }
            //Hay nuevos items en la tienda
            if(info.shopItems.Count < progressData.shopItems.Count)
            {
                for (int i = info.shopItems.Count; i < progressData.shopItems.Count; i++)
                {
                    info.shopItems.Add(progressData.shopItems[i]);
                }
            }

            progressData = info;
        }
        else //No hay archivo todavia
        {

            progressData.diasInfo = new System.Collections.Generic.Dictionary<int, infoJugador.nivel>();
            
            for (int i = 0; i < 25; i++)
            {
                progressData.diasInfo.Add(i + 1, new infoJugador.nivel());
            }

            BinaryFormatter bf = new BinaryFormatter();
            string filePath = Application.persistentDataPath + "/progressData" + ".dat";
            FileStream file = File.Create(filePath);
            bf.Serialize(file, progressData);
            file.Close();
        }

        for (int i = 0; i < diasHabilitados; i++)
        {
            daysAttributes[i].habilitado = true;
        }
        for (int i = diasHabilitados; i < 25; i++)
        {
            daysAttributes[i].habilitado = false;
        }
    }

    //Cuando se cierra la aplicacion se guarda la info del jugador en el archivo
    private void OnApplicationQuit()
    {
        BinaryFormatter bf = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/progressData" + ".dat";
        FileStream file = File.Create(filePath);
        bf.Serialize(file, progressData);
        file.Close();
    }

    private void OnApplicationPause(bool pause)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/progressData" + ".dat";
        FileStream file = File.Create(filePath);
        bf.Serialize(file, progressData);
        file.Close();
    }
}
