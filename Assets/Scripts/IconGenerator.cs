using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconGenerator : MonoBehaviour
{
    public List<GameObject> tokens;
    private List<GameObject> quadrants = new List<GameObject>();
    private List<Player> player = new List<Player>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            player.Add(new Player(i));            
        }

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
            GenerateIcon(player[Random.Range(0,player.Count)]);
        }
    }

    private void GenerateIcon(Player player)
    {
        int cornerImage = player.icons[0];
        int sideImage = player.icons[1];
        int middleImage = player.icons[2];

        byte red = player.rgb[0];
        byte green = player.rgb[1];
        byte blue = player.rgb[2];

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

            //Set same corners
            if (quadrant.name == "TL" || quadrant.name == "TR" || quadrant.name == "BL" || quadrant.name == "BR")
                token = tokens[cornerImage];
            //Set same sides
            else if (quadrant.name == "ML" || quadrant.name == "MR" || quadrant.name == "TC" || quadrant.name == "BC")
                token = tokens[sideImage];
            //Set Center
            else if (quadrant.name == "MC")
                token = tokens[middleImage];

            GameObject createImage = Instantiate(token) as GameObject;
            createImage.transform.SetParent(quadrant.transform, false);

         
            foreach (Transform child in quadrant.transform)
            {
                child.GetComponent<Image>().color = new Color32(red, green, blue, 255);
            }
        }
    }
}
