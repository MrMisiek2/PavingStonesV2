using TMPro;
using UnityEditor;
using UnityEngine;

public class KostkaHollandScript : MonoBehaviour, IItem, Kostka
{
    public TextMeshProUGUI objectName;
    public TextMeshProUGUI objectAmmount;

    private float weight = 5f;
    private string ingridiendName = "Holland";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //objectName.text = GetName();
        //objectAmmount.text = GetWeight().ToString() + " kg";
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

