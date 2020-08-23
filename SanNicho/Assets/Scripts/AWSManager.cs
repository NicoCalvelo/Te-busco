using Amazon;
using Amazon.CognitoIdentity;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Este script hace de intermediario entre los requests de informacion y la base de datos en la nube
///     de AWS. Principalmente maneja toda la informacion referida a los torneos.
/// 
/// Creación:
///     13/08/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     13/08/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class AWSManager : MonoBehaviour
{
    #region singleton
    private static AWSManager _instance;
    public static AWSManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("AWS Manager is null");
            }
            return _instance;
        }
    }
    #endregion

    public string S3Region = RegionEndpoint.USEast2.SystemName;
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }

    private AmazonS3Client _s3Client;
    public AmazonS3Client S3Client
    {
        get
        {
            if(_s3Client == null)
            {
                 _s3Client = new AmazonS3Client(new CognitoAWSCredentials(
                "us-east-2:615bc63c-3f7a-46f8-b879-e68a030b01d3", // identity Pool
                RegionEndpoint.USEast2 // region
                ), _S3Region);
            }
            return _s3Client;
        }
    }

    private void Awake()
    {
        _instance = this;

        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        S3Client.ListBucketsAsync(new ListBucketsRequest(), (responseObject) =>
        {
            if (responseObject.Exception == null)
            {
                responseObject.Response.Buckets.ForEach((S3b) =>
                {
                    Debug.Log("Bucket Name: " + S3b.BucketName);
                });
            }
            else
            {
                Debug.Log("AWS Error" + responseObject.Exception);
            }
        });

        DontDestroyOnLoad(gameObject);
    }


    //Se buscan carpetas o archivos dentro de la direccion especificada y se devuelve un List<string> con los fileNames
    public void getListFromS3(string bucket, string folderPath, Action<string> filePath, Action<long> listLength)
    {
        S3Client.ListObjectsAsync(bucket, folderPath, (responseObject) =>
        {

            if (responseObject.Exception == null)
            {
                listLength(responseObject.Response.ContentLength);
                responseObject.Response.S3Objects.ForEach((S3b) =>
                {
                    filePath(S3b.Key);
                });
            }
            else
            {
                Debug.Log("Error getting List of items from S3: " + responseObject.Exception);
                filePath(null);
            }
        });
    }

    //Se descarga el archivo especificado
    public void downloadFromS3(string bucket, string fileName, Action<MemoryStream> onComplete)
    {
        S3Client.GetObjectAsync(bucket, fileName, (responseObj) =>
        {
            if (responseObj.Response.ResponseStream != null)
            {
                //read data and aply it to a case (object) to be used
                byte[] data = null;

                using (StreamReader reader = new StreamReader(responseObj.Response.ResponseStream))
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        var buffer = new byte[512];
                        var byteReads = default(int);

                        while ((byteReads = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            memory.Write(buffer, 0, byteReads);
                        }
                        data = memory.ToArray();
                    }
                }

                using (MemoryStream memory = new MemoryStream(data))
                {
                    onComplete(new MemoryStream(data));
                }
            }
            else
            {
                Debug.Log("file not founded");
                onComplete(null);
            }
        });
    }

    //Se sube el archivo especificado
    public void uploadToS3(string path, string bucket, string fileName, Action<bool> succes)
    {
        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = bucket,
            Key = fileName,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = RegionEndpoint.USEast2
        };
        S3Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                //se subio el archivo
                Debug.Log("Succesfuly posted to bucket");
                succes(true);
            }
            else
            {
                //no se pudo subir el archivo
                Debug.Log("Exception occured during uploading: " + responseObj.Exception);
                succes(false);
            }

        });
    }
}
