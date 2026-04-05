using System;
using TMPro;
using UnityEngine;

public class PlayerAccount : MonoBehaviour
{

    public float money;
    public TextMeshProUGUI moneyText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moneyText.text = "Money: " + Math.Round(money,2) + " PLN";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMoney(float money_)
    {
        money += money_;
        moneyText.text = "Money: " + Math.Round(money, 2) + " PLN";
    }

    public float GetMoney()
    {
        return money;
    }
}
