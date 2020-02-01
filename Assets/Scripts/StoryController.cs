using UnityEngine;

<<<<<<< Updated upstream
public class StoryController : MonoBehaviour
{
=======
public class StoryController : MonoBehaviour {
>>>>>>> Stashed changes
    public static StoryController instance;

    public StoryController Instance() {
        if (instance == null) instance = this;
        return instance;
    }

    private void Awake() {
        instance = this;
    }

<<<<<<< Updated upstream
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
=======
    public void SymbolWasMatched() {
        Debug.Log("Symbol was matched");
    }

    public void SymbolWasMisMatched() {
        Debug.Log("Symbol was mismatched");
    }

    public void ReachedWinState() {
        Debug.Log("Reached win state");
    }

    public void ReachedLoseState() {
>>>>>>> Stashed changes
        Debug.Log("Reached lose state");
    }
}
