using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconGenerator : MonoBehaviour
{
    public List<GameObject> tokens;
    private List<GameObject> quadrants = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        quadrants.Add(transform.Find("TL").gameObject);
        quadrants.Add(transform.Find("TC").gameObject);
        quadrants.Add(transform.Find("TR").gameObject);
        quadrants.Add(transform.Find("ML").gameObject);
        quadrants.Add(transform.Find("MC").gameObject);
        quadrants.Add(transform.Find("MR").gameObject);
        quadrants.Add(transform.Find("BL").gameObject);
        quadrants.Add(transform.Find("BC").gameObject);
        quadrants.Add(transform.Find("BR").gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GenerateIcon();
        }
    }

    private void GenerateIcon()
    {
        int cornerImage = Random.Range(0, tokens.Count);
        int sideImage = Random.Range(0, tokens.Count);
        int middleImage = Random.Range(0, tokens.Count);

        //Destroy children before instantiating new ones
        foreach (GameObject quadrant in quadrants)
        {
            foreach (Transform child in quadrant.transform)
            {
                Destroy(child.gameObject);
            }
        }

        //Instantiate the objects
        foreach (GameObject quadrant in quadrants) 
        {
            GameObject token = tokens[Random.Range(0, tokens.Count)];

            if (quadrant.name == "TL" || quadrant.name == "TR" || quadrant.name == "BL" || quadrant.name == "BR")
                token = tokens[cornerImage];
            else if (quadrant.name == "ML" || quadrant.name == "MR" || quadrant.name == "TC" || quadrant.name == "BC")
                token = tokens[sideImage];
            else if (quadrant.name == "MC")
                token = tokens[middleImage];

            var createImage = Instantiate(token) as GameObject;
            createImage.transform.SetParent(quadrant.transform, false);
        }
    }
}
