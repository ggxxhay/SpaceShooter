using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkinColor
{
    static public Color playerSkinColor = Color.white;
}

public class ShopManager : MonoBehaviour
{
    public Keys keys;
    //public SkinColor skinColor;

    // Shopping variables
    public GameObject player;
    public Color[] skinColors;

    private string[] priceKeys;
    public int[] pricesArray;

    private string currentSkinIndexKey;
    public int currentSkinIndex;

    // Current selected color index
    public int currentSkinSelected;

    GameObject buyButton;
    GameObject selectButton;
    Text priceUI;
    
    // Player's saved point
    private int playerPoint;

    // Use this for initialization
    void Start()
    {
        print("Start shop menu");

        priceKeys = new string[] { "price0", "price1", "price2", "price3", "price4" };
        currentSkinIndexKey = "skinIndex";

        // Get player saved point
        playerPoint = PlayerPrefs.GetInt(keys.totalScoreKey);

        // Get current selected skin index
        currentSkinIndex = PlayerPrefs.GetInt(currentSkinIndexKey);
        //currentSkinSelected = currentSkinIndex;

        priceUI = GameObject.Find("Price").GetComponent<Text>();
        
        ShopBegin();
    }

    // Shop initialize
    public void ShopBegin()
    {
        buyButton = GameObject.Find("Buy");
        selectButton = GameObject.Find("Select");

        SelectButtonClick();

        // Change player skin color
        player.GetComponent<Renderer>().material.color = skinColors[currentSkinIndex];

        GetPrice();
        ChangeButtonStage();

        Text point = GameObject.Find("Point").GetComponent<Text>();
        //int playerPoint = PlayerPrefs.GetInt("total");

        point.text = "Your point: " + playerPoint.ToString();
    }

    // Get saved prices info from PlayerPrefs
    public void GetPrice()
    {
        for (int i = 0; i < pricesArray.Length; i++)
        {
            pricesArray[i] = PlayerPrefs.GetInt(priceKeys[i]);
        }
    }

    // Change player skin color
    public void NextButtonClick()
    {
        // Change current skin index
        if (currentSkinIndex < skinColors.Length - 1)
        {
            currentSkinIndex++;
        }
        else
        {
            currentSkinIndex = 0;
        }

        // Change player skin color
        player.GetComponent<Renderer>().material.color = skinColors[currentSkinIndex];

        // Change prices
        priceUI.text = "Price: " + pricesArray[currentSkinIndex];

        ChangeButtonStage();
    }

    // Change buttons' stage: buy and select buttons
    public void ChangeButtonStage()
    {
        if (pricesArray[currentSkinIndex]==0)
        {
            if (currentSkinIndex == currentSkinSelected) {
                selectButton.GetComponent<Text>().text = "Selected";
                //selectButton.GetComponent<Button>().onClick.AddListener(null);
            }
            else
            {
                selectButton.GetComponent<Text>().text = "Select";
                //selectButton.GetComponent<Button>().onClick.AddListener(SelectButtonClick);
            }
            selectButton.SetActive(true);
            buyButton.SetActive(false);
        }
        else
        {
            selectButton.SetActive(false);
            buyButton.SetActive(true);
        }
    }

    //
    public void SelectButtonClick()
    {
        // Set skin color info
        SkinColor.playerSkinColor = skinColors[currentSkinIndex];

        currentSkinSelected = currentSkinIndex;

        selectButton.GetComponent<Text>().text = "Selected";
    }

    public void BuyButtonCLick()
    {
        playerPoint -= pricesArray[currentSkinIndex];
        pricesArray[currentSkinIndex] = 0;

        buyButton.SetActive(false);
        selectButton.SetActive(true);
    }

    // Save information
    public void SaveInfo()
    {
        // Save prices
        for (int i = 0; i < pricesArray.Length; i++)
        {
            PlayerPrefs.SetInt(priceKeys[i], pricesArray[i]);
        }

        // Save current skin index
        PlayerPrefs.SetInt(currentSkinIndexKey, currentSkinSelected);

        print("Code run");
    }
}
