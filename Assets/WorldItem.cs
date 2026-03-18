using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public InventoryItem item;

    public void Init(InventoryItem newItem)
    {
        item = newItem;
    }
}