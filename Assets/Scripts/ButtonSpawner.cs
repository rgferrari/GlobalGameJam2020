using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonSpawner : MonoBehaviour{

    public static ButtonSpawner instance;

    public Image imageHolder1, imageHolder2;
    int imageHolderFlag = 1;
    public Vector3 topImagePosition, middleImagePosition, bottomImagePosition;

    //public float shakeMagnitude = 3f;
    public Sprite[] icons;

    private void Awake() {
        instance = instance ? instance : this;
        //DontDestroyOnLoad(_instance);
    }

    private void Start() {
        topImagePosition = imageHolder2.transform.position;
        middleImagePosition = imageHolder1.transform.position;
        bottomImagePosition = imageHolder1.transform.position - (imageHolder2.transform.position - imageHolder1.transform.position);
        imageHolder1.sprite = icons[Random.Range(0, icons.Length)]; // Init
        imageHolder2.sprite = icons[Random.Range(0, icons.Length)]; // Init
    }

    public void ButtonPressed(bool correctButtonPressed){
        if(correctButtonPressed){

            if(imageHolderFlag == 1){
                imageHolder1.sprite = icons[Random.Range(0, icons.Length)];
            }else if(imageHolderFlag == 2){
                imageHolder2.sprite = icons[Random.Range(0, icons.Length)];
            }

            ChangeImage();
        }
    }

    private void Update() {
        if(Input.anyKeyDown){
            StoryController.instance.SymbolWasMatched();
        }
    }

    public void ChangeImage(){

        // Switch positions between two image holders
        // DOMove is from DOTween, a tool to create lerp animations without calling Lerp or Coroutines
        if (imageHolderFlag == 1) {
            imageHolder1.transform.position = middleImagePosition;
            imageHolder1.transform.DOMove(bottomImagePosition, 0.15f);

            imageHolder2.transform.position = topImagePosition;
            imageHolder2.transform.DOMove(middleImagePosition, 0.15f);

        } else if (imageHolderFlag == 2) {
            imageHolder1.transform.position = topImagePosition;
            imageHolder1.transform.DOMove(middleImagePosition, 0.15f);

            imageHolder2.transform.position = middleImagePosition;
            imageHolder2.transform.DOMove(bottomImagePosition, 0.15f);
        }

        // Switch Flag
        imageHolderFlag = imageHolderFlag == 1 ? 2 : 1;
    }


}
