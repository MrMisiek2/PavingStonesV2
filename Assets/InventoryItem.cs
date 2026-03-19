using NUnit.Framework.Interfaces;

[System.Serializable]
public class InventoryItem
{
    //InventoryItem powinien zawiereać konkretne instancje w ekwipunku (stan)
    public ItemData data;
    public int amount;

    // przykładowe dynamiczne rzeczy
    public int durability;
    public bool isEmpty;
    private InventoryItem item;

    public InventoryItem() { }

    public InventoryItem(InventoryItem other)
    {
        data = other.data;
        amount = other.amount;
        durability = other.durability;
        isEmpty = other.isEmpty;
    }

    public void Initialize(InventoryItem inventoryItem)
    {
        item = inventoryItem;
    }

    void Update()
    {
        if (item == null) return;

        if (isEmpty==true)
        {
            Forma forma = item.data.prefab.GetComponent<Forma>();
            if (forma != null)
            {
                if (item.isEmpty)
                {
                    forma.setIsEmpty(false);
                }
                else
                {
                    forma.setIsEmpty(true);
                }
            }

        }
    }


}