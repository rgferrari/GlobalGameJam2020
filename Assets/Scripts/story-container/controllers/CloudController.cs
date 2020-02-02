using System.Collections;
using UnityEngine;

namespace controllers
{
    public class CloudController : MonoBehaviour
    {
        private Transform _cloudA;
        private Transform _cloudB;
        // Start is called before the first frame update
        
        public float animTimer;
        public float spinSpeed;
        void Awake()
        {
            _cloudA = transform.GetChild(0).transform;
            _cloudB = transform.GetChild(1).transform;
            StartCoroutine(Extinguisher());
        }

        // Update is called once per frame
        IEnumerator Extinguisher()
        {
            var timer = 0f;
            _cloudA.localScale = Vector3.zero;
            _cloudB.localScale = Vector3.zero;
            float scale;
            while (timer < animTimer)
            {
                timer += Time.deltaTime;
                scale = timer / animTimer;
                _cloudA.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, scale);
                _cloudA.Rotate(Vector3.forward, spinSpeed);
                _cloudB.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, scale);
                _cloudB.Rotate(Vector3.back, spinSpeed);
                yield return new WaitForEndOfFrame();

            }

            timer = 0f;
            while (timer < animTimer)
            {
                timer += Time.deltaTime;
                scale = timer / animTimer;
                _cloudA.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, scale);
                _cloudA.Rotate(Vector3.forward, spinSpeed);
                _cloudB.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, scale);
                _cloudB.Rotate(Vector3.back, spinSpeed);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Destroy(this.gameObject);
        }
    }
}
