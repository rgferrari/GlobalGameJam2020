using System.Collections;
using UnityEngine;
using Zenject;

namespace scene
{
  public class IntroScene : MonoBehaviour
  {
    public GameObject starfieldPrefab;
    public GameObject shootingStarPrefab;
    public GameObject rockPrefab;
    public GameObject swirlPrefab;

    public Transform cam; 
    public Transform ship;
    public Transform shipMount;
    public Transform vanishingPoint;
    public Transform nearClipPoint;
    public Transform origin;

    public float jitterFactor;
    public Vector2 spawnWindow;
    
    void Start()
    {
      StartCoroutine(FixedSpeedOnVector(2f, ship, shipMount.position - ship.position));
      FindObjectOfType<AudioManager>().Play("ship_rumble");
      LaunchDebris(starfieldPrefab, 1f, 0.5f, 0.5f, 0.5f, 0f);
      StartCoroutine(SpaceDebris(starfieldPrefab, 20f, 1f, 0.5f, 0.5f, 0.5f, 0f));
      StartCoroutine(SpaceDebris(rockPrefab, 4f, 1f, 0.1f, 5f, 1f, 1f));
      StartCoroutine(SpaceDebris(shootingStarPrefab, 1f, 1f, 0.1f, 10f, 1f, 0f));
      StartCoroutine(SpaceDebris(starfieldPrefab, 20f, 5f, 1f, 2f, 0.5f, 0f)); // not sure bout this
    }

    void Update()
    {
            /*
      if (Input.anyKey)
      {
        StopAllCoroutines();
        Debug.Log("Load next scene, or something");
        
      }
      */
    }

    IEnumerator SpaceDebris(GameObject prefab, float frequency, float startScale, float endScale, float speed, float transparency, float rotate)
    {
      float time = Jitter(frequency);
      float timer = 0f;
      
      // get travel time
      while (true)
      {
        timer += Time.fixedDeltaTime;
        if (timer > time)
        {
          LaunchDebris(prefab, startScale, endScale, speed, transparency, rotate);
          time = Jitter(frequency);
          timer = 0f;
        }
        yield return new WaitForFixedUpdate();
      }
    }

    void LaunchDebris(GameObject prefab, float startScale, float endScale, float speed, float transparency, float rotate)
    {
      // Debug.Log("Launching debris");
      var go = SpawnNearPoint(prefab, nearClipPoint);
      var rend = go.GetComponentInChildren<SpriteRenderer>();
      var vector = vanishingPoint.position - go.transform.position;
      go.transform.GetChild(0).LookAt(vanishingPoint, Vector3.up);
      float travelTime = TravelTime(go.transform, vanishingPoint, speed);
      StartCoroutine(FadeIn(0.5f, transparency, rend)); // pass transparency
      StartCoroutine(FixedSpeedOnVector(speed, go.transform, vector));
      if (rotate < 0f || rotate > 0f) StartCoroutine(Rotate(180 * rotate, 90f * rotate, go.transform.GetChild(0)));
      StartCoroutine(Scale(startScale, endScale, travelTime, go.transform));
      StartCoroutine(WaitAndThen(travelTime, FadeOut(0.5f, rend)));
      StartCoroutine(WaitAndThen(travelTime + 1f, Dispose(go)));
    }
    
    IEnumerator FadeIn(float fadeLength, float transparency, SpriteRenderer rend)
    {
      var transparent = new Color(1, 1, 1, 0f);
      rend.color = transparent;
      var fadedIn = new Color(1, 1, 1, transparency);
      var timer = 0f;
      Debug.Log("Fade in");
      while (timer < fadeLength)
      {
        timer += Time.fixedDeltaTime;
        rend.color = Color.Lerp(transparent, fadedIn, timer / fadeLength);
        yield return new WaitForFixedUpdate();
      }
      
    }

    IEnumerator FadeOut(float fadeLength, SpriteRenderer rend)
    {
      Debug.Log("Fade out");
      var fadedIn = rend.color;
      var fadedOut = new Color(1, 1, 1, 0f);
      var timer = 0f;
      while (timer < fadeLength)
      {
        timer += Time.fixedDeltaTime;
        rend.color = Color.Lerp(fadedIn, fadedOut, timer / fadeLength);
        yield return new WaitForFixedUpdate();
      }

    }

    IEnumerator Dispose(GameObject go)
    {
      
      Destroy(go, 1f);
      yield return null;
    }

    IEnumerator FixedSpeedOnVector(float speed, Transform traveler, Vector3 travelVector)
    { 
      // Vector3 travelVector = dest.position - traveler.position;
      var distance = travelVector.magnitude;
      while (distance > 0)
      {
        var moveDelta = travelVector.normalized * (speed * Time.deltaTime);
        distance -= moveDelta.magnitude;
        traveler.Translate(moveDelta);
        yield return new WaitForEndOfFrame();
      }
    }

    IEnumerator WaitAndThen(float waitTime, IEnumerator toRunNext)
    {
      var timer = 0f;
      while (timer < waitTime)
      {
        timer += Time.fixedDeltaTime;
        yield return new WaitForFixedUpdate();
      }
      StartCoroutine(toRunNext);
    }

    GameObject SpawnAtPoint(GameObject prefab, Transform spawnPoint)
    {
      var go = Instantiate(prefab, spawnPoint);
      go.transform.SetParent(origin);
      return go;
    }

    GameObject SpawnNearPoint(GameObject prefab, Transform spawnPoint)
    {
      var go = SpawnAtPoint(prefab, spawnPoint);
      go.transform.Translate(new Vector3(
        Random.Range(-spawnWindow.x, spawnWindow.x), Random.RandomRange(-spawnWindow.y, spawnWindow.y), 0f));
      return go;
    }

    IEnumerator EaseToPoint(float speed, Transform traveler, Transform dest)
    {
      yield return null;
    }

    IEnumerator Rotate(float initAngle, float tumbleSpeed, Transform actor)
    {
      Debug.Log("Rotating");
      var sprite = actor.GetChild(0).transform;
      sprite.Rotate(Vector3.forward, Jitter(initAngle));
      var tumble = Random.Range(0f, 1f) > 0.5f? Jitter(tumbleSpeed) : -Jitter(tumbleSpeed);
      
      while (true)
      {
        sprite.Rotate(Vector3.forward, tumble * Time.deltaTime);
        yield return new WaitForEndOfFrame();
      }
    }

    IEnumerator Scale(float fromScale, float toScale, float scaleTime, Transform actor)
    {
      var startScale  = new Vector3(fromScale, fromScale, fromScale);
      var endScale = new Vector3(toScale, toScale, toScale);
      actor.localScale = startScale;
      var timer = 0f;
      while (timer < scaleTime)
      {
        timer += Time.deltaTime;
        actor.localScale = Vector3.Lerp(startScale, endScale, timer / scaleTime);
        yield return new WaitForEndOfFrame();
      }
    }

    IEnumerator CamShake(float duration, float intensity)
    {
      yield return null;
    }

    private float Jitter(float value)
    {
      return value + Random.Range(value * jitterFactor, -value * jitterFactor);
    }

    private float TravelTime(Transform start, Transform end, float speed)
    {
      return (end.transform.position - start.transform.position).magnitude / speed;
    }
  }
}