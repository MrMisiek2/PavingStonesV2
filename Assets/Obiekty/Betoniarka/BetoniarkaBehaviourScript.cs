using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.Reflection.Emit;
using Unity.Multiplayer.Center.Common;

public class BetoniarkaBehaviourScript : MonoBehaviour,IInteractable
{
    public GameObject ramiona;
    public TextMeshProUGUI IngridientsText;
    [SerializeField] private float rotationSpeed = 60f; // stopnie na sekundê
    [SerializeField] public bool isActive = true;
    [SerializeField] public bool isReady = false;
    [SerializeField] public GameObject infill;
    [SerializeField] private float infillAmmount;
    [SerializeField] private float concerteAmmount;

    [SerializeField] private TextMeshProUGUI MixingStatus;
    public Image mixingProgressBar;
    public Image mixingProgressBarBackground;
    private float mixingTimer = 0f;
    [SerializeField] private float mixingTime = 15f;

    [System.Serializable]
    public class Ingredient
    {
        public string name;
        public float amount;
    }

    [System.Serializable]
    public class Recipe
    {
        public string name;
        public float amount;
    }

    public List<Ingredient> ingredients = new List<Ingredient>();
    public List<Recipe> Recipes = new List<Recipe>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Aktualizcja tekstu w okienku nad obiektem
        IngridientsText.text = GetDisplayText();

        UpdateVisualOccucpacy();



        if (isActive) {

            mixingTimer += Time.deltaTime;
            MixingStatus.gameObject.SetActive(true);
            MixingStatus.text = "Mieszanie...";
            mixingProgressBar.fillAmount = Mathf.Clamp01(mixingTimer / mixingTime);
            mixingProgressBar.color = Color.yellow;
            mixingProgressBar.enabled = true;
            mixingProgressBarBackground.enabled = true;
        }
        else if (isReady)
        {
            MixingStatus.gameObject.SetActive(true);
            MixingStatus.text = "GOTOWE";
            mixingProgressBar.enabled = true;
            mixingProgressBarBackground.enabled = true;
            mixingProgressBar.color = Color.green;
        }
        else
        {
            MixingStatus.gameObject.SetActive(false);
            MixingStatus.text = "";
            mixingProgressBar.enabled = false;
            mixingProgressBarBackground.enabled = false;
            mixingTimer = 0f;
            mixingProgressBar.fillAmount = 0f;
        }

        if (!isActive) return;
            
        ramiona.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

    }

    public int  AddIngredient(GameObject ingredientName, float amount)
    {
        IItem item = ingredientName.GetComponent<IItem>();
        // sprawdzamy czy sk³adnik ju¿ istnieje
        
        Ingredient existing = ingredients.Find(i => i.name == item.GetName());
        //Debug.Log("existing" + item + "ingredientName" + ingredientName);
        if (existing != null)
        {
            float sumIngridientRecipe = 0f;
            foreach (var Recipe in Recipes)
            {
                if (Recipe.name == existing.name)
                    sumIngridientRecipe += Recipe.amount;
            }

            //Nie dodajemy jeœli iloœc przekroczy przepis
            if (existing.amount >= sumIngridientRecipe) return -1;

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
        return 0;
    }

    public void SetActive(bool state)
    {
        isActive = state; 
    }

    public void Interact()
    {

        if (isActive == false)
            isActive = !isActive;
        if (isActive == true)
            StartCoroutine(TurnOffAfterTime(mixingTime));

        Debug.Log("Maszyna: " + (isActive ? "W£¥CZONA" : "WY£¥CZONA"));
    }
    public string GetInteractText()
    {
        string message = "";
        if (isActive == false && isReady == false)
            message = "W³¹cz 'E'";
        return message;
    }

    public float GetTotalWeight(string name)
    {
        float total = 0f;
        if (concerteAmmount == 0f)
        {
            if (name is null)
            {
                foreach (var ingredient in ingredients)
                {
                    total += ingredient.amount;

                }
            }
            else
            {
                foreach (var ingredient in ingredients)
                {
                    if (ingredient.name == name)
                        total += ingredient.amount;

                }
            }
        }
        else
            total = concerteAmmount;




        return total;
    }

    public string GetDisplayText()
    {
        string result="";//= objectName + "\n";

        if (!isReady)
        foreach (Recipe rec in Recipes)
        {

            result += rec.name + ": " + GetTotalWeight(rec.name) + " / " + rec.amount + "\n";
        }
        if (isReady)
        {
            result = "Beton: " + concerteAmmount + " / " + "250";
        }

        return result;
    }

    private void UpdateVisualOccucpacy()
    {
        infillAmmount = GetTotalWeight(null);

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
            //Debug.Log("pos.y " + pos.y);
            infill.transform.localPosition = pos;
        }
    }

    IEnumerator TurnOffAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        isReady = true;
        isActive = false;
        concerteAmmount = GetTotalWeight(null);
        ClearIngredients();

    }


    public void ClearIngredients()
    {
        ingredients.Clear();
    }

    public int GetConcerte(int ammount)
    {
        if (concerteAmmount > 0 && isReady == true)
        {
            concerteAmmount = concerteAmmount - ammount;
            
            if (concerteAmmount <= 0)
            {
                isReady = false;
                concerteAmmount = 0f;
            }
            return 0;

        }
        else
            return -1;

    }
    public float GetInteractTextSize() { return 24; }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

}
