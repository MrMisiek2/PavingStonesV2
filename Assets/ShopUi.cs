using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public Transform content;
    public GameObject itemUIPrefab;
    public ItemData[] items;

    void Start()
    {
        foreach (var item in items)
        {
            GameObject obj = Instantiate(itemUIPrefab, content);
            obj.GetComponent<ShopItemUI>().Setup(item);
        }
    }
}