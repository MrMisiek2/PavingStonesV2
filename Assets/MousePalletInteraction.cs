using Unity.VisualScripting;
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
            Debug.Log("Palete hit take" + pallet);
            if (inventory.GetCurrentObject() == null)
                {
                if (pallet != null)
                {
                    //To zabiera worek palety
                    ItemData bagWorldItemPrefab = pallet.TakeBag();

                    if (bagWorldItemPrefab != null)
                    {
                        //To dodaje worek do inventory
                        inventory.AddItemToSlot(bagWorldItemPrefab, 1);
                    }
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
                if (inventory.GetCurrentObject().prefab != null)
                {
                    if (inventory.GetCurrentObject().prefab.GetComponent<Bag>() != null)
                    {
                        WorldItem bagWorldItemPrefab = pallet.AddBag(inventory.GetCurrentInventoryItem());

                        if (bagWorldItemPrefab != null)
                        {
                            //Debug.Log("Hello", bagWorldItemPrefab);
                            inventory.RemoveFromCurrentSlot();
                            //Debug.Log("Hello3");
                        }
                    }
                }
                
            }
        }
    }


}