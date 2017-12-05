using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que representa el tendedero
/// </summary>

public class Clothesline : InteractiveObject {

    [SerializeField]
    private Sprite dryClothesSprt;

    private bool thereAreClothes;
    private bool clothesDry;

    private void Start()
    {
        IsNecessaryDraggObjToInteract = true;
    }

    /// <summary>
    /// Si se tiene algo que se pueda lavar, lo introduce
    /// </summary>

    public override bool InteractWith()
    {
        bool hasToRemoveObjDragged = false;

        if (!thereAreClothes && Inventory.Instance.IdDraggedObj == 2)
        {
            hasToRemoveObjDragged = true;

            TimeManager.Instance.CountdownDryingClothes();

            SubtitlesManager.Instance.LoadSubtitles(55);
            thereAreClothes = true;
            clothesDry = false;

            IsNecessaryDraggObjToInteract = false;
        }
        else if(thereAreClothes && !clothesDry)
        {
            SubtitlesManager.Instance.LoadSubtitles(56);
        }
        else if (thereAreClothes && clothesDry)
        {
            Inventory.Instance.SetObject(dryClothesSprt, 99);
            SubtitlesManager.Instance.LoadSubtitles(57);
            thereAreClothes = false;

            IsNecessaryDraggObjToInteract = true;
        }
        return hasToRemoveObjDragged;
    }

    /// <summary>
    /// Cuando se termina de secar la ropa cambia su estado
    /// </summary>

    public void ClothesAreDryNow()
    {
        clothesDry = true;
    }
}
