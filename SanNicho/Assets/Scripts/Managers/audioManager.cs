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
///     26/06/2020 Calvelo Nicolás
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

    public sound[] backgroundsMusics;

    [Header("Ambience Sounds")]
    public sound[] morningAS, eveningAS, nightAS;

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

        audioManagerSounds.Add("backgroundMusic", gameObject.AddComponent<AudioSource>());

        audioManagerSounds.Add("ambienceSound", gameObject.AddComponent<AudioSource>());         
    }

    public void playSound(string clipName)
    {
        audioManagerSounds[clipName].Play();
    }
    public void unPauseMusic()
    {
        audioManagerSounds["backgroundMusic"].UnPause();
    }
    public void stopSound(string clipName)
    {
        audioManagerSounds[clipName].Pause();
    }

    public void changeBackgroundMusic(int clipIndx)
    {
        audioManagerSounds["backgroundMusic"].clip = backgroundsMusics[clipIndx].clip;
        audioManagerSounds["backgroundMusic"].volume = backgroundsMusics[clipIndx].volume;
        audioManagerSounds["backgroundMusic"].pitch = backgroundsMusics[clipIndx].pitch;
        audioManagerSounds["backgroundMusic"].loop = backgroundsMusics[clipIndx].loop;

        audioManagerSounds["backgroundMusic"].Play();
    }

    public void playRandomAmbienceSound()
    {
        sound clip;
        if (gameManager.Instance.hora < 16)
        {
            clip = morningAS[Random.Range(0, morningAS.Length)];
        }
        else if(gameManager.Instance.hora < 20)
        {
            clip = eveningAS[Random.Range(0, eveningAS.Length)];
        }
        else
        {
            clip = nightAS[Random.Range(0, nightAS.Length)];
        }

        audioManagerSounds["ambienceSound"].clip = clip.clip;
        audioManagerSounds["ambienceSound"].volume = clip.volume;
        audioManagerSounds["ambienceSound"].pitch = clip.pitch;
        audioManagerSounds["ambienceSound"].loop = clip.loop;
        audioManagerSounds["ambienceSound"].Play();
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
