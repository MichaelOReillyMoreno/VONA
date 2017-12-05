using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Plates
{
    public const int eggs = 43;
    public const int baresky = 51;
    public const int borsk = 52;
    public const int cake = 54;
}

/// <summary>
/// Script que controla todo lo referente a comer en la mesa, colocar la comida, platos, comer, recoger los platos sucios
/// </summary>

public class EatingAreaOnTable : InteractiveObject
{
    //sprite de platos sucios que va al inventario
    [SerializeField]
    private Sprite dirtyContainerToInv;

    //recipientes calientes y sucios en la mesa
    [SerializeField]
    private GameObject[] hotPlateOnTable;

    [SerializeField]
    private SpriteRenderer[] dirtyPlateOnTable;

    //Platos sucios y limpios de la mesa
    [SerializeField]
    private GameObject dirtyDishes;

    [SerializeField]
    private GameObject cleanDishes;

    [SerializeField]
    private IrrelevantObject[] chairs;

    private bool cleanDishesOnTable;
    private bool hotFoodOnTable;

    private bool tableIsDirty;

    private int nHotPlate;
    private int idDirtyPlate;

    void Start()
    {
        nHotPlate = 0;
        tableIsDirty = false;
        IsNecessaryDraggObjToInteract = true;
    }

    /// <summary>
    /// Permite interactuar con la mesa, dependiendo de su estado, si esta vacia, con platos limpios o sucios ocurren diferentes cosas
    /// </summary>
    /// <returns>Si borra objeto arrastrado del inventario</returns>

    public override bool InteractWith()
    {
        int invObj = Inventory.Instance.IdDraggedObj;
        bool hasToRemoveObjDragged = false;

        if (!tableIsDirty)
        {
            if (!hotFoodOnTable && (invObj == Plates.eggs || invObj == Plates.baresky || invObj == Plates.borsk || invObj == Plates.cake))
            {
                ServeHotFoodOnTable(invObj);
                hasToRemoveObjDragged = true;
            }
            else if (!cleanDishesOnTable && invObj == 33)// si se intenta colocar los platos
            {
                placeDishesOnTable();
                hasToRemoveObjDragged = true;
            }
            else if (DaysManager.Instance.Day == 0)
            {
                DaysManager.Instance.RepeatMessageOnWorngActions();
            }
        }
        else if (DaysManager.Instance.Day != 0)
        {
            TakeDirtyDishesFromTable();
        }
        else
        {
            SubtitlesManager.Instance.LoadSubtitles(10);
        }

        if (cleanDishesOnTable && hotFoodOnTable)
        {
            StartCoroutine(ChangeTableToDirtyState());
        }
        return hasToRemoveObjDragged;
    }

    private void ServeHotFoodOnTable(int invObj)
    {
        switch (invObj)
        {
            case Plates.baresky:

                nHotPlate = 0;
                idDirtyPlate = 12;
                break;

            case Plates.borsk:

                nHotPlate = 1;
                idDirtyPlate = 13;
                break;

            case Plates.eggs:

                nHotPlate = 2;
                idDirtyPlate = 14;
                break;

            case Plates.cake:

                nHotPlate = 3;
                idDirtyPlate = 15;
                break;
        }

        hotPlateOnTable[nHotPlate].SetActive(true);
        hotFoodOnTable = true;
    }

    private void TakeDirtyDishesFromTable()
    {
        Inventory.Instance.SetObject(dirtyContainerToInv, idDirtyPlate);
        dirtyPlateOnTable[nHotPlate].enabled = false;
        dirtyDishes.SetActive(false);
        tableIsDirty = false;

        IsNecessaryDraggObjToInteract = true;
    }

    private void placeDishesOnTable()
    {
        cleanDishes.SetActive(true);
        cleanDishesOnTable = true;
    }

    /// <summary>
    /// Cambia de platos limpios a sucios
    /// </summary>
    /// <returns></returns>

    private IEnumerator ChangeTableToDirtyState()
    {

        SubtitlesManager.Instance.LoadSubtitles(7);
        yield return new WaitForSeconds(3.5f);

        if (DaysManager.Instance.Day == 0)
        {
            StartCoroutine(SubtitlesManager.Instance.ShowSubsInCartel(26, false));
            yield return new WaitForSeconds(2f);
            DaysManager.Instance.SetPhaseOfTheDay(DayPhase.phase6);
        }
        else
        {
            StartCoroutine(FadeInOut.Instance.FadeInOutCo(1, 4));
            yield return new WaitForSeconds(2f);
        }

        cleanDishes.SetActive(false);
        hotPlateOnTable[nHotPlate].SetActive(false);

        dirtyDishes.SetActive(true);
        dirtyPlateOnTable[nHotPlate].enabled = true;

        hotFoodOnTable = false;
        cleanDishesOnTable = false;
        tableIsDirty = true;

        chairs[0].ForceWrongPos();
        chairs[1].ForceWrongPos();

        IsNecessaryDraggObjToInteract = false;
    }
}
