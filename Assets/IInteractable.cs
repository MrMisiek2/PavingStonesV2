using UnityEngine;

public interface IInteractable
{
    void Interact();
    int AddIngredient(GameObject ingredientName, float amount);
    int GetConcerte(int ammount);

    string GetInteractText();
}