using OpenCover.Framework.Model;
using System;
using System.Xml.Linq;
using TMPro;
using UnityEngine;



public class OrderManager : MonoBehaviour
{
    public Order currentOrder;
    public TextMeshProUGUI orderText;

    public float deliveredAmount = 0;
    public PlayerAccount playerAccount;

    public ItemData[] itemsToSell;


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
            product = GetRandomItem()
        };

        deliveredAmount = 0;

        Debug.Log("Nowe zamówienie: " + currentOrder.requiredAmount);
    }

    public void Deliver(float amount)
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

    ItemData GetRandomItem()
    {
        if (itemsToSell.Length == 0)
            return null;

        return itemsToSell[UnityEngine.Random.Range(0, itemsToSell.Length)];
    }

    public float GetRequiredAmount()
    {
        return currentOrder.requiredAmount;
    }
    public GameObject GetRequiredObject()
    {
        return currentOrder.product.prefab;
    }

    public void setDeliveredAmount(float deliveredAmount_)
    {
        deliveredAmount = deliveredAmount_;
    }


}