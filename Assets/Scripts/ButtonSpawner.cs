using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonSpawner : MonoBehaviour{

    public Image imageHolder;

    public Sprite[] icons;

    public void ButtonPressed(bool correctButtonPressed){
        if(correctButtonPressed){
            imageHolder.sprite = icons[Random.Range(0, icons.Length)];
            StartCoroutine(ImageHolderShake());
        }
    }

    private void Update() {
        if(Input.anyKeyDown){
            ButtonPressed(true);
        }
    }

    IEnumerator ImageHolderShake(){

        Vector3 originalPosition = transform.position;

        float timeElapsed = 0f;
        float duration = 1f;

        while(timeElapsed < duration){

            transform.position = transform.position + Vector3.Normalize(new Vector3(Random.Range(-1,1), Random.Range(-1, 1), Random.Range(-1, 1)));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }

}
