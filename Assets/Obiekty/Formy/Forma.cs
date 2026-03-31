using UnityEngine;

public interface Forma
{
    void setIsDry(bool isDrying);
    void setIsEmpty(bool isEmpty);
    bool isEmptyForm();
    bool isReadyProduct();
    ItemData GetProduct();
    float GetProductAmmount();
}
