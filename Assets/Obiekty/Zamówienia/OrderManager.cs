using OpenCover.Framework.Model;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.Audio.ProcessorInstance.AvailableData;



public class OrderManager : MonoBehaviour
{
    public Order currentOrder;
    public OrderElement currentOrderElement;
    public TextMeshProUGUI orderText;

    public PlayerAccount playerAccount;
    public int maxNumberOfElements=3;
    public int orderNumber;
    public ItemData[] itemsToSell;


    void Start()
    {
        orderNumber = 0;
        GenerateOrder();
        Debug.Log("Nowe zamówienie: " + currentOrder.name);
    }

    void Update()
    {
        orderText.text =  currentOrder.name + "\n";
        if(currentOrder.elements.Count>0)
            foreach (OrderElement el in currentOrder.elements)
            {
                orderText.text = orderText.text + el.deliveredAmount + " / " + el.requiredAmount + " " + el.product.itemName + "\n";
            }
        orderText.text = orderText.text + "Total: " + currentOrder.TotalPrice + " PLN";
    }


    public void GenerateOrder()
    {
        orderNumber++;
        currentOrder = new Order
        {
            name = "Order nr: " + orderNumber,
            elements = new List<OrderElement>()
        };
        Debug.Log("Dodanie zamówienia" + currentOrder.name);

        int NumberOfElement = 0; //obecenie generowany numer elementu zamówienia
        int maxNumberOfElement = UnityEngine.Random.Range(1, maxNumberOfElements);
        for(NumberOfElement = 0; NumberOfElement < maxNumberOfElement; NumberOfElement++)
        {
            currentOrderElement = new OrderElement
            {
                requiredAmount = UnityEngine.Random.Range(5, 15),
                deliveredAmount = 0,
                pricePerUnit = 0, //UnityEngine.Random.Range(5f, 10f),
                product = GetRandomItem(),
                itemInZone = 0
            };
            currentOrderElement.pricePerUnit = currentOrderElement.product.price;
            Debug.Log("Dodanie towaru do zamówienia");

            currentOrder.elements.Add(currentOrderElement);
        }

        currentOrder.TotalPrice = 0;
        foreach (OrderElement el in currentOrder.elements)
        {
            currentOrder.TotalPrice = currentOrder.TotalPrice + el.pricePerUnit * el.requiredAmount;
        }

        Debug.Log("Nowe zamówienie: " + currentOrder.name);
        Debug.Log("Nowe zamówienie (ilosc elementów): " + currentOrder.elements.Count);
    }

    public void Deliver(OrderElement el, float amount)
    {
        el.deliveredAmount += amount;

        Debug.Log("Dostarczono: " + el.deliveredAmount);

        foreach (OrderElement elem in currentOrder.elements)
        {
            if (elem.deliveredAmount < elem.requiredAmount)
            {
               return;
            }
        }
        CompleteOrder();
    }

    void CompleteOrder()
    {
        float earned = currentOrder.TotalPrice;
        playerAccount.AddMoney(earned);

        Debug.Log("Zamówienie zrealizowane! Zarobiono: " + earned);

        GenerateOrder();
    }

    ItemData GetRandomItem()
    {
        if (itemsToSell.Length == 0)
            return null;

        return itemsToSell[UnityEngine.Random.Range(0, itemsToSell.Length)];
    }

    public float GetRequiredAmount(OrderElement el)
    {
        return el.requiredAmount;
    }
    public ItemData GetRequiredObject(OrderElement el)
    {
        return el.product;
    }

    public void updateDeliveredAmountInZone(OrderElement el, float deliveredAmount_)
    {
        el.itemInZone = el.itemInZone + deliveredAmount_;
    }


}