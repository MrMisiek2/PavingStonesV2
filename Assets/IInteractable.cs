using UnityEngine;

public interface IInteractable
{
    void Interact();
    int  AddIngredient(GameObject ingredientName, float amount);

    string GetInteractText();
}