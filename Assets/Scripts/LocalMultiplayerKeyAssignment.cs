using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// TODO - clean up codes, make controller mapping available

public class LocalMultiplayerKeyAssignment : MonoBehaviour{

    public Text instructionText, infoText;
    public CanvasGroup lobbyPanel;
    public Sprite[] spritesToChooseFrom;
    public Image[] playerImages;
    public GameObject[] outlines;

    int currentPlayer = 1;
    bool assignmentFinish = false;

    // KEY ASSIGNMENTS
    private void OnGUI() {
        //return;

        if (assignmentFinish)
            return;

        Event e = Event.current;
        if(e.type == EventType.KeyDown && e.isKey && e.keyCode.ToString() != "None"){

            // Ignores Escape Key
            if (e.keyCode.ToString() == "Escape")
                return;

            // Ignores Repeated Key
            for (int i = 0; i < UserInputController.instance.playerKeyCode.Length; i++){
                if (e.keyCode == UserInputController.instance.playerKeyCode[i]){
                    StartCoroutine(ShowInfoText("This key has already been assigned, pick another!", 3f));
                    return;
                }
                   
            }

            UserInputController.instance.playerKeyCode[currentPlayer - 1] = e.keyCode;
            currentPlayer++;
            if(currentPlayer >= 5){
                lobbyPanel.DOFade(0f, 0.5f).OnComplete(()=>{
                    LogicController.instance.StartMainGame();

                    // Set button spawner icons
                    ButtonSpawner.instance.icons = new Sprite[UserInputController.instance.playerKeyCode.Length];
                    for (int i = 0; i < UserInputController.instance.playerKeyCode.Length; i++) {
                        ButtonSpawner.instance.icons[i] = playerImages[i].sprite;
                    }

                    LogicController.instance.gameStart = true;

                    Destroy(gameObject);
                });
            }
        }
    }

    // Start: assign images to each player
    private void Start() {

        for (int i = 0; i < UserInputController.instance.playerKeyCode.Length; i++) {
            Sprite temp;
            do {
                temp = spritesToChooseFrom[Random.Range(0, spritesToChooseFrom.Length)];
            } while (CheckRepeatPlayerImage(temp));
            playerImages[i].sprite = temp;
        }

        // Init
        ButtonSpawner.instance.imageHolder1.sprite = playerImages[Random.Range(0, playerImages.Length)].sprite;
        ButtonSpawner.instance.imageHolder2.sprite = playerImages[Random.Range(0, playerImages.Length)].sprite;
    }

    // Return false if a repeated image is chosen
    bool CheckRepeatPlayerImage(Sprite sprite){
        for (int i = 0; i < UserInputController.instance.playerKeyCode.Length; i++) {
            if (sprite == playerImages[i].sprite)
                return true;
        }
        return false;
    }

    IEnumerator ShowInfoText(string content, float duration){
        infoText.text = content;
        yield return new WaitForSeconds(duration);
        infoText.text = "";
    }

    // Assigning player keys
    private void Update() {

        System.Array values = System.Enum.GetValues(typeof(KeyCode));
        foreach (KeyCode code in values) {
            if (Input.GetKeyDown(code)) { print(System.Enum.GetName(typeof(KeyCode), code)); }
        }


        switch(currentPlayer){
            case 1:
                instructionText.text = "Waiting for <color=red>Player 1</color> to assign their key...";
                outlines[0].SetActive(true);
                outlines[1].SetActive(false);
                outlines[2].SetActive(false);
                outlines[3].SetActive(false);
                break;
            case 2:
                instructionText.text = "Waiting for <color=cyan>Player 2</color> to assign their key...";
                outlines[0].SetActive(false);
                outlines[1].SetActive(true);
                outlines[2].SetActive(false);
                outlines[3].SetActive(false);
                break;
            case 3:
                instructionText.text = "Waiting for <color=green>Player 3</color> to assign their key...";
                outlines[0].SetActive(false);
                outlines[1].SetActive(false);
                outlines[2].SetActive(true);
                outlines[3].SetActive(false);
                break;
            case 4:
                instructionText.text = "Waiting for <color=orange>Player 4</color> to assign their key...";
                outlines[0].SetActive(false);
                outlines[1].SetActive(false);
                outlines[2].SetActive(false);
                outlines[3].SetActive(true);
                break;
            default:
                assignmentFinish = true;
                break;
        }
    }
}