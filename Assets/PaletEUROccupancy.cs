using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PaletEUROccupancy : MonoBehaviour
{
    [SerializeField] private ItemData worldItemPrefab;
    [SerializeField] private Transform bagsParent;

    private int maxLayers = 8;
    private int bagsPerLayer = 8;

    [SerializeField] private int currentBags = 0;

    private float bagHeight = 0.10f;
    private float bagSpacing = 0.3f;
    private float bagSpacingHeight = 0.45f;

    private void Start()
    {
        //Dodanie 30 worków na paletê na pocz¹tku
        //for (int i = 0; i < 30; i++)
        //    AddBag();
    }

    public WorldItem AddBag(InventoryItem newBagInventoryItem)
    {
        //Debug.Log("instatntiate" + worldItemPrefab.item.data.prefab + "test " + newBagInventoryItem);
        if  (worldItemPrefab != null )
        {
            Debug.Log("" + worldItemPrefab);
            Debug.Log("" + newBagInventoryItem);
            if (newBagInventoryItem.data != worldItemPrefab) return null;
        }
        if (currentBags >= maxLayers * bagsPerLayer) return null;

        Debug.Log("Dziala" + newBagInventoryItem);
        if (worldItemPrefab == null)
            worldItemPrefab = newBagInventoryItem.data;

        int layer = currentBags / bagsPerLayer;
        int indexInLayer = currentBags % bagsPerLayer;

        Vector3 position = new Vector3(
            (indexInLayer % 4) * bagSpacing,
            layer * bagHeight,
            (indexInLayer / 4) * bagSpacingHeight
        );


        GameObject bag = Instantiate(newBagInventoryItem.data.prefab, bagsParent);
        bag.transform.localPosition = position;
        WorldItem worldItem = bag.GetComponent<WorldItem>();
        worldItem.Initialize(newBagInventoryItem);
        Debug.Log("Dziala" + bag);
        worldItem.item.amount = 1;

        //worldItemPrefab = bag.GetComponent<WorldItem>();
        //Debug.Log("worldItem" + worldItemPrefab);
        //worldItemPrefab.Initialize(newBagInventoryItem);

        //worldItemPrefab = worldItem;

        //GameObject bag = Instantiate(worldItemPrefab.item.data.prefab, bagsParent);
        //bag.transform.localPosition = position;

        currentBags++;
        return worldItem;
    }

    //public void AddBag() //Dodaj worek na paletê
    //{
    //    if (currentBags <= 0)
    //        return;

    //    Transform lastBag = bagsParent.GetChild(bagsParent.childCount - 1);
    //    Destroy(lastBag.gameObject);

    //    currentBags--;
    //    return cementBagPrefab;
    //}

    public ItemData TakeBag() //Zabierz worek z palety
    {
        if (currentBags <= 0)
            return null;

        Transform lastBag = bagsParent.GetChild(bagsParent.childCount - 1);
        WorldItem worldItem = lastBag.GetComponent<WorldItem>();
        Destroy(worldItem.gameObject); 
        
        //Destroy(lastBag.gameObject);

        currentBags--;

        return worldItemPrefab;
    }

    public int GetCurrentBags()
    {
        return currentBags;
    }
    public void Update()
    {
        if (currentBags == 0)
        {
            worldItemPrefab = null;
        }
    }
}