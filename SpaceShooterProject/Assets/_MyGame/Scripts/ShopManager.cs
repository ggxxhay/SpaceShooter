using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    // Shopping variables
    public GameObject Player;
    //public Color[] SkinColors;
    public int[] PricesArray;

    SkinColor skinColorClass = new SkinColor();

    private string[] isBoughtKeys;
    private int[] isBoughtArray;

    public int currentSkinIndex;

    // Current selected color index
    public int currentSkinSelected;

    GameObject buyButton;
    GameObject selectButton;
    GameObject noticeText;

    Text priceUI;
    [HideInInspector]
    public Text TotalPointUI;

    // Player's saved point
    public static int PlayerPoint;

    // Use this for initialization
    void Start()
    {
        Player = Instantiate(Player, new Vector3(0, 0, 2.5f), new Quaternion());

        //PlayerPrefs.DeleteAll();
        isBoughtKeys = new string[] { "isBought1", "isBought2", "isBought3", "isBought4", "isBought5" };
        PricesArray = new int[] { 0, 50, 60, 70, 80 };
        isBoughtArray = new int[5];

        // Get player's saved point
        PlayerPoint = PlayerPrefs.GetInt(Keys.totalScoreKey);

        // Get current selected skin index
        currentSkinIndex = PlayerPrefs.GetInt(SkinColor.currentSkinIndexKey);

        currentSkinSelected = currentSkinIndex;

        priceUI = GameObject.Find("Price").GetComponent<Text>();
        TotalPointUI = GameObject.Find("Point").GetComponent<Text>();

        ShopBegin();
    }

    //public void LoadInitialColor()
    //{
    //    // Get current selected skin index.
    //    currentSkinIndex = PlayerPrefs.GetInt(currentSkinIndexKey);

    //    currentSkinSelected = currentSkinIndex;
        
    //    // Set skin color info.
    //    SkinColor.playerSkinColor = SkinColors[currentSkinIndex];
    //}

    private void Update()
    {
        // Update variable for Shop menu.
        if (Player.activeSelf == false)
        {
            Player.SetActive(true); 
        }
        ShopManager.PlayerPoint = PlayerPrefs.GetInt(Keys.totalScoreKey);
        GetComponent<ShopManager>().TotalPointUI.text = "Your money: " + PlayerPrefs.GetInt(Keys.totalScoreKey);
    }

    /// <summary>
    /// Shop initialize.
    /// </summary>
    public void ShopBegin()
    {
        buyButton = GameObject.Find("Buy");
        selectButton = GameObject.Find("Select");
        noticeText = GameObject.Find("Notice");

        noticeText.SetActive(false);

        // Set initial status of player's skin and UI texts
        SelectButtonClick();

        // Change player's skin color to current selected color
        Player.GetComponent<Renderer>().material.color = skinColorClass.SkinColors[currentSkinIndex];

        GetBuyingInfo();
        UpdatePrice();
        ChangeButtonStatus();

        // Set point to point UI text
        TotalPointUI.text = "Your money: " + PlayerPoint.ToString();
    }

    /// <summary>
    /// Get saved buying information.
    /// </summary>
    public void GetBuyingInfo()
    {
        for (int i = 0; i < PricesArray.Length; i++)
        {
            isBoughtArray[i] = PlayerPrefs.GetInt(isBoughtKeys[i]);
        }
    }

    /// <summary>
    /// Update price.
    /// </summary>
    private void UpdatePrice()
    {
        for (int i = 0; i < isBoughtArray.Length; i++)
        {
            if (isBoughtArray[i] == 1)
            {
                PricesArray[i] = 0;
            }
        }
    }

    /// <summary>
    /// Execution when next button clicked.
    /// </summary>
    public void NextButtonClick()
    {
        noticeText.SetActive(false);
        // Change current skin index
        if (currentSkinIndex < skinColorClass.SkinColors.Length - 1)
        {
            currentSkinIndex++;
        }
        else
        {
            currentSkinIndex = 0;
        }

        // Change player skin color
        Player.GetComponent<Renderer>().material.color = skinColorClass.SkinColors[currentSkinIndex];

        // Change prices
        priceUI.text = "Price: " + PricesArray[currentSkinIndex];

        ChangeButtonStatus();
    }

    /// <summary>
    /// Change buy button and select button status.
    /// </summary>
    public void ChangeButtonStatus()
    {
        if (PricesArray[currentSkinIndex] == 0)
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

    /// <summary>
    /// Execution when select button clicked.
    /// </summary>
    public void SelectButtonClick()
    {
        // Set skin color info
        SkinColor.playerSkinColor = skinColorClass.SkinColors[currentSkinIndex];

        currentSkinSelected = currentSkinIndex;

        selectButton.GetComponent<Text>().text = "Selected";
    }

    /// <summary>
    /// Execution when buy button clicked.
    /// </summary>
    public void BuyButtonCLick()
    {
        if (PlayerPoint >= PricesArray[currentSkinIndex])
        {
            PlayerPoint -= PricesArray[currentSkinIndex];
            priceUI.text = "Price: 0";
            PricesArray[currentSkinIndex] = 0;

            // Define that this skin was bought
            isBoughtArray[currentSkinIndex] = 1;

            buyButton.SetActive(false);
            selectButton.SetActive(true);

            selectButton.GetComponent<Text>().text = "Select";

            // Set point to point UI text
            TotalPointUI.text = "Your point: " + PlayerPoint.ToString();

            // Select recent bought skin
            SelectButtonClick();
        }
        else
        {
            StartCoroutine(FindObjectOfType<LevelManager>().Notify(noticeText, "Not enough money!"));
        }
    }

    /// <summary>
    /// Save buying and current skin information.
    /// </summary>
    public void SaveInfo()
    {
        // Save prices
        for (int i = 0; i < isBoughtArray.Length; i++)
        {
            PlayerPrefs.SetInt(isBoughtKeys[i], isBoughtArray[i]);
        }

        // Save current skin index
        PlayerPrefs.SetInt(SkinColor.currentSkinIndexKey, currentSkinSelected);

        print("Code run");
    }
}
