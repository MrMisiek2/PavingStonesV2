using UnityEngine;

public interface IInteractable
{
    GameObject GetGameObject();
    void Interact();
    int AddIngredient(GameObject ingredientName, float amount);
    int GetConcerte(int ammount);

    string GetInteractText(); 
    float GetInteractTextSize();
}