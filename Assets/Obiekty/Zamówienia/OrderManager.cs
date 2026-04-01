using System;
using UnityEngine;
using TMPro;



public class OrderManager : MonoBehaviour
{
    public Order currentOrder;
    public TextMeshProUGUI orderText;

    public int deliveredAmount = 0;
    public PlayerAccount playerAccount;


    void Start()
    {
        GenerateOrder();
    }

    void Update()
    {
        orderText.text = "Obecne zamówienie: "
            + deliveredAmount + "/" + currentOrder.requiredAmount;
    }


    public void GenerateOrder()
    {
        currentOrder = new Order
        {
            requiredAmount = UnityEngine.Random.Range(5, 15),
            pricePerUnit = UnityEngine.Random.Range(5f, 10f),
            //product = 
        };

        deliveredAmount = 0;

        Debug.Log("Nowe zamówienie: " + currentOrder.requiredAmount);
    }

    public void Deliver(int amount)
    {
        deliveredAmount += amount;

        Debug.Log("Dostarczono: " + deliveredAmount);

        if (deliveredAmount >= currentOrder.requiredAmount)
        {
            CompleteOrder();
        }
    }

    void CompleteOrder()
    {
        float earned = currentOrder.requiredAmount * currentOrder.pricePerUnit;
        playerAccount.AddMoney(earned);

        Debug.Log("Zamówienie zrealizowane! Zarobiono: " + earned);

        GenerateOrder();
    }


}