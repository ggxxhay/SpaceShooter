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
    // Shopping variables
    public GameObject player;
    public Color[] skinColors;
    public int[] pricesArray;

    private string[] isBoughtKeys;
    private int[] isBoughtArray;

    private string currentSkinIndexKey;
    public int currentSkinIndex;

    // Current selected color index
    public int currentSkinSelected;

    GameObject buyButton;
    GameObject selectButton;

    Text priceUI;
    Text totalPointUI;

    // Player's saved point
    private int playerPoint;

    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        isBoughtKeys = new string[] { "isBought1", "isBought2", "isBought3", "isBought4", "isBought5" };
        pricesArray = new int[] { 0, 50, 60, 70, 80 };
        isBoughtArray = new int[5];

        currentSkinIndexKey = "skinIndex";

        // Get player saved point
        playerPoint = PlayerPrefs.GetInt(Keys.totalScoreKey);

        // Get current selected skin index
        //if (PlayerPrefs.HasKey(currentSkinIndexKey))
        //{
            currentSkinIndex = PlayerPrefs.GetInt(currentSkinIndexKey);
        //}
        //else
        //{
        //    currentSkinIndex = 0;
        //}

        currentSkinSelected = currentSkinIndex;

        priceUI = GameObject.Find("Price").GetComponent<Text>();
        totalPointUI = GameObject.Find("Point").GetComponent<Text>();

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

        GetBuyingInfo();
        UpdatePrice();
        ChangeButtonStage();

        // Set point to point UI text
        totalPointUI.text = "Your point: " + playerPoint.ToString();
    }

    // Get saved buy info from PlayerPrefs
    public void GetBuyingInfo()
    {
        for (int i = 0; i < pricesArray.Length; i++)
        {
            //if (PlayerPrefs.HasKey(isBoughtKeys[i]))
            //{
                isBoughtArray[i] = PlayerPrefs.GetInt(isBoughtKeys[i]);
            //}
            //else
            //{
            //    isBoughtArray[i] = 0;
            //}
        }
    }

    // Update price as buying info
    private void UpdatePrice()
    {
        for (int i = 0; i < isBoughtArray.Length; i++)
        {
            if (isBoughtArray[i] == 1)
            {
                pricesArray[i] = 0;
            }
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
        if (pricesArray[currentSkinIndex] == 0)
        {
            if (currentSkinIndex == currentSkinSelected)
            {
                selectButton.GetComponent<Text>().text = "Selected";
            }
            else
            {
                selectButton.GetComponent<Text>().text = "Select";
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

    // Select a bought skin
    public void SelectButtonClick()
    {
        // Set skin color info
        SkinColor.playerSkinColor = skinColors[currentSkinIndex];

        currentSkinSelected = currentSkinIndex;

        selectButton.GetComponent<Text>().text = "Selected";
    }

    // Reduce price, hide buy button when user buy a skin
    public void BuyButtonCLick()
    {
        if(playerPoint >= pricesArray[currentSkinIndex])
        {
            playerPoint -= pricesArray[currentSkinIndex];
            pricesArray[currentSkinIndex] = 0;

            // Define that this skin was bought
            isBoughtArray[currentSkinIndex] = 1;

            buyButton.SetActive(false);
            selectButton.SetActive(true);

            selectButton.GetComponent<Text>().text = "Select";

            // Set point to point UI text
            totalPointUI.text = "Your point: " + playerPoint.ToString();
        }
        else
        {
            // Not enough point to buy skin
        }
    }

    // Save information
    public void SaveInfo()
    {
        // Save prices
        for (int i = 0; i < isBoughtArray.Length; i++)
        {
            PlayerPrefs.SetInt(isBoughtKeys[i], isBoughtArray[i]);
        }

        // Save current skin index
        PlayerPrefs.SetInt(currentSkinIndexKey, currentSkinSelected);

        print("Code run");
    }
}
