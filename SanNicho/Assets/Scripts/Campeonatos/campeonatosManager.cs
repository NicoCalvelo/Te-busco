using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <Documentacion>
/// Resumen:
///     Este script se encarga de controlar la seccion de campeonatos y setearlos correctamente.
/// 
/// Creación:
///     06/08/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     23/08//2020 Calvelo Nicolás
///     
/// </Documentacion>

public class campeonatosManager : MonoBehaviour
{
    #region singleton
    private static campeonatosManager _instance;
    public static campeonatosManager Instance
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

    public CampeonatosData campeonatosData;


    bool isPlaying = false;
    public string champIDplaying;

    private void Awake()
    {
        if (campeonatosManager.Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {

        decompressCampeonatosData();
    }

    #region Comunicacion con AWS
    public void subirTablaDePosiciones(string campeonatoID, List<userDisplay> listToUpload, Action<bool> onComplete)
    {
        string bucket = "campeonatostebusco";
        string fileName = "tablasdeposiciones/" + campeonatoID;

        BinaryFormatter bf = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "tablasDePosiciones/" + campeonatoID + ".dat";
        FileStream file = File.Create(filePath);
        bf.Serialize(file, listToUpload);
        file.Close();

        AWSManager.Instance.uploadToS3(filePath, bucket, fileName, (bool succes) =>
        {
            onComplete(succes);
        });
    }

    public void descargarTabalDePosiciones(string campeonatoID, Action<List<userDisplay>> downloadedInfo)
    {
        string bucket = "campeonatostebusco";
        string fileName = "tablasdeposiciones/" + campeonatoID;

        AWSManager.Instance.downloadFromS3(bucket, fileName, (MemoryStream memory) =>
        {
            if (memory.Length < 1) //No se encontro usuario
            {
                downloadedInfo(null);
                return;
            }
            else //Si se encontro usuario
            {
                BinaryFormatter bfo = new BinaryFormatter();
                List<userDisplay> downloadedProfile = (List<userDisplay>)bfo.Deserialize(memory);
                downloadedInfo(downloadedProfile);
            }
        });
    }
    public void descargarCampeonatos(Action completed)
    {
        string bucket = "campeonatostebusco";
        string fileName = "campeonatos/";
        int campeonatosAdescargar = 1;
        campeonatosData.campeonatosList = new List<campeonatoAttribute>();

        AWSManager.Instance.getListFromS3(bucket, fileName, (string campeonatoName) =>
        {
            AWSManager.Instance.downloadFromS3(bucket, fileName + campeonatoName, (MemoryStream memory) =>
            {
                BinaryFormatter bfo = new BinaryFormatter();
                campeonatoAttribute downloadedFile = (campeonatoAttribute)bfo.Deserialize(memory);
                campeonatosData.campeonatosList.Add(downloadedFile);
                if (campeonatosData.campeonatosList.Count == campeonatosAdescargar)
                    completed();
            });

        }, (long listLength) => { campeonatosAdescargar = listLength.GetHashCode(); });
    }

    #endregion

    void decompressCampeonatosData()
    {
        if (File.Exists(Application.persistentDataPath + "/campeonatosData.dat") == true)
        {
            Debug.Log(Application.persistentDataPath + "/campeonatosData.dat");
            FileStream infoPlayer = new FileStream(Application.persistentDataPath + "/campeonatosData.dat", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
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

            CampeonatosData campeonatoInfo = (CampeonatosData)bfo.Deserialize(memory02);

            campeonatosData = campeonatoInfo;

            if (DateTime.Compare(DateTime.Now.Date, campeonatosData.lastUpdate.Date) > 0)
            {
                campeonatosData.campeonatosList = new List<campeonatoAttribute>();
                campeonatosData.tablaDePosiciones = new Dictionary<string, List<userDisplay>>();
                descargarCampeonatos(() =>
                {
                    campeonatosData.lastUpdate = DateTime.Now;
                    panelCampeonatos.Instance.setTarjetasCampeonatos();
                });
            }
            else
                panelCampeonatos.Instance.setTarjetasCampeonatos();
        }
        else //No hay archivo todavia
        {
            campeonatosData.campeonatosList = new List<campeonatoAttribute>();
            campeonatosData.tablaDePosiciones = new Dictionary<string, List<userDisplay>>();
            descargarCampeonatos(() =>
            {
                BinaryFormatter bf = new BinaryFormatter();
                string filePath = Application.persistentDataPath + "/campeonatosData" + ".dat";
                FileStream file = File.Create(filePath);
                bf.Serialize(file, campeonatosData);
                file.Close();

                campeonatosData.lastUpdate = DateTime.Now;
                panelCampeonatos.Instance.setTarjetasCampeonatos();
            });
        }

    }


    public void inicioDia()
    {
        isPlaying = true;
        gameManager.onComplete += diaCompletado;
        gameManager.onLose += lose;
    }

    public void lose()
    {
        outputPartida(true);
        isPlaying = false;
    }

    public void diaCompletado()
    {
        campeonatosData.userCampeonatoProgress[champIDplaying].diaActual++;
        campeonatosData.userCampeonatoProgress[champIDplaying].puntosActuales += gameManager.Instance.starsLeft * 10;

        outputPartida(false);

        isPlaying = false;
    }

    //Se setean los datos del dia jugado
    void outputPartida(bool lose)
    {
        //Si el score es mayo que el maximo previo
        if(campeonatosData.userCampeonatoProgress[champIDplaying].diaActual > campeonatosData.userCampeonatoProgress[champIDplaying].cantidadMaximaDeDias)
        {
            campeonatosData.userCampeonatoProgress[champIDplaying].cantidadMaximaDeDias = campeonatosData.userCampeonatoProgress[champIDplaying].diaActual;
            campeonatosData.userCampeonatoProgress[champIDplaying].cantidadMaximaDePuntos = campeonatosData.userCampeonatoProgress[champIDplaying].puntosActuales;
            
            //Subir info a la lista del torneo
            //No habia info antes
            if(Array.Exists(campeonatosData.tablaDePosiciones[champIDplaying].ToArray(), s => s.nombreDelUsuario == campeonatosData.nombreDeUsuario) == false)
            {
                userDisplay newDisp = new userDisplay();
                newDisp.nombreDelUsuario = campeonatosData.nombreDeUsuario;
                campeonatosData.tablaDePosiciones[champIDplaying].Add(newDisp);
            }

            userDisplay toSet = Array.Find(campeonatosData.tablaDePosiciones[champIDplaying].ToArray(), s => s.nombreDelUsuario == campeonatosData.nombreDeUsuario);
            toSet.CantidadMaximaDeDias = campeonatosData.userCampeonatoProgress[champIDplaying].diaActual;
            toSet.cantidadDePuntos = campeonatosData.userCampeonatoProgress[champIDplaying].puntosActuales;

            campeonatosData.tablaDePosiciones[champIDplaying].Sort(SortByScore);

            //Subir info al awsManager
            subirTablaDePosiciones(champIDplaying, campeonatosData.tablaDePosiciones[champIDplaying], (bool succes) => { });
        }

        if (lose)
        {
            campeonatosData.userCampeonatoProgress[champIDplaying].diaActual = 1;
            campeonatosData.userCampeonatoProgress[champIDplaying].puntosActuales = 0;
        }
    }

    static int SortByScore(userDisplay p1, userDisplay p2)
    {
        if(p1.CantidadMaximaDeDias == p2.CantidadMaximaDeDias)
            return p1.cantidadDePuntos.CompareTo(p2.cantidadDePuntos);
        else 
            return p1.CantidadMaximaDeDias.CompareTo(p2.CantidadMaximaDeDias);
    }


    [System.Serializable]
    public class userProgress 
    {
        public string nombreDeUsuario;
        public int diaActual;
        public int puntosActuales;
        public int cantidadMaximaDeDias;
        public int cantidadMaximaDePuntos;
    }
    [System.Serializable]
    public class userDisplay
    {
        public string nombreDelUsuario;
        public int CantidadMaximaDeDias;
        public int cantidadDePuntos;
    }
    [System.Serializable]
    public class CampeonatosData
    {
        public string nombreDeUsuario;
        public Dictionary<string, userProgress> userCampeonatoProgress;
        public Dictionary<string, List<userDisplay>> tablaDePosiciones;
        public List<campeonatoAttribute> campeonatosList;
        public Dictionary<string, campeonatoAttribute> campeonatosInscripto;
        public DateTime lastUpdate;
    }


    private void OnApplicationQuit()
    {
        if (isPlaying)
            outputPartida(true);
    }
    private void OnApplicationPause(bool pause)
    {
        if (isPlaying)
            outputPartida(true);
    }
}
