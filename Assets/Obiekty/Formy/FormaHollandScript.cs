using UnityEngine;

public class FormaHollandScript : MonoBehaviour, IItem, Forma
{
    private float weight =25f;
    private string objectName = "Holland";
    [SerializeField] private bool isEmpty = true;
    [SerializeField] private bool isDry = false;

    [SerializeField] public GameObject infill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isEmpty = true;
        Debug.Log("Is empty form: " + isEmpty);
    }

    // Update is called once per frame
    void Update()
    {
        if( isEmpty == true)
        {
            infill.SetActive(false);
        }
        else
        {
            infill.SetActive(true);
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
    
    public void setIsDry(bool isDry)
    {
        isDry = this.isDry;
    }

    public void setIsEmpty(bool isEmpty)
    {
        this.isEmpty = isEmpty;
    }

    public bool isEmptyForm()
    {
        return isEmpty;
    }
}
