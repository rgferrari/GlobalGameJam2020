using System.Collections;
using UnityEngine;

namespace controllers
{
    public class MobController : MonoBehaviour
    {
        public float WalkSpeed;
        public float DistanceToGoal;
        public float WanderRange;
        public float timeBeforeWander;
        public GameObject cloudPrefab;
        
        private Transform _goal;
        private Vector3 _lastPos;
        private bool _wandering;

        void Awake()
        {
            _lastPos = transform.position;
        }

        void Update()
        {
            if (HasGoal() && IsAtGoal())
            {
                // Debug.Log("Fighting fire");
                FightFire();
            }
            else if (!HasGoal() && !_wandering) Wander();
            CheckFacing();
            _lastPos = transform.position;
        }

        void CheckFacing()
        {
            var isMovingRight = transform.position.x > _lastPos.x;
            if (isMovingRight && transform.localScale.x < 0 || !isMovingRight && transform.localScale.x > 0)
            {
                Debug.Log("Flip");
                var localScale = transform.localScale;
                localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                transform.localScale = localScale;
            }
        }

        void FaceToward(Transform that)
        {
            var facingRight = transform.position.x > 0f;
            var thatIsToMyRight = transform.position.x < that.position.x;
            if (facingRight && !thatIsToMyRight || !facingRight && thatIsToMyRight)
            {
                var localScale = transform.localScale;
                localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                transform.localScale = localScale;
            }
        }

        public void SeekToGoal(Transform goal)
        {
            // Debug.Log("Chicken seeking to goal: " + goal.ToString());
            StopAllCoroutines();
            _wandering = false;
            _goal = goal;
            StartCoroutine(SeekTo());
        }

        IEnumerator SeekTo()
        {
            var vectorToGoal = _goal.position - transform.position;
            while (vectorToGoal.magnitude > DistanceToGoal)
            {
                vectorToGoal = _goal.position - transform.position;
                transform.Translate(vectorToGoal * (WalkSpeed * Time.deltaTime));
                yield return null;
            }
        }

        bool IsAtGoal()
        {
            if (_goal == null) return false;
            return !((_goal.position - transform.position).magnitude > DistanceToGoal);
        }

        public bool HasGoal()
        {
            return _goal != null;
        }

        void FightFire()
        {
            FaceToward(_goal);
            // Debug.Log("Fighting fire");
            StopAllCoroutines();
            var cloud = Instantiate(cloudPrefab, _goal);
            cloud.transform.SetParent(StoryController.Instance().transform);
            _goal.GetComponent<FlameController>().Extinguish();
            _goal = null;
            FindObjectOfType<AudioManager>().Play("steam_hiss");
        }

        void Wander()
        {
            _wandering = true;
            StartCoroutine(DoWander());
        }

        IEnumerator DoWander()
        {
            var timer = 0f;
            while (timer < timeBeforeWander)
            {
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            var startPos = transform.position;
            while (_wandering)
            {
                while (transform.position.x < startPos.x + WanderRange)
                {
                    transform.Translate(Vector2.right * (WalkSpeed * Time.deltaTime));
                    yield return new WaitForEndOfFrame();
                }
                var scale = transform.localScale;
                transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                while (transform.position.x > startPos.x - WanderRange)
                {
                    transform.Translate(Vector2.left * (WalkSpeed * Time.deltaTime));
                    yield return new WaitForEndOfFrame();
                }
                transform.localScale = new Vector3(scale.x, scale.y, scale.z);
            }
        }
    }
}
