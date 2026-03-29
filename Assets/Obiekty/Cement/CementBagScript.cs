using UnityEngine;
using TMPro;



public class CementBagScript : MonoBehaviour, IItem, Bag
{
    public TextMeshProUGUI objectName;
    public TextMeshProUGUI objectAmmount;

    private float weight =25f;
    private string ingridiendName = "Cement";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectName.text = GetName();
        objectAmmount.text = GetWeight().ToString() + " kg";
    }

    // Update is called once per frame
    void Update()
    {

    }
    public string GetName()
    {
        return ingridiendName;
    }

    public float GetWeight()
    {
        return weight;
    }
}
