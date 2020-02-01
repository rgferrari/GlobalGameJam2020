using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LogicController : MonoBehaviour{

    public static LogicController instance;
    public CanvasGroup mainGamePanel;
    public bool gameStart = false;

    /*
    public LogicController Instance() {
        if (_instance == null) _instance = this;
        return _instance;
    }*/

    private void Awake() {
        instance = instance ? instance : this;
        DontDestroyOnLoad(instance);
    }

    public void StartMainGame() {
        mainGamePanel.gameObject.SetActive(true);
        mainGamePanel.alpha = 0f;
        mainGamePanel.DOFade(1f, 0.5f);
        GameStatusController.instance.StartCountdown();
    }

    public void SymbolWasMatched() {
        //Debug.Log("Symbol was matched");

        // Actions
        GameStatusController.instance.remainingSlots--;
        ButtonSpawner.instance.ButtonPressed();

        // Win if no more remaining slots left
        if (GameStatusController.instance.remainingSlots <= 0)
            ReachedWinState();

        StoryController.instance.SymbolWasMatched();
    }

    public void SymbolWasMisMatched() {
        //Debug.Log("Symbol was mismatched");

        // Actions
        GameStatusController.instance.remainingSlots += 5;
        ButtonSpawner.instance.ButtonPressed();
        PenaltyController.instance.TriggerPenalty(0.3f);
        CanvasShaker.instance.Shake(100f, 0.25f);

        StoryController.instance.SymbolWasMisMatched();
    }

    public void ReachedWinState() {
        //Debug.Log("Reached win state");

        // Actions
        StoryController.instance.ReachedWinState();
    }

    public void ReachedLoseState() {
        //Debug.Log("Reached lose state");

        // Actions
        StoryController.instance.ReachedLoseState();
    }
}
