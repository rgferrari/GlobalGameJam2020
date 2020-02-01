using UnityEngine;

public class StoryController : MonoBehaviour
{
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

    public void SymbolWasMatched()
    {
        Debug.Log("Symbol was matched");
        GameStatusController.instance.remainingSlots--;
        ButtonSpawner.instance.ButtonPressed(true);
    }

    public void SymbolWasMisMatched()
    {
        Debug.Log("Symbol was mismatched");
    }

    public void ReachedWinState()
    {
        Debug.Log("Reached win state");
    }

    public void ReachedLoseState()
    {
        Debug.Log("Reached lose state");
    }
}