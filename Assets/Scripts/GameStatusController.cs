using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatusController : MonoBehaviour{

    public static GameStatusController instance;

    [Header("Handles countdown, remaining slots, and game states")]

    public Text countdownText;
    public Text remainingSlotsText;
    public Text gameStatusText;

    [Multiline]
    [TextArea]
    public string countdownTextPrefix = "Countdown: ";

    [Multiline]
    [TextArea]
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

    public void StopCountdown(){
        countdown = false;
    }

    private void Update() {
        if (!countdown)
            return;

        // Active timer
        timer -= Time.deltaTime;
        countdownText.text = countdownTextPrefix + (int)timer;
        remainingSlotsText.text = remainingTextPrefix + remainingSlots;

        if(timer < 10 && countdown){
            countdownText.color = Color.red;
            countdownText.text = countdownTextPrefix + (float)((int)(timer * 10f)/10f);

            if( (int)(timer * 10f) % 10 == 0){
                countdownText.text = countdownTextPrefix + (float)((int)(timer * 10f) / 10f) + ".0";
            }
        }

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
