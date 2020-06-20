using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <Documentacion>
/// Resumen:
///     Este script controla las transcisiones y cambios de escena.
/// 
/// Creación:
///     05/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     14/06/2020 Calvelo Nicolás
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
    public float transitionTime = 1;
    public Animator crossFadeAnim, noticiasAnim;

    private Animator transitionAnim;

    public int indexCostanera = 3, indxNoticiero = 2;

    [HideInInspector]
    public bool changeScene = false;

    AsyncOperation operation;

    private void Awake()
    {
        if (sceneLoader.Instance != null)
        {
            Destroy(gameObject);
            return;
        }


        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        onChangeScene(0);
    }
    private void OnLevelWasLoaded(int level)
    {
        onChangeScene(SceneManager.GetActiveScene().buildIndex);
    }

    void onChangeScene(int scene)
    {
        changeScene = false;
        noticiasAnim.enabled = false;
        crossFadeAnim.enabled = false;

        if (scene == 2)
        {
            crossFadeAnim.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            transitionAnim = noticiasAnim;
            transitionTime = 2f;

        }
        else
        {
            noticiasAnim.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            transitionAnim = crossFadeAnim;
            transitionTime = .8f;
        }

        transitionAnim.enabled = true;
    }

    public IEnumerator loadScene(int sceneIndex)
    {
        transitionAnim.SetTrigger("startFade");

        yield return new WaitForSeconds(transitionTime);
        transitionAnim.ResetTrigger("startFade");
        SceneManager.LoadScene(sceneIndex);
    }

    public IEnumerator loadSceneAsync()
    {
        operation = SceneManager.LoadSceneAsync(indexCostanera, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        yield return new WaitUntil(() => changeScene == true);
        transitionAnim.SetTrigger("startFade");
        yield return new WaitForSeconds(transitionTime);
        transitionAnim.ResetTrigger("startFade");
        operation.allowSceneActivation = true;
    }
}
