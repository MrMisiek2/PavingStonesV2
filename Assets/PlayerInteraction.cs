using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private TextMeshProUGUI interactText;

    [SerializeField] private IInteractable currentInteractable;
    [SerializeField] private GridBuildSystem inventory;

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
                Debug.Log("Dodanie obiektu do betoniarki");
                currentInteractable.AddIngredient(inventory.GetCurrentObject(), item.GetWeight());
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

            if (interactable != null)
            {
                currentInteractable = interactable;
                interactText.text = interactable.GetInteractText();
                interactText.gameObject.SetActive(true);
                return;
            }
        }

        currentInteractable = null;
        interactText.gameObject.SetActive(false);
    }
}