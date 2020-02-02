using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatusController : MonoBehaviour{

    public static GameStatusController instance;

    public Text countdownText, remainingSlotsText, gameStatusText;
    public string countdownTextPrefix = "Countdown: ";
    public string remainingTextPrefix = "Remaining Slots: ";
    bool countdown = false;

    // TODO: replace timer to somewhere else (level-controlled variable)
    public float timer = 30f;
    public int remainingSlots = 30;

    void Awake(){
        instance = instance ? instance : this;
        DontDestroyOnLoad(instance);
        gameStatusText.text = "";
    }

    public void StartCountdown(){
        countdown = true;
    }

    private void Update() {
        if (!countdown)
            return;

        // Active timer
        timer -= Time.deltaTime;
        countdownText.text = countdownTextPrefix + (int)timer;
        remainingSlotsText.text = remainingTextPrefix + remainingSlots;

        // Timer reached - game over
        if(timer < 0 && countdown){
            countdown = false;
            LogicController.instance.ReachedLoseState();
        }
    }

    public void UpdateGameStatusText(string content) {
        gameStatusText.text = content;
    }
}
