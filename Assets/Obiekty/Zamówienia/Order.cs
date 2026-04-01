using System;
using UnityEngine;

[System.Serializable]
public class Order
{
    public int requiredAmount;
    public float pricePerUnit;
    public ItemData product;
}