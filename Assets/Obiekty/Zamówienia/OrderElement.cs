using System;
using UnityEngine;

[System.Serializable]
public class OrderElement
{
    public float requiredAmount;
    public float deliveredAmount;
    public float itemInZone;
    public float pricePerUnit;
    public ItemData product;

}