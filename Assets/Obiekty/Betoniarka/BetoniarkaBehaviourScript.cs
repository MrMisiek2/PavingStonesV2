using UnityEngine;

public class BetoniarkaBehaviourScript : MonoBehaviour,IInteractable
{
    public GameObject ramiona;
    [SerializeField] private float rotationSpeed = 60f; // stopnie na sekundê
    [SerializeField] public bool isActive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        ramiona.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    public void SetActive(bool state)
    {
        isActive = state;
    }

    public void Interact()
    {
        isActive = !isActive;
        Debug.Log("Maszyna: " + (isActive ? "W£¥CZONA" : "WY£¥CZONA"));
    }
    public string GetInteractText()
    {
        return isActive ? "Naciœnij 'E', aby wy³¹czyæ"
                        : "Naciœnij 'E', aby w³¹czyæ";
    }

}
