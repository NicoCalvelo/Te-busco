using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

/// <Documentacion>
/// Resumen:
///     Este script controla las transcisiones y cambios de escena.
/// 
/// Creación:
///     05/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     05/06/2020 Calvelo Nicolás
///     
/// </Documentacion>

public class sceneLoader : MonoBehaviour
{
    #region singleton
    private static sceneLoader _instance;
    public static sceneLoader Instance
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

    [Header("Scene Transition")]
    public Animator crossFadeAnim;
    public float transitionTime = 1;

    public int indexCostanera;

    [HideInInspector]
    public bool changeScene = false;

    AsyncOperation operation;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator loadScene(int sceneIndex)
    {
        crossFadeAnim.SetTrigger("startFade");

        yield return new WaitForSeconds(transitionTime);
        crossFadeAnim.ResetTrigger("startFade");
        SceneManager.LoadScene(sceneIndex);
    }

    public IEnumerator loadSceneAsync()
    {
        operation = SceneManager.LoadSceneAsync(indexCostanera, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        yield return new WaitUntil(() => changeScene == true);
        crossFadeAnim.SetTrigger("startFade");
        yield return new WaitForSeconds(transitionTime);
        crossFadeAnim.ResetTrigger("startFade");
        operation.allowSceneActivation = true;
    }
}
