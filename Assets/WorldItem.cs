using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public InventoryItem item;

    public void Initialize(InventoryItem inventoryItem)
    {
        // jeśli chcesz kopię:
        item = new InventoryItem(inventoryItem);

        // albo jeśli referencję:
        // item = inventoryItem;

        // aktualizacja wyglądu
        UpdateVisual();
    }

    void UpdateVisual()
    {
        // np. zmiana sprite na podstawie item.data.icon
    }

    //void Update()
    //{
    //    if (item == null) return;

    //    if (isEmpty == true)
    //    {
    //        Forma forma = item.data.prefab.GetComponent<Forma>();
    //        if (forma != null)
    //        {
    //            if (item.isEmpty)
    //            {
    //                forma.setIsEmpty(false);
    //            }
    //            else
    //            {
    //                forma.setIsEmpty(true);
    //            }
    //        }

    //    }
    //}

}