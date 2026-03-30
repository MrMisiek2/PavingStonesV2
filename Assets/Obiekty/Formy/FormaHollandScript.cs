using UnityEngine;
using System.Collections;

public class FormaHollandScript : MonoBehaviour, IItem, Forma, IInteractable
{
    private float weight = 25f;
    private string objectName = "Holland";
    [SerializeField] private bool isEmpty = true;
    [SerializeField] private bool isDrying = false;
    [SerializeField] private bool isReady = false;
    [SerializeField] private bool previousEmptyState = true;
    private float dryingTimer = 0f;
    [SerializeField] private float dryingTime = 15f;
    //private bool isEmpty = true;

    [SerializeField] public GameObject infill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousEmptyState = isEmpty;
    }

    // Update is called once per frame
    void Update()
    {

        WorldItem worldItem = GetComponent<WorldItem>();
        if (worldItem != null)
        {
            if (worldItem.item.isEmpty == true)
            {
                infill.SetActive(false);
                setIsEmpty(true);
            }
            else
            {
                infill.SetActive(true);
                setIsEmpty(false);
            }
        }
        Debug.Log("Drying" + isEmpty + "previous" + previousEmptyState);
        if (previousEmptyState != isEmpty && isEmpty == false && !isReady)
        {
            Debug.Log("Drying");
            dryingTimer = 0f;
            isDrying = true;
        }
        if (isDrying)
        {
            dryingTimer += Time.deltaTime;
            StartCoroutine(TurnOffAfterTime(dryingTime));
        }
    }
    public string GetName()
    {
        return objectName;
    }

    public float GetWeight()
    {
        return weight;
    }

    public void setIsDry(bool isDrying)
    {
        isDrying = this.isDrying;
    }

    public void setIsEmpty(bool isEmpty)
    {
        this.isEmpty = isEmpty;
    }

    public bool isEmptyForm()
    {
        return isEmpty;
    }
    IEnumerator TurnOffAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        isDrying = false;
        Renderer r = infill.GetComponent<Renderer>();
        r.material.color = Color.red;
        isReady = true;
    }





    //Nagranie interactable 
    public void Interact() { }
    public int AddIngredient(GameObject ingredientName, float amount) { return 0;}
    public int GetConcerte(int ammount) { return 0; }
    public string GetInteractText() 
    {
        if (isReady)
        {
            return "Zabierz kostkę";
        }
        else if (isDrying)
        {
            return "Suszenie";
        }
        else
        {
            return "Forma pusta";
        }
    }
    public float GetInteractTextSize() { return 20; }
}
