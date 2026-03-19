using UnityEngine;

public class SandBagScript : MonoBehaviour, IItem,Bag
{
    private float weight =25f;
    private string ingridiendName = "Piasek";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
