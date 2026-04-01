using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    public OrderManager orderManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kostka"))
        {
            orderManager.Deliver(1);
            Destroy(other.gameObject);
        }
    }
}