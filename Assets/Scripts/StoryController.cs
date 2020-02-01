using UnityEngine;

public class StoryController : MonoBehaviour{

    public static StoryController instance;

    /*
    public StoryController Instance()
    {
        if (_instance == null) _instance = this;
        return _instance;
    }
    */

    private void Awake() {
        instance = instance ? instance : this;
        DontDestroyOnLoad(instance);
    }

    public void SymbolWasMatched(){
        Debug.Log("Symbol was matched");

        // Actions
        GameStatusController.instance.remainingSlots--;
        ButtonSpawner.instance.ButtonPressed(true);
    }

    public void SymbolWasMisMatched(){
        Debug.Log("Symbol was mismatched");

        // Actions
        CanvasShaker.instance.Shake(50f, 0.5f);
    }

    public void ReachedWinState(){
        Debug.Log("Reached win state");

        // Actions
    }

    public void ReachedLoseState(){
        Debug.Log("Reached lose state");

        // Actions
    }
}