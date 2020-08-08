using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

/// <Documentacion>
/// Resumen:
///     Este script controla las interacciones del usuario con la escena.
/// 
/// Creación:
///     12/06/2020 Calvelo Nicolás
/// 
/// Ultima modificación:
///     30/07//2020 Calvelo Nicolás
///     
/// </Documentacion>

public class noticieroManager : MonoBehaviour
{
    public float waitChar, waitAfterDot;

    [Header("UI Elements")]
    public TextMeshProUGUI noticiaText;
    public Image noticiaImg;

    [SerializeField]
    private dayAttributes.noticia endOfNoticias;

    private Coroutine coroutine;

    private Queue<dayAttributes.noticia> noticiasList;

    [SerializeField]
    private Sprite[] gatoSprites;

    [SerializeField]
    Image gato, reportera;

    //Para checkear si el courutine que escribe la noticia sigue siendo ejecutado
    bool courRunning = false;

    private void Start()
    {
        if(progressManager.Instance.nextDayAttribute.diaNumero == 13)
        {
            courRunning = true;
            gato.enabled = false;
            reportera.enabled = false;
            if(progressManager.Instance.progressData.diasInfo[13].estrellas == 3)
            {
                noticiaText.text = "Por motivos supersticiosos hoy no trabajamos...";
            }
            else
            {
                progressManager.Instance.progressData.diasInfo[13].completado = true;
                if (progressManager.Instance.progressData.diasInfo[12].estrellas == 0)
                    noticiaText.text = "Por motivos supersticiosos hoy no trabajamos...";
                else if (progressManager.Instance.progressData.diasInfo[13].estrellas == 1)
                    noticiaText.text = "Ya te hemos avisado... Hoy cerramos, vuelve mañana.";
                else if (progressManager.Instance.progressData.diasInfo[13].estrellas == 2)
                    noticiaText.text = "Si que eres persistenete... Aqui tienes tu tercer estrella.";
                progressManager.Instance.progressData.diasInfo[13].estrellas++;
            }

            Invoke("returnLevels", 6);

            return;
        }

        FindObjectOfType<audioManager>().playSound("mainMusic");
        courRunning = false;
        StartCoroutine(setNoticias(progressManager.Instance.nextDayAttribute.noticiasList));

        StartCoroutine(sceneLoader.Instance.loadSceneAsync());
    }

    // Se setean las noticias que se van a mostrar
    IEnumerator setNoticias(dayAttributes.noticia[] notList)
    {
        noticiasList = new Queue<dayAttributes.noticia>();
        foreach (dayAttributes.noticia not in progressManager.Instance.nextDayAttribute.noticiasList)
        {
            noticiasList.Enqueue(not);
        }

        yield return new WaitUntil(() => noticiasList.Count == notList.Length);

        if(progressManager.Instance.nextDayAttribute.diaNumero != 25)
        noticiasList.Enqueue(endOfNoticias);

        Invoke("displayNextNoticia", 2.5f);
    }

    public void displayNextNoticia()
    {
        if (courRunning == true) //Se checkea si todavia se sigue ejecutando el corutine
            return;

        if (noticiasList.Count == 0)
        {
            endNoticias();
            return;
        }

        dayAttributes.noticia not = noticiasList.Dequeue();
        coroutine = StartCoroutine(typeSentence(not.noticiaSentence));
        //noticiaImg.sprite = not.tvImage;

    }
    IEnumerator typeSentence(string sentece)
    {
        courRunning = true;
        noticiaText.text = "";

        foreach (char letter in sentece.ToCharArray())
        {
            noticiaText.text += letter;
            if(letter.ToString() == "." || letter.ToString() == "!" || letter.ToString() == "?")
                yield return new WaitForSeconds(waitAfterDot);
            else
            {
                FindObjectOfType<audioManager>().playSound("voice" + Random.Range(1, 9).ToString());
                yield return new WaitForSeconds(waitChar);
            }

        }

        courRunning = false;
    }
    void endNoticias()
    {
        sceneLoader.Instance.changeScene = true;
    }
    public void changeCat(Image gato)
    {
        audioManager.Instance.playSound("Meow" + Random.Range(0,3).ToString());
        gato.sprite = gatoSprites[Random.Range(0, gatoSprites.Length)];
    }

    void returnLevels()
    {
        StartCoroutine(sceneLoader.Instance.loadScene(1));
    }
}
