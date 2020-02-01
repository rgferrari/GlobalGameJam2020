using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PenaltyController : MonoBehaviour{

    public static PenaltyController instance;

    public Transform gameStatusPanel;
    public GameObject penaltyTextPrefab;

    private void Awake() {
        instance = instance ? instance : this;
        DontDestroyOnLoad(this);
    }

    public void TriggerPenalty(float duration){
        StartCoroutine(PenaltyCoroutine(duration));
    }

    IEnumerator PenaltyCoroutine(float duration){

        GameObject penaltyText = Instantiate(penaltyTextPrefab, gameStatusPanel);
        CanvasGroup penaltyTextTransparency = penaltyText.GetComponent<CanvasGroup>();
        penaltyTextTransparency.alpha = 0f;


        penaltyTextTransparency.DOFade(1f, 0.15f);

        yield return new WaitForSeconds(duration);

        penaltyTextTransparency.DOFade(0f, 0.15f);
        penaltyText.transform.DOMove(penaltyText.transform.position + Vector3.down * 30f, 0.15f).OnComplete(()=>{
            Destroy(penaltyText);
        });
    }

}