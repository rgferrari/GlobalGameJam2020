using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonSpawner : MonoBehaviour{

    public static ButtonSpawner instance;

    public Image imageHolder1, imageHolder2;
    public int currentPlayer;
    int imageHolderFlag = 1;
    Vector3 topImagePosition, middleImagePosition, bottomImagePosition;

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
    }

    public void ButtonPressed() {

        currentPlayer = Random.Range(0, icons.Length);
        if (imageHolderFlag == 1) {
            imageHolder1.sprite = icons[currentPlayer];
        } else if (imageHolderFlag == 2) {
            imageHolder2.sprite = icons[currentPlayer];
        }

        ChangeImage();
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
