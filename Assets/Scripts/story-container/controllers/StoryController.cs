using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.Random;

namespace controllers
{
    public class StoryController : MonoBehaviour
    {
        private GameObject _motherCluckerPrefab;
        private GameObject _lankyPrefab;
        private GameObject _cluckingtonPrefab;
        private GameObject _flambeauPrefab;
        private GameObject _firePrefab;
        private GameObject _backgroundPrefab;
        private List<Transform> _fires;
        private List<MobController> _chickens;
        private BackgroundController _background;
        private Vector2 spawnTopLeft = new Vector2(-8f, 4f); 
        private Vector2 spawnBottomRight = new Vector2(8f, -1f); 
        private float _xBounds = 5f;
        private float _yBounds = 3f;
        private float fireSpawnTime = 4f;
        private float timer = 0f;

        private bool isGameRunning;
        
        [Inject]
        private static StoryController _instance;

        public void Init(
            GameObject motherCluckerPrefab, 
            GameObject lankyPrefab,
            GameObject cluckingtonPrefab,
            GameObject flambeauPrefab,
            GameObject firePrefab, 
            GameObject backgroundPrefab)
        {
            isGameRunning = false;
            _motherCluckerPrefab = motherCluckerPrefab;
            _lankyPrefab = lankyPrefab;
            _cluckingtonPrefab = cluckingtonPrefab;
            _flambeauPrefab = flambeauPrefab;
            _firePrefab = firePrefab;
            _backgroundPrefab = backgroundPrefab;
        }

        public static StoryController Instance()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StoryController>();
            }
            return _instance;
        }

        void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
            if (timer >= fireSpawnTime)
            {
                timer = 0f;
                if (isGameRunning) SpawnFire();
            }
        }

        public void SymbolWasMatched()
        {
            // Debug.Log("Symbol was matched");
            var chicken = _chickens.Find(c => !c.HasGoal());
            var goal = FindFireToFight(chicken.transform);
            if (goal != null) chicken.SeekToGoal(goal);
        }

        public void SymbolWasMisMatched()
        {
            Debug.Log("Symbol was mismatched");
            SpawnFire();
        }

        public void StartStory()
        {
            _fires = new List<Transform>();
            _chickens = new List<MobController>();
            SpawnBackground();
            SpawnFire();
            SpawnChicken(_motherCluckerPrefab);
            SpawnChicken(_lankyPrefab);
            SpawnChicken(_cluckingtonPrefab);
            SpawnChicken(_flambeauPrefab);
        }

        public void ReachedWinState()
        {
            Debug.Log("Reached win state");
            DespawnAllFire();
            isGameRunning = false;
        }

        private void DespawnAllFire()
        {
            foreach (var fire in _fires)
            {
                Destroy(fire.gameObject, 0.1f);
            }
        }

        public void ReachedLoseState()
        {
            Debug.Log("Reached lose state");
            isGameRunning = false;

        }

        void SpawnBackground()
        {
            _background = Instantiate(_backgroundPrefab).GetComponent<BackgroundController>();
        }

        void SpawnChicken(GameObject prefab)
        {
            // Debug.Log("Spawned a chicken");
            var newChicken = Instantiate(prefab, RandomSpawnPoint(), Quaternion.identity);
            var chickenController = newChicken.GetComponent<MobController>();
            _chickens.Add(chickenController);
        }

        void SpawnFire()
        {
            // Debug.Log("Spawned fire");
            var newFire = Instantiate(_firePrefab, RandomSpawnPoint(), Quaternion.identity);
            _fires.Add(newFire.transform);
            _background.IncBurnCount();
        }

        public Transform FindFireToFight(Transform chicken)
        {
            if (_fires.Count == 0) return null;
            return ClosestFireTo(chicken);
        }

        Transform ClosestFireTo(Transform chicken)
        {
            if (_fires.Count == 1) return _fires[0];
        
            Transform goal = _fires[0];
            float distance = float.MaxValue;
            foreach (var fire in _fires)
            {
                if ((fire.position - chicken.position).magnitude < distance)
                {
                    goal = fire;
                    distance = (fire.position - chicken.position).magnitude;
                }
            }
            return goal;
        }

        public void FireWasExtinguished(FlameController fire)
        {
            // Debug.Log("Extinguishing fire");
            _fires.Remove(fire.transform);
            _background.DecBurnCount();
            Destroy(fire.gameObject, 1f);
        }

        private Vector2 RandomSpawnPoint()
        {
            float x = Range(spawnTopLeft.x, spawnBottomRight.x);
            float y = Range(spawnBottomRight.y, spawnTopLeft.y);
            return new Vector2(x, y);
        }
    }
}
