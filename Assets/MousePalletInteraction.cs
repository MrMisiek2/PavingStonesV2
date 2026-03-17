using UnityEngine;

public class MousePalletInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance = 5f;
    [SerializeField] private LayerMask palletLayer;

    [SerializeField] private GridBuildSystem inventory;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryTakeBag();
        }

        if (Input.GetMouseButtonDown(1))
        {
            TryAddBag();
        }
    }
  

    void TryTakeBag() //Zabierz worek z palety
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, palletLayer))
        {
            PaletEUROccupancy pallet = hit.collider.GetComponentInParent<PaletEUROccupancy>();

            if (pallet != null && inventory.GetCurrentObject() == null)
            {
                GameObject bagPrefab = pallet.TakeBag();

                if (bagPrefab != null)
                {
                    inventory.AddToCurrentSlot(bagPrefab);
                }
            }
        }
    }

    void TryAddBag() //Dodaj worek na paletê
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, palletLayer))
        {
            PaletEUROccupancy pallet = hit.collider.GetComponentInParent<PaletEUROccupancy>();

            if (pallet != null && inventory.GetCurrentObject() != null)
            {
                GameObject bagPrefab = pallet.AddBag(inventory.GetCurrentObject());
                
                if (bagPrefab != null)
                {
                    Debug.Log("Hello", bagPrefab);
                    inventory.RemoveFromCurrentSlot();
                    Debug.Log("Hello3");
                }
            }
        }
    }


}