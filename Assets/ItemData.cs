using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ItemData : ScriptableObject
{
    //ItemData powinien zawierać dane „stałe” (definicja przedmiotu)
    public string itemName;
    public GameObject prefab;
    public int maxStack;
}