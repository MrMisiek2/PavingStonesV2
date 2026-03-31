using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ItemData : ScriptableObject
{
    //ItemData powinien zawierać dane „stałe” (definicja przedmiotu)
    public string itemName; //nazwa obiektu
    public GameObject prefab; //obiekt w świecie gry
    public int maxStack; //ile można stackować razem
    public int maxOnPalette; //ile można stackować na palecie
    public float height; //wysokość obiektu
    public float length; //długość obiektu
    public float width; //szerokość obiektu
}