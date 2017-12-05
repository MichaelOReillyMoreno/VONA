using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que representa la plancha
/// </summary>

public class Iron : InteractiveObject
{
    [SerializeField]
    private Sprite ironedClothesSprt;

    private void Start()
    {
        IsNecessaryDraggObjToInteract = true;
    }
    /// <summary>
    /// Planchar la ropa
    /// </summary>
    /// <returns>Si borra objeto arrastrado del inventario</returns>

    public override bool InteractWith()
    {
        if (Inventory.Instance.IdDraggedObj == 99)
        {
            StartCoroutine(IroningClothes());
            return true;
        }
        return false;
    }

    private IEnumerator IroningClothes()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeInOut.Instance.FadeInOutCo(1, 4));

        yield return new WaitForSeconds(2);
        Inventory.Instance.SetObject(ironedClothesSprt, 100);
    }
}
