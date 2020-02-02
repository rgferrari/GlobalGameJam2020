using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PenaltyController : MonoBehaviour{

    public static PenaltyController instance;

    [Header("Handles UI animation if mismatches")]

    public Transform gameStatusPanel;
    public GameObject penaltyTextPrefab;

    private void Awake() {
        instance = instance ? instance : this;
        DontDestroyOnLoad(this);
    }

    public void TriggerPenalty(int amount, float duration){
        StartCoroutine(PenaltyCoroutine(amount, duration));
    }

    IEnumerator PenaltyCoroutine(int amount, float duration){

        // Penalty Animation

        GameObject penaltyText = Instantiate(penaltyTextPrefab, gameStatusPanel);
        CanvasGroup penaltyTextTransparency = penaltyText.GetComponent<CanvasGroup>();
        penaltyText.GetComponent<Text>().text = "+" + amount;

        penaltyTextTransparency.alpha = 0f;


        penaltyTextTransparency.DOFade(1f, 0.15f);

        yield return new WaitForSeconds(duration);

        penaltyTextTransparency.DOFade(0f, 0.15f);
        penaltyText.transform.DOMove(penaltyText.transform.position + Vector3.down * 30f, 0.15f).OnComplete(()=>{
            Destroy(penaltyText);
        });
    }

}