using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public int maxStack;
    public bool isEmpty;
}