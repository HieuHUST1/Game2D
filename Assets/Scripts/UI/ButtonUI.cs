using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI priceText;
    public List<int> levelPrices;
    public int currentLevel = 0;

    private bool isEnabled = true;
    public int currentPriceLevel { get; private set; }

    private void Start()
    {

    }
    private void Update()
    {
        UpdatePriceText();
    }
    public void EnableButton()
    {
        button.interactable = true;     
    }

    public void DisableButton()
    {
        button.interactable = false;
    }


    public void UpdatePriceText()
    {
        if (currentLevel + 1 < levelPrices.Count)
        {
            currentPriceLevel = levelPrices[currentLevel + 1];  
            priceText.text = currentPriceLevel.ToString();
        }
        else
        {
            priceText.text = "Max";
            DisableButton();
        }
            
    }

    public bool CheckUpgrade(int currentMoney)
    {
        if (currentLevel + 1 < levelPrices.Count && currentMoney >= levelPrices[currentLevel + 1])  
        {
            return true;  
        }
        return false;  
    }

    public void GetLevelPricesList(List<int> levelPricesList)
    {
        this.levelPrices = levelPricesList;
        UpdatePriceText();
    }


    public int ButtonOnClickUpgrade()
    {
        currentLevel++;
        print(currentLevel);
        UpdatePriceText();
        return levelPrices[currentLevel];
    }

}

