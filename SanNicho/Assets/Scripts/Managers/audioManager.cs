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
///     06/06/2020 Calvelo Nicolás
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

    public sound[] audioClips;

    Dictionary<string, AudioSource> audioManagerSounds;


    void Awake()
    {
        _instance = this;

        audioManagerSounds = new Dictionary<string, AudioSource>();

        foreach (sound clip in audioClips)
        {
            audioManagerSounds.Add(clip.name, gameObject.AddComponent<AudioSource>());

            audioManagerSounds[clip.name].clip = clip.clip;
            audioManagerSounds[clip.name].volume = clip.volume;
            audioManagerSounds[clip.name].pitch = clip.pitch;
            audioManagerSounds[clip.name].loop = clip.loop;
        }
    }

    public void playSound(string clipName)
    {
        audioManagerSounds[clipName].Play();
    }

    [System.Serializable]
    public class sound 
    {
        public string name;

        public AudioClip clip;
        [Range(0, 1)]
        public float volume;
        [Range(0, 3)]
        public float pitch;
        public bool loop;
    }
}
