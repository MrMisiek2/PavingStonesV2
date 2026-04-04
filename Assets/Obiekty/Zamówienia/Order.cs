using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Order
{
    public string name;
    public List<OrderElement> elements;
    public float TotalPrice;
}