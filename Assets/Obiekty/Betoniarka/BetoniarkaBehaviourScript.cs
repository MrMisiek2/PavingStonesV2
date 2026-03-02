using UnityEngine;
using System.Collections.Generic;

public class BetoniarkaBehaviourScript : MonoBehaviour,IInteractable
{
    public GameObject ramiona;
    [SerializeField] private float rotationSpeed = 60f; // stopnie na sekundê
    [SerializeField] public bool isActive = true;
    [SerializeField] public GameObject infill;
    [SerializeField] private float infillAmmount;

    [System.Serializable]
    public class Ingredient
    {
        public string name;
        public float amount;
    }

    public List<Ingredient> ingredients = new List<Ingredient>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        infillAmmount = GetTotalWeight();
 
        if (infillAmmount == 0)
        {
            Vector3 pos = infill.transform.localPosition;
            pos.y = 1.05f;
            infill.transform.localPosition = pos;
        }

        if (infillAmmount == 250)
        {
            Vector3 pos = infill.transform.localPosition;
            pos.y = 1.75f;
            infill.transform.localPosition = pos;
        }

        if (infillAmmount > 0 && infillAmmount < 250)
        {
            Vector3 pos = infill.transform.localPosition;
            pos.y = 1.05f + ((0.4f * infillAmmount) / 100 * 0.7f);
            Debug.Log("pos.y "+pos.y) ;
            infill.transform.localPosition = pos;
        }


        if (!isActive) return;
            
        ramiona.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
       
        


    }

    public void AddIngredient(GameObject ingredientName, float amount)
    {
        IItem item = ingredientName.GetComponent<IItem>(); 
        // sprawdzamy czy sk³adnik ju¿ istnieje
        Ingredient existing = ingredients.Find(i => i.name == item.GetName());

        if (existing != null)
        {
            existing.amount += amount;
        }
        else
        {
            Ingredient newIngredient = new Ingredient();
            newIngredient.name = item.GetName();
            newIngredient.amount = amount;
            ingredients.Add(newIngredient);
        }

        Debug.Log("Dodano " + amount + " " + ingredientName);
    }

    public void SetActive(bool state)
    {
        isActive = state;
    }

    public void Interact()
    {
        isActive = !isActive;
        Debug.Log("Maszyna: " + (isActive ? "W£¥CZONA" : "WY£¥CZONA"));
    }
    public string GetInteractText()
    {
        return isActive ? "W³¹cz 'E'"
                        : "Wy³¹cz 'E'";
    }

    public float GetTotalWeight()
    {
        float total = 0f;

        foreach (var ingredient in ingredients)
        {
            total += ingredient.amount;
            
        }

        return total;
    }

}
