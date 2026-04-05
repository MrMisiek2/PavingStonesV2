using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    private ItemData item;

    public void Setup(ItemData newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        nameText.text = item.itemName;
        priceText.text = item.price.ToString();
    }

    public void OnBuy()
    {
        ShopManager.Instance.BuyItem(item);
    }
}