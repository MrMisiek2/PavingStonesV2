using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    public OrderManager orderManager;
    public LayerMask layerMask;
    [SerializeField]  private List<WorldItem> itemsInZone = new List<WorldItem>();

    void OnTriggerEnter(Collider other)
    {
        //if ((layerMask & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        //{

        //}
        if (other.GetComponentInParent<WorldItem>() != null)
        {
            if (other.GetComponentInParent<WorldItem>().item != null)
            {
                if (other.GetComponentInParent<WorldItem>().item.data != null)
                {
                    if (other.GetComponentInParent<WorldItem>().item.data.itemName == "Palet EURO")
                    {
                        if (!itemsInZone.Contains(other.GetComponentInParent<WorldItem>()))
                        {
                            itemsInZone.Add(other.GetComponentInParent<WorldItem>());
                            Debug.Log("Paleta dodana do strefy sprzedaży");
                        }
                        

                        
                    }
                    CheckDelivery();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        
        if (other.GetComponentInParent<WorldItem>() != null)
        {
            if (other.GetComponentInParent<WorldItem>().item != null)
            {
                if (other.GetComponentInParent<WorldItem>().item.data != null)
                {
                    if (other.GetComponentInParent<WorldItem>().item.data.itemName == "Palet EURO")
                    {
                        itemsInZone.Remove(other.GetComponentInParent<WorldItem>());
                    }
                }
            }
        }
    }

    void CheckDelivery()
    {

        foreach (OrderElement el in orderManager.currentOrder.elements)
        {


            if (orderManager.GetRequiredObject(el).prefab.GetComponent<WorldItem>() != null)
            {
                ItemData requiredObject = orderManager.GetRequiredObject(el);
                float requiredAmount = orderManager.GetRequiredAmount(el);


                //Zlicz ile towarów na placeie jest w tej strefie 
                el.itemInZone = 0;
                foreach (WorldItem item in itemsInZone)
                {
                    PaletEUROccupancy palet = item.GetComponent<PaletEUROccupancy>();
                    if (palet != null)
                    {
                        ItemData deliveredObject = palet.TakeBagInfo();
                        int deliveredAmmount = palet.GetCurrentBags();
                        //Debug.Log("deliveredObject" + deliveredObject.itemName);
                        if (requiredObject == deliveredObject)
                        {
                            orderManager.updateDeliveredAmountInZone(el, deliveredAmmount);
                            // Debug.Log("orderManager" + orderManager.deliveredAmountInZone);
                        }
                    }
                }


                //jeśli w strefie jest wystarczająco obiektów żeby je sprzedać to je usuwamy 
                if (el.itemInZone >= orderManager.GetRequiredAmount(el) && el.deliveredAmount<el.requiredAmount)
                {
                    foreach (WorldItem item in itemsInZone)
                    {
                        PaletEUROccupancy palet = item.GetComponent<PaletEUROccupancy>();
                        if (palet != null)
                        {
                            ItemData deliveredObject = palet.TakeBagInfo();

                            if (requiredObject == deliveredObject)
                            {
                                float amount = 0f;
                                if (palet.GetCurrentBags() < orderManager.GetRequiredAmount(el))
                                    amount = palet.GetCurrentBags();
                                else
                                    amount = orderManager.GetRequiredAmount(el);
                                Debug.Log("amount " + amount);
                                for (int i = 0; i < amount; i++)
                                {

                                    Debug.Log("TakeBag " + amount);
                                    ItemData item2 = palet.TakeBag();
                                }
                                orderManager.Deliver(el,amount);
                            }

                        }
                    }
                }

                //Zlicz ile towarów na placeie jest w tej strefie 
                el.itemInZone = 0;
                foreach (WorldItem item in itemsInZone)
                {
                    PaletEUROccupancy palet = item.GetComponent<PaletEUROccupancy>();
                    if (palet != null)
                    {
                        ItemData deliveredObject = palet.TakeBagInfo();
                        int deliveredAmmount = palet.GetCurrentBags();
                        //Debug.Log("deliveredObject" + deliveredObject.itemName);
                        if (requiredObject == deliveredObject)
                        {
                            orderManager.updateDeliveredAmountInZone(el, deliveredAmmount);
                            // Debug.Log("orderManager" + orderManager.deliveredAmountInZone);
                        }
                    }
                }
            }
        }


        //orderManager.setDeliveredAmount(itemsInZone.Count(obj => requiredObject));
        //if (itemsInZone.Count(obj => requiredObject) >= requiredAmount)
        //{
        //    int j = 0;
        //    // Usuwamy tylko tyle ile potrzeba
        //    for (int i = 0; i < requiredAmount+j; i++)
        //    {
        //        if (itemsInZone[i] == requiredObject)
        //        {
        //            Destroy(itemsInZone[i]); 
        //            orderManager.Deliver(1);
        //        }
        //        else
        //            j++;


        //    }

        //    // Czyścimy tylko wykorzystane elementy
        //    //itemsInZone.RemoveRange(0, requiredAmount);


        //}
    }


    //void CheckDelivery_old()
    //{
    //    GameObject requiredObject = orderManager.GetRequiredObject();
    //    int requiredAmount = (int)orderManager.GetRequiredAmount();

    //    int count = itemsInZone.Count(obj => obj == requiredObject);

    //    orderManager.setDeliveredAmount(count);

    //    if (count >= requiredAmount)
    //    {
    //        int removed = 0;

    //        // lecimy po całej liście, ale bez kombinacji z j
    //        for (int i = 0; i < itemsInZone.Count; i++)
    //        {
    //            if (itemsInZone[i] == requiredObject)
    //            {
    //                Destroy(itemsInZone[i]);
    //                orderManager.Deliver(1);

    //                removed++;

    //                if (removed >= requiredAmount)
    //                    break;
    //            }
    //        }

    //        // usuń null-e (bo Destroy nie usuwa z listy od razu)
    //        itemsInZone.RemoveAll(obj => obj == null);
    //    }
    //}


}