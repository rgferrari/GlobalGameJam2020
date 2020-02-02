using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour{

    public GameObject splashScreen;

    public scene.IntroScene introScene;

    [Header("Game Title")]
    public GameObject gameTitle;
    Vector3 gameTitleFinalPosition;

    [Header("Press Anykey Prompt")]
    public GameObject anykeyPrompt;
    Vector3 anykeyPromptFinalPosition;

    [Header("Lobby")]
    public CanvasGroup lobby;

    [Header("Progress Bar")]
    public RectTransform progressBar;
    public float duration = 1f;

    private void Start() {
        StartCoroutine(MenuSequence());
        StartCoroutine(AnykeyFlash());
    }

    IEnumerator MenuSequence(){

        lobby.gameObject.SetActive(false);

        gameTitleFinalPosition = gameTitle.transform.position;
        anykeyPromptFinalPosition = anykeyPrompt.transform.position;

        gameTitle.transform.position -= Vector3.right * 1500;
        anykeyPrompt.transform.position -= Vector3.right * 1500;

        yield return new WaitForSeconds(4.5f);
        gameTitle.transform.DOMove(gameTitleFinalPosition, 2f).SetEase(Ease.OutExpo);

        yield return new WaitForSeconds(0.2f);
        anykeyPrompt.transform.DOMove(anykeyPromptFinalPosition, 2f).SetEase(Ease.OutExpo);

        while(true){
            yield return null;
            if (Input.anyKeyDown)
                break;
        }

        splashScreen.SetActive(false);
        lobby.gameObject.SetActive(true);
        lobby.alpha = 0f;
        lobby.DOFade(1f, 0.3f);
        /*
        float _t = 0f;
        while(true){
            yield return null;
            if(Input.anyKey){
                _t += Time.deltaTime;
                progressBar.anchorMax = new Vector2(Mathf.Clamp01(_t / duration), 1f);
                print("holding");
            }else{
                _t = 0f;
                progressBar.anchorMax = new Vector2(0, 1);
            }

            if (_t > duration)
                break;
        }
        */
        //SceneManager.LoadScene("Integration");
        //introScene.Crash();
    }

    public void StartGame(){
        SceneManager.LoadScene("Integration");
    }

    IEnumerator AnykeyFlash(){
        while(true){
            yield return new WaitForSeconds(1f);
            anykeyPrompt.SetActive(!anykeyPrompt.activeSelf);
        }
    }
}
