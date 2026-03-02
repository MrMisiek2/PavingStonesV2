using UnityEngine;

public interface IInteractable
{
    void Interact();
    void AddIngredient(GameObject ingredientName, float amount);

    string GetInteractText();
}