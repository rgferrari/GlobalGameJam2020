// using UnityEngine;
//
// public class StoryController : MonoBehaviour {
//     public static StoryController instance;
//
//     public StoryController Instance() {
//         if (instance == null) instance = this;
//         return instance;
//     }
//
//     private void Awake() {
//         instance = this;
//     }
//
//     public void SymbolWasMatched() {
//         Debug.Log("Symbol was matched");
//     }
//
//     public void SymbolWasMisMatched() {
//         Debug.Log("Symbol was mismatched");
//     }
//
//     public void ReachedWinState() {
//         Debug.Log("Reached win state");
//     }
//
//     public void ReachedLoseState() {
//         Debug.Log("Reached lose state");
//     }
// }