using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridBuildSystem : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public LayerMask buildSurface;
    public Canvas uiCanvas;
    public Image demolishProgressBar;

    [Header("Build Prefabs (9 Slots)")]
    public GameObject[] buildPrefabs = new GameObject[9];

    [Header("Slot Prefabs (9 Slots)")]
    public GameObject[] slotsPrefabs = new GameObject[9];

    [Header("Grid Settings")]
    public float gridSize = 1f;
    public float buildDistance = 6f;
    public float rotationStep = 90f;

    [Header("Demolish Settings")]
    public float demolishHoldTime = 1f;

    [Header("Visual Grid")]
    public GameObject gridTilePrefab; // mały kwadrat z półprzezroczystym materiałem

    private int currentSlot = 0;
    private GameObject previewObject;
    private HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();
    private bool canPlace;
    private float currentRotation;
    private float heightOffset;
    private float demolishTimer = 0f;

    [Header("Obiekty do generowania")]
    public GameObject SandBag;
    public GameObject CementBag;
    public GameObject EmptyPallete;
    public GameObject EmptyBucket;
    public GameObject BucketOfWater;
    public GameObject EmptyForm;




    private GameObject[,] visualGrid = new GameObject[3, 3]; // 3x3 grid

    void Start()
    {
        //CreateVisualGrid();
    }

    void Update()
    {
        HandleItemSpawnInInventory();
        HandleSlotChange();
        HandleRotation();
        UpdatePreview();
        HandleInput();
        UpdateDemolishUI();
        //UpdateVisualGrid();
    }

    // --- SLOT ---
    void HandleSlotChange()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) currentSlot = (currentSlot + 1) % buildPrefabs.Length;
        if (scroll < 0f) currentSlot = (currentSlot - 1 + buildPrefabs.Length) % buildPrefabs.Length;

        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
    }

    // --- ROTACJA ---
    void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.Q)) currentRotation -= rotationStep;
        if (Input.GetKeyDown(KeyCode.E)) currentRotation += rotationStep;
    }

    void HandleItemSpawnInInventory()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)) buildPrefabs[currentSlot] = SandBag;
        if (Input.GetKeyDown(KeyCode.Keypad2)) buildPrefabs[currentSlot] = CementBag;
        if (Input.GetKeyDown(KeyCode.Keypad3)) buildPrefabs[currentSlot] = EmptyPallete;
        if (Input.GetKeyDown(KeyCode.Keypad4)) buildPrefabs[currentSlot] = EmptyBucket;
        if (Input.GetKeyDown(KeyCode.Keypad5)) buildPrefabs[currentSlot] = BucketOfWater;
        if (Input.GetKeyDown(KeyCode.Keypad6)) buildPrefabs[currentSlot] = EmptyForm;
    }


    // --- PREVIEW ---
    void UpdatePreview()
    {

        //Aktualizacja wybranego slotu
        GameObject currentPrefab = buildPrefabs[currentSlot];

        for (int i = 0; i < 9; i++)
        {
            GameObject currentSlotNumber = slotsPrefabs[i];
            if (currentSlot == i)
            {
                currentSlotNumber.transform.localScale = new Vector3(1.15f, 1.15f, 1f);
                Image image = currentSlotNumber.GetComponent<Image>();
                Color c = image.color;
                c.a = 0.7f;        
                image.color = c;
            }
            else
            {
                currentSlotNumber.transform.localScale = new Vector3(1f, 1f, 1f);
                Image image = currentSlotNumber.GetComponent<Image>();
                Color c = image.color;
                c.a = 0.5f;
                image.color = c;
            }
                
        }
        
        if (currentPrefab == null || (currentPrefab.tag != "Buildable" && currentPrefab.tag != "Placable")  ) { DestroyPreview(); return; }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, buildDistance, buildSurface)) { DestroyPreview(); return; }

        Vector3 snappedPos = SnapToGrid(hit.point);
        Vector2Int cell = WorldToCell(snappedPos);

        canPlace = !occupiedCells.Contains(cell);

        if (previewObject == null)
        {
            previewObject = Instantiate(currentPrefab);
            heightOffset = 0f; // CalculateHeightOffset(previewObject);
        }

        Debug.Log("currentRotationX: " + previewObject.transform.eulerAngles.x);
        snappedPos.y += heightOffset;
        previewObject.transform.position = snappedPos;
        previewObject.transform.rotation = Quaternion.Euler(previewObject.transform.eulerAngles.x, currentRotation, 0f);

        SetPreviewColor(previewObject, canPlace ? Color.blue : Color.red, 0.5f);
    }

    // --- INPUT ---
    void HandleInput()
    {
        // USUWANIE – działa ZAWSZE
        if (Input.GetMouseButton(1))
        {
            Vector3 pos2 = GetMouseWorldPosition(); // <- ważne!
            Vector2Int cell2 = WorldToCell(pos2);

            if (occupiedCells.Contains(cell2))
            {
                demolishTimer += Time.deltaTime;

                if (demolishTimer >= demolishHoldTime)
                {
                    RemoveObjectAtCell(cell2);
                    demolishTimer = 0f;
                }
            }
            else demolishTimer = 0f;
        }
        else demolishTimer = 0f;

        if (previewObject == null) return;
        if (previewObject.tag != "Buildable" && previewObject.tag != "Placable") return;

        Vector3 pos = previewObject.transform.position;
        Vector2Int cell = WorldToCell(pos);

        // Stawianie
        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            GameObject obj = Instantiate(buildPrefabs[currentSlot], pos, previewObject.transform.rotation);
            occupiedCells.Add(cell);
            RemoveFromCurrentSlot();
        }

        // Usuwanie
        //if (Input.GetMouseButton(1))
        //{
        //    if (occupiedCells.Contains(cell))
        //    {
        //        demolishTimer += Time.deltaTime;
        //        if (demolishTimer >= demolishHoldTime)
        //        {
        //            RemoveObjectAtCell(cell);
        //            demolishTimer = 0f;
        //        }
        //    }
        //    else demolishTimer = 0f;
        //}
        //else demolishTimer = 0f;
    }

    void UpdateDemolishUI()
    {
        if (demolishProgressBar == null) return;

        if (Input.GetMouseButton(1))
        {
            Vector3 pos = GetMouseWorldPosition();
            Vector2Int cell = WorldToCell(SnapToGrid(pos));

            if (occupiedCells.Contains(cell))
            {
                demolishProgressBar.fillAmount = Mathf.Clamp01(demolishTimer / demolishHoldTime);
                demolishProgressBar.enabled = true;
                return;
            }
        }

        demolishProgressBar.fillAmount = 0f;
        demolishProgressBar.enabled = false;
    }

    // --- REMOVE ---
    void RemoveObjectAtCell(Vector2Int cell)
    {
        GameObject toRemove = null;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Buildable"))
        {
            Vector3 snappedPos = SnapToGrid(obj.transform.position);
            Vector2Int objCell = WorldToCell(snappedPos);

            if (objCell == cell)
            {
                toRemove = obj;
                break;
            }
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Placable"))
        {
            Vector3 snappedPos = SnapToGrid(obj.transform.position);
            Vector2Int objCell = WorldToCell(snappedPos);

            if (objCell == cell)
            {
                toRemove = obj;
                break;
            }
        }

        if (toRemove != null)
        {
            Destroy(toRemove);
            occupiedCells.Remove(cell);
        }
    }

    // --- HELPERS ---
    float CalculateHeightOffset(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return 0f;

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers) bounds.Encapsulate(r.bounds);

        return bounds.size.y / 2f;
    }

    Vector3 SnapToGrid(Vector3 worldPos)
    {
        float x = Mathf.Round(worldPos.x / gridSize) * gridSize;
        float z = Mathf.Round(worldPos.z / gridSize) * gridSize;
        return new Vector3(x, 0f, z);
    }

    Vector2Int WorldToCell(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / gridSize);
        int z = Mathf.RoundToInt(worldPos.z / gridSize);
        return new Vector2Int(x, z);
    }

    void DestroyPreview()
    {
        if (previewObject != null) Destroy(previewObject);
    }

    void SetPreviewColor(GameObject obj, Color color, float alpha)
    {
        foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
        {
            Material m = new Material(r.material);
            color.a = alpha;
            m.color = color;

            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.renderQueue = 3000;

            r.material = m;
        }
    }

    // --- VISUAL GRID ---
    void CreateVisualGrid()
    {
        if (gridTilePrefab == null) return;

        for (int x = 0; x < 3; x++)
        {
            for (int z = 0; z < 3; z++)
            {
                GameObject tile = Instantiate(gridTilePrefab);
                tile.transform.localScale = Vector3.one * gridSize * 0.2f; // zmniejszamy do 30%
                visualGrid[x, z] = tile;
            }
        }
    }

    void UpdateVisualGrid()
    {
        if (previewObject == null) return;

        Vector3 center = SnapToGrid(previewObject.transform.position);
        //int index = 0;
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                GameObject tile = visualGrid[x + 1, z + 1];
                Vector3 pos = new Vector3(center.x + x * gridSize, 0.01f, center.z + z * gridSize);
                tile.transform.position = pos;

                Vector2Int cell = WorldToCell(pos);
                if (cell == WorldToCell(previewObject.transform.position))
                {
                    // aktywne pole ghost
                    tile.GetComponent<Renderer>().material.color = canPlace ? new Color(0f, 0.5f, 1f, 0.2f) : new Color(1f, 0f, 0f, 0.2f);
                }
                else
                {
                    // pozostałe pola grid
                    //tile.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
                    tile.GetComponent<Renderer>().enabled = false;
                }
            }
        }
    }

    public bool AddToCurrentSlot(GameObject itemPrefab)
    {
        if (buildPrefabs[currentSlot] != null)
            return false; // slot zajęty

        //if (slotsPrefabs[currentSlot] == null)
        //    return false;

        //GameObject item = Instantiate(itemPrefab, buildPrefabs[currentSlot].transform);
        //item.transform.localPosition = Vector3.zero;
        //item.transform.localRotation = Quaternion.identity;
        //item.transform.localScale = Vector3.one * 0.6f;

        buildPrefabs[currentSlot] = itemPrefab;

        return true;
    }

    public bool RemoveFromCurrentSlot()
    {
        if (buildPrefabs[currentSlot] == null)
            return false; // slot wolny

        //if (slotsPrefabs[currentSlot] == null)
        //    return false;

        //GameObject item = Instantiate(itemPrefab, buildPrefabs[currentSlot].transform);
        //item.transform.localPosition = Vector3.zero;
        //item.transform.localRotation = Quaternion.identity;
        //item.transform.localScale = Vector3.one * 0.6f;

        buildPrefabs[currentSlot] = null;

        return true;
    }

    public GameObject GetCurrentObject()
    {
        return buildPrefabs[currentSlot];
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}
