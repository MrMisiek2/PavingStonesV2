using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private TextMeshProUGUI interactText;

    [SerializeField] private IInteractable currentInteractable;
    [SerializeField] private GridBuildSystem inventory;


    private GameObject currentObject;

    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }

        if (Input.GetMouseButton(1) && currentInteractable != null && inventory.GetCurrentObject() != null)
        {
            
            if(inventory.GetCurrentObject().prefab.tag != "Form")
            {
                IItem item = inventory.GetCurrentObject().prefab.GetComponent<IItem>();
                if (item != null)
                {
                    //Debug.Log("Dodanie obiektu do betoniarki: ", inventory.GetCurrentObject().prefab);
                    int status = currentInteractable.AddIngredient(inventory.GetCurrentObject().prefab, item.GetWeight());

                    if (status == 0)
                    {
                        inventory.RemoveFromCurrentSlot(1f);

                    }

                }
            }
            
        }


        //if (Input.GetMouseButton(0))
        //{ 
        //    Debug.Log("Dodanie obiektu" + currentInteractable + " test " + inventory.GetCurrentObject().data.prefab);
        //}

        if (Input.GetMouseButton(0) && currentInteractable != null && inventory.GetCurrentObject() != null)
        {
            if (inventory.GetCurrentObject().prefab.tag == "Form")
            {
                //Debug.Log("Dodanie obiektu");
                bool isEmpty = inventory.GetCurrentInventoryItem().isEmpty;

                Debug.Log("Zabranie betonu z betoniarki: " + inventory.GetCurrentObject() + "Czy forma jest pusta: " + isEmpty);
                if (isEmpty == true)
                {
                    int status = currentInteractable.GetConcerte(25);

                    if (status == 0)
                        inventory.GetCurrentInventoryItem().isEmpty = false;
                }
            }
            
        }

        //Zabranie kostki z formy i dodanie jej do EQ
        if (Input.GetMouseButton(0) && currentInteractable != null && inventory.GetCurrentObject() == null)
        {
            GameObject forma = currentInteractable.GetGameObject();

            if (forma != null)
            {
                if (forma.tag == "Form")
                {
                    Forma fForma = forma.GetComponent<Forma>();
                    if (fForma.isEmptyForm() == false && fForma.isReadyProduct())
                    {
                        inventory.AddItemToSlot(fForma.GetProduct(), fForma.GetProductAmmount());

                        WorldItem worldItem = forma.GetComponent<WorldItem>();
                        worldItem.item.isEmpty = true;
                    }
                }
            }
        }
    }

    void CheckForInteractable()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  //new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);


        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            GameObject hitObject = hit.collider.gameObject;
            //Debug.Log("Hit w interactable");


            //Jeœli zmieniamy obiekt to wy³¹czamy podpowiedz
            if (hitObject != currentObject && hitObject != null && currentObject != null)
            {
                if (currentObject != null)
                {

                    ToggleCanvas(currentObject, false);
                    currentObject = null;
                }
            }

            if (interactable != null)
            {
                currentObject = hitObject;
                ToggleCanvas(currentObject, true);


                
                currentInteractable = interactable;
                interactText.text = interactable.GetInteractText();
                interactText.fontSize = interactable.GetInteractTextSize();
                interactText.gameObject.SetActive(true);
                return;
            }
            //else
            //{
            //    if (hitObject != null)
            //    {
            //        ToggleCanvas(hitObject, false);
            //        hitObject = null;
            //    }

            //}
        }


        if (currentObject != null)
        {
            ToggleCanvas(currentObject, false);
            currentObject = null;
        }
        currentInteractable = null;
        interactText.gameObject.SetActive(false);
    }

    void ToggleCanvas(GameObject obj, bool state)
    {
        Canvas canvas = obj.GetComponentInChildren<Canvas>(true);


        //Debug.Log("currentObject: " + obj  + "statue: " + state + "canvas: " + canvas);
        
        if (canvas != null)
        {
            canvas.gameObject.SetActive(state);
        }
    }
}