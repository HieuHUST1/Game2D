using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public ButtonUI[] uiButton;
    public TextMeshProUGUI[] textStat;
    public TextMeshProUGUI totalMoneyText;
    public int totalMoney;
    private VehicleManager vehicleManager;

    private LevelPrices levelPrices;
    public Canvas canvas;

    public int profitStat = 1;
    public float speedStat = 0.5f;
    public int carStat = 1;

    [SerializeField] private GameObject clickParticlePrefab;

    private void Awake()
    {
        levelPrices = FindObjectOfType<LevelPrices>();
        canvas = GetComponent<Canvas>();
        totalMoney = 0;
    }

    private void Start()
    {
        vehicleManager = FindObjectOfType<VehicleManager>();
        uiButton[0].GetLevelPricesList(levelPrices.profitLevelPrice);
        uiButton[1].GetLevelPricesList(levelPrices.carLevelPrice);
        uiButton[2].GetLevelPricesList(levelPrices.speedLevelPrice);

        UpdateTextStat();

        for (int i = 0; i < uiButton.Length; i++)
        {
            int index = i;
            uiButton[i].button.onClick.AddListener(() => {

                OnButtonClick(index);

                uiButton[index].transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f, 10, 1);
            });
        }
    }
    
    private void UpdateTextStat()
    {
        textStat[0].text = "x" + profitStat.ToString();
        textStat[1].text = carStat.ToString() + "/10";
        textStat[2].text = "x" + (speedStat + 0.2f).ToString();
    }

    private void Update()
    {
        UpdateTextStat();
        UpdateUI();
    }

    private void UpdateUITotalMoney()
    {
        totalMoneyText.text = totalMoney.ToString();
    }

    public void UpdateTotalMoney(int takeMoney)
    {
        totalMoney += takeMoney;

    }


    private void UpgradeSpeed()
    {
        //Debug.Log("click speed");
        int price = uiButton[2].ButtonOnClickUpgrade();

        totalMoneyText.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f, 10, 1); 
        totalMoneyText.DOColor(Color.red, 0.1f).OnComplete(() => 
        {
            totalMoneyText.DOColor(Color.white, 0.2f);  
        });

        totalMoney = totalMoney - price;
        speedStat = speedStat + 0.2f;     
    }

    private void SpawnClickParticle(Vector3 position)
    {
        GameObject particleEffect = Instantiate(clickParticlePrefab, canvas.transform); 
        particleEffect.transform.position = position; 
        Destroy(particleEffect, 1f); 
    }
    public float SetNewSpeedForVehicle()
    {
        return speedStat;
    }

    private void UpgradeCar()
    {
        //Debug.Log("click car");
        int price = uiButton[1].ButtonOnClickUpgrade();

        totalMoneyText.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f, 10, 1);
        totalMoneyText.DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            totalMoneyText.DOColor(Color.white, 0.2f);
        });
        totalMoney = totalMoney - price;
        carStat = carStat + 1;
        vehicleManager.CreateVehicle();
    }

    public int SetNewCountVehicle()
    {
        return carStat;
    }

    private void UpgradeProfit()
    {
        //Debug.Log("click profit");
        int price = uiButton[0].ButtonOnClickUpgrade();

        totalMoneyText.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f, 10, 1);
        totalMoneyText.DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            totalMoneyText.DOColor(Color.white, 0.2f);
        });
        totalMoney = totalMoney - price;
        profitStat = profitStat + 1;

    }


    private void OnButtonClick(int index)
    {

        Vector3 buttonPos = uiButton[index].transform.position;
        SpawnClickParticle(buttonPos);
        
        
        switch (index)
        {
            case 0:               
                UpgradeProfit();     
                break;
            case 1:
                UpgradeCar();
                break;
            case 2:
                UpgradeSpeed();
                break;
        }
    }

    private void UpdateUI()
    {
        UpdateUITotalMoney();

        if (uiButton[0].CheckUpgrade(totalMoney))
        {
            uiButton[0].EnableButton();
        }
        else
        {
            uiButton[0].DisableButton();
        }
        

        if (uiButton[1].CheckUpgrade(totalMoney))
        {
            uiButton[1].EnableButton();
        }
        else
        {
            uiButton[1].DisableButton();
        }

        if (uiButton[2].CheckUpgrade(totalMoney))
        {
            uiButton[2].EnableButton();
        }
        else
        {
            uiButton[2].DisableButton();
        }
        
    }
}
