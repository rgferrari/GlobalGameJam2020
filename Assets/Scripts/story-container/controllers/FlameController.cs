using System.Collections;
using UnityEngine;

namespace controllers
{
    public class FlameController : MonoBehaviour
    {
        public float shrinkAnimTime;
        
        public void Extinguish()
        {
            StartCoroutine(Shrink(transform.GetComponentInChildren<SpriteRenderer>()));
            // GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            StoryController.Instance().FireWasExtinguished(this);
        }

        IEnumerator Shrink(SpriteRenderer rend)    
        {
            var timer = 0f;
            while (timer < shrinkAnimTime)
            {
                timer += Time.deltaTime;
                var t = timer / shrinkAnimTime;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
                rend.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), t);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
