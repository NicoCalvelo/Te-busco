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
///     03/07//2020 Calvelo Nicolás
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

    //Para checkear si el courutine que escribe la noticia sigue siendo ejecutado
    bool courRunning = false;

    private void Start()
    {

        FindObjectOfType<audioManager>().playSound("mainMusic");

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
        audioManager.Instance.playSound("Meow" + Random.Range(0,4).ToString());
        gato.sprite = gatoSprites[Random.Range(0, gatoSprites.Length)];
    }
}
