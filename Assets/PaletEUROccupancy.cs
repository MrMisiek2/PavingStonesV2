using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PaletEUROccupancy : MonoBehaviour
{
    [SerializeField] private ItemData worldItemPrefab;
    [SerializeField] private Transform bagsParent;

    private int bagsPerLayer = 8;

    [SerializeField] private int currentBags = 0;

    private float bagHeight;

    private float paletteLength = 0.8f;
    private float paletteWidth = 1.2f;

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
            //Debug.Log("" + worldItemPrefab);
            //Debug.Log("" + newBagInventoryItem);
            if (newBagInventoryItem.data != worldItemPrefab) return null;
        }

        if (currentBags >= newBagInventoryItem.data.maxOnPalette)
            return null;

        //Debug.Log( "Szer "+ ((paletteLength + (0.45 * newBagInventoryItem.data.length)) / newBagInventoryItem.data.length) + "D³ug " + ((paletteWidth + (0.45 * newBagInventoryItem.data.width)) / newBagInventoryItem.data.width));
        bagHeight = newBagInventoryItem.data.height;
        int bagsPerLayerWidth = (int)((paletteWidth + (0.45 * newBagInventoryItem.data.width)) / newBagInventoryItem.data.width);
        int bagsPerLayerLength = (int)((paletteLength + (0.45 * newBagInventoryItem.data.length)) / newBagInventoryItem.data.length);
        bagsPerLayer = bagsPerLayerWidth * bagsPerLayerLength;

        //Debug.Log("Dziala" + newBagInventoryItem);
        if (worldItemPrefab == null)
            worldItemPrefab = newBagInventoryItem.data;

        int layer = currentBags / bagsPerLayer;
        int indexInLayer = currentBags % bagsPerLayer;

        Vector3 position = new Vector3(
            ((indexInLayer % bagsPerLayerWidth) * newBagInventoryItem.data.width + 0.5f * newBagInventoryItem.data.width - 0.5f* (bagsPerLayerWidth * newBagInventoryItem.data.width - paletteWidth)),
            layer * bagHeight,
            ((indexInLayer / bagsPerLayerWidth) * newBagInventoryItem.data.length + 0.5f * newBagInventoryItem.data.length - 0.5f * (bagsPerLayerLength * newBagInventoryItem.data.length - paletteLength))
        );


        GameObject bag = Instantiate(newBagInventoryItem.data.prefab, bagsParent);
        bag.transform.localPosition = position;
        WorldItem worldItem = bag.GetComponent<WorldItem>();
        worldItem.Initialize(newBagInventoryItem);
        //Debug.Log("Dziala" + bag);
        worldItem.item.amount = 1;

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
        Debug.Log("Zabierz worek z palety" + worldItem.gameObject.GetEntityId());

        lastBag.SetParent(null);
        Destroy(worldItem.gameObject); 
        
        //Destroy(lastBag.gameObject);

        currentBags--;

        return worldItemPrefab;
    }

    public ItemData TakeBagInfo() //Informacja co jest na palecie
    {
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