using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.Random;

namespace controllers
{
    public class StoryController : MonoBehaviour
    {
        private GameObject _chickenPrefab;
        private GameObject _firePrefab;
        private GameObject _backgroundPrefab;
        private List<Transform> _fires;
        private List<MobController> _chickens;
        private BackgroundController _background;
        private float _xBounds = 5f;
        private float _yBounds = 3f;
    
        [Inject]
        private static StoryController _instance;
    
        // Start is called before the first frame update
        void Start()
        {
            _fires = new List<Transform>();
            _chickens = new List<MobController>();
            // _background = _backgroundPrefab.GetComponent<BackgroundController>();
            SpawnBackground();
            SpawnFire();
            SpawnChicken();
        }

        public void Init(GameObject chickenPrefab, GameObject firePrefab, GameObject backgroundPrefab)
        {
            _chickenPrefab = chickenPrefab;
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

        // Update is called once per frame
        void Update()
        {
            // PH
            if (Input.GetKeyDown(KeyCode.Q)) SymbolWasMatched();
            if(Input.GetKeyDown(KeyCode.P)) SymbolWasMisMatched();
            if(Input.GetKeyDown(KeyCode.T)) ReachedWinState();
            if(Input.GetKeyDown(KeyCode.Y)) ReachedLoseState();
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

        public void ReachedWinState()
        {
            Debug.Log("Reached win state");
        }

        public void ReachedLoseState()
        {
            Debug.Log("Reached lose state");
        }

        void SpawnBackground()
        {
            _background = Instantiate(_backgroundPrefab).GetComponent<BackgroundController>();
        }

        void SpawnChicken()
        {
            // Debug.Log("Spawned a chicken");
            var newChicken = Instantiate(_chickenPrefab, new Vector3(-4, 0, 0), Quaternion.identity);
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
            float x = Range(-_xBounds, _xBounds);
            float y = Range(-_yBounds, _yBounds);
            return new Vector2(x, y);
        }
    }
}
