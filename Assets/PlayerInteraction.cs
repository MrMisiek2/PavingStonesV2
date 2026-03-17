using UnityEngine;
using TMPro;

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
            IItem item = inventory.GetCurrentObject().GetComponent<IItem>();
            if (item != null)
            {
                Debug.Log("Dodanie obiektu do betoniarki: " , inventory.GetCurrentObject());
                int status = currentInteractable.AddIngredient(inventory.GetCurrentObject(), item.GetWeight());
                
                if (status==0)
                    inventory.RemoveFromCurrentSlot();

            }
        }

    }

    void CheckForInteractable()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            GameObject hitObject = hit.collider.gameObject;



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