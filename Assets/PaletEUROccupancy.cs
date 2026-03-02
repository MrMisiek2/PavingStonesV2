using UnityEngine;

public class PaletEUROccupancy : MonoBehaviour
{
    [SerializeField] private GameObject cementBagPrefab;
    [SerializeField] private Transform bagsParent;

    private int maxLayers = 8;
    private int bagsPerLayer = 8;

    private int currentBags = 0;

    private float bagHeight = 0.10f;
    private float bagSpacing = 0.3f;
    private float bagSpacingHeight = 0.45f;

    private void Start()
    {
        //Dodanie 30 worków na paletê na pocz¹tku
        for (int i = 0; i < 30; i++)
            AddBag();
    }

    public GameObject AddBag()
    {
        if (currentBags >= maxLayers * bagsPerLayer)
            return null;

        int layer = currentBags / bagsPerLayer;
        int indexInLayer = currentBags % bagsPerLayer;

        Vector3 position = new Vector3(
            (indexInLayer % 4) * bagSpacing,
            layer * bagHeight,
            (indexInLayer / 4) * bagSpacingHeight
        );

        GameObject bag = Instantiate(cementBagPrefab, bagsParent);
        bag.transform.localPosition = position;

        currentBags++;
        return cementBagPrefab;
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

    public GameObject TakeBag() //Zabierz worek z palety
    {
        if (currentBags <= 0)
            return null;

        Transform lastBag = bagsParent.GetChild(bagsParent.childCount - 1);
        Destroy(lastBag.gameObject);

        currentBags--;

        return cementBagPrefab;
    }

    public int GetCurrentBags()
    {
        return currentBags;
    }
}