using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public GameObject playerAccount;

    public Transform spawnPoint; // miejsce na mapie

    void Awake()
    {
        Instance = this;
    }

    public void BuyItem(ItemData item)
    {
        PlayerAccount account = playerAccount.GetComponent<PlayerAccount>();
        
        if (account.GetMoney() >= item.price)
        {
            Debug.Log("Kasa " + account.GetMoney());
            account.AddMoney(-item.price);
            SpawnItem(item);
        }
        else
        {
            Debug.Log("Za mało kasy");
        }
    }

    void SpawnItem(ItemData item)
    {
        Instantiate(item.prefab, spawnPoint.position, Quaternion.identity);
    }
}