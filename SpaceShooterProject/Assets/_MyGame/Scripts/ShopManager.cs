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
    GameObject noticeText;

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
        currentSkinIndex = PlayerPrefs.GetInt(currentSkinIndexKey);

        currentSkinSelected = currentSkinIndex;

        priceUI = GameObject.Find("Price").GetComponent<Text>();
        totalPointUI = GameObject.Find("Point").GetComponent<Text>();

        ShopBegin();
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
        SelectButtonClick();

        // Change player skin color
        player.GetComponent<Renderer>().material.color = skinColors[currentSkinIndex];

        GetBuyingInfo();
        UpdatePrice();
        ChangeButtonStatus();

        // Set point to point UI text
        totalPointUI.text = "Your money: " + playerPoint.ToString();
    }

    /// <summary>
    /// Get saved buying information.
    /// </summary>
    public void GetBuyingInfo()
    {
        for (int i = 0; i < pricesArray.Length; i++)
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
                pricesArray[i] = 0;
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

        ChangeButtonStatus();
    }

    /// <summary>
    /// Change buy button and select button status.
    /// </summary>
    public void ChangeButtonStatus()
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

    /// <summary>
    /// Execution when select button clicked.
    /// </summary>
    public void SelectButtonClick()
    {
        // Set skin color info
        SkinColor.playerSkinColor = skinColors[currentSkinIndex];

        currentSkinSelected = currentSkinIndex;

        selectButton.GetComponent<Text>().text = "Selected";
    }

    /// <summary>
    /// Execution when buy button clicked.
    /// </summary>
    public void BuyButtonCLick()
    {
        if (playerPoint >= pricesArray[currentSkinIndex])
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
            StartCoroutine(Notify());
        }
    }

    /// <summary>
    /// Notify player about money problem.
    /// </summary>
    /// <returns></returns>
    IEnumerator Notify()
    {
        noticeText.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            noticeText.GetComponent<Text>().text = "";
            yield return new WaitForSeconds(0.15f);
            noticeText.GetComponent<Text>().text = "Not enough money!";
            yield return new WaitForSeconds(0.15f);
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
        PlayerPrefs.SetInt(currentSkinIndexKey, currentSkinSelected);

        print("Code run");
    }
}
