using NUnit.Framework.Interfaces;

[System.Serializable]
public class InventoryItem
{
    public ItemData data;
    public int amount;

    // przykładowe dynamiczne rzeczy
    public int durability;
}