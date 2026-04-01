using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    public OrderManager orderManager;
    public LayerMask layerMask;
    private List<GameObject> itemsInZone = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        //if ((layerMask & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        //{

        //}
        if (other.CompareTag("Kostka"))
        {
            if (!itemsInZone.Contains(other.gameObject))
                itemsInZone.Add(other.gameObject);

            
            CheckDelivery();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Kostka"))
        {
            itemsInZone.Remove(other.gameObject);
        }
    }

    void CheckDelivery()
    {
        GameObject requiredObject = orderManager.GetRequiredObject();
        float requiredAmount = orderManager.GetRequiredAmount();

        Debug.Log("Dziala");

        orderManager.setDeliveredAmount(itemsInZone.Count(obj => requiredObject));
        if (itemsInZone.Count(obj => requiredObject) >= requiredAmount)
        {
            int j = 0;
            // Usuwamy tylko tyle ile potrzeba
            for (int i = 0; i < requiredAmount+j; i++)
            {
                if (itemsInZone[i] == requiredObject)
                {
                    Destroy(itemsInZone[i]); 
                    orderManager.Deliver(1);
                }
                else
                    j++;
                    

            }

            // Czyścimy tylko wykorzystane elementy
            //itemsInZone.RemoveRange(0, requiredAmount);

            
        }
    }


    void CheckDelivery()
    {
        GameObject requiredObject = orderManager.GetRequiredObject();
        int requiredAmount = (int)orderManager.GetRequiredAmount();

        int count = itemsInZone.Count(obj => obj == requiredObject);

        orderManager.setDeliveredAmount(count);

        if (count >= requiredAmount)
        {
            int removed = 0;

            // lecimy po całej liście, ale bez kombinacji z j
            for (int i = 0; i < itemsInZone.Count; i++)
            {
                if (itemsInZone[i] == requiredObject)
                {
                    Destroy(itemsInZone[i]);
                    orderManager.Deliver(1);

                    removed++;

                    if (removed >= requiredAmount)
                        break;
                }
            }

            // usuń null-e (bo Destroy nie usuwa z listy od razu)
            itemsInZone.RemoveAll(obj => obj == null);
        }
    }


}