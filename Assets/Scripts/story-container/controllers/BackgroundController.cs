using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace controllers
{
    public class BackgroundController : MonoBehaviour
    {
        private int _burnCount;

        public int BurnThresholdInterval;
        public GameObject BurnLayerPrefab;
        public float burnLayerFadeSpeed;
        public List<Sprite> _burnLayers;
        private List<SpriteRenderer> _rends;
        

        void Start()
        {
            _burnCount = 0;
            _rends = new List<SpriteRenderer>();
            foreach (var sprite in _burnLayers)
            {
                var newBurnLayer = Instantiate(BurnLayerPrefab, transform);
                var rend = newBurnLayer.GetComponent<SpriteRenderer>();
                _rends.Add(rend);
                rend.sprite = sprite;
                rend.color = new Color(0, 0, 0, 0);
            }
        }
        
        public void IncBurnCount()
        {
            _burnCount++;
            if (_burnCount > 0 & _burnCount % BurnThresholdInterval == 0)
            {
                var idx = Math.Min(_rends.Count - 1, _burnCount / BurnThresholdInterval - 1);
                // _rends[idx].color = new Color(0, 0, 0, 1);
                StartCoroutine(FadeAlphaTo(_rends[idx], 1));
            }
        }

        public void DecBurnCount()
        {
            _burnCount--;
        }

        IEnumerator FadeAlphaTo(SpriteRenderer rend, float targetAlpha)
        {
            var elapsed = 0f;
            var startAlpha = rend.color.a;
            var range = targetAlpha - startAlpha;
            
            while (elapsed < burnLayerFadeSpeed)
            {
                elapsed += Time.deltaTime;
                var progress = elapsed / burnLayerFadeSpeed;
                var newAlpha = startAlpha + range * progress;
                rend.color = new Color(0, 0, 0, newAlpha);
                yield return null;
            }
        }
    }
}
