using System.Collections.Generic;
using UnityEngine;

/// <Documentacion>
/// Resumen:
///     Un simple manager de sonidos. Explicado en video de Brackeys "Introduction to audio in Unity".
/// 
/// Creación:
///     09/05/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     15/05/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class audioManager : MonoBehaviour
{
    #region singleton
    private static audioManager _instance;
    public static audioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("audioManager is null");
            }
            return _instance;
        }
    }
    #endregion

    GameObject player;
    public sound[] audioClips;

    Dictionary<string, AudioSource> audioManagerSounds;
    Dictionary<string, AudioSource> playerSounds;

    public enum gameObjectSource{
        audioManager,
        playerSound       
    }

    void Awake()
    {
        _instance = this;

        player = GameObject.FindGameObjectWithTag("Player");

        audioManagerSounds = new Dictionary<string, AudioSource>();
        playerSounds = new Dictionary<string, AudioSource>();

        foreach (sound clip in audioClips)
        {
            if (clip.objSource == gameObjectSource.audioManager)
            {
                audioManagerSounds.Add(clip.name, gameObject.AddComponent<AudioSource>());

                audioManagerSounds[clip.name].clip = clip.clip;
                audioManagerSounds[clip.name].volume = clip.volume;
                audioManagerSounds[clip.name].pitch = clip.pitch;
                audioManagerSounds[clip.name].loop = clip.loop;
            }
            else if (clip.objSource == gameObjectSource.playerSound && player != null)
            {
                playerSounds.Add(clip.name, player.AddComponent<AudioSource>());

                playerSounds[clip.name].clip = clip.clip;
                playerSounds[clip.name].volume = clip.volume;
                playerSounds[clip.name].pitch = clip.pitch;
                playerSounds[clip.name].loop = clip.loop;
                playerSounds[clip.name].spatialBlend = clip.spatialBlend;
            }
        }
    }

    public void playSound(gameObjectSource sourceObj, string clipName)
    {
        if (sourceObj == gameObjectSource.audioManager)
            audioManagerSounds[clipName].Play();
        else if (sourceObj == gameObjectSource.playerSound)
            playerSounds[clipName].Play();
    }

    [System.Serializable]
    public class sound 
    {
        public string name;
        public gameObjectSource objSource;

        public AudioClip clip;
        [Range(0, 1)]
        public float volume;
        [Range(0, 3)]
        public float pitch;
        public bool loop;
        [Range(0, 1)]
        public float spatialBlend;
    }
}
