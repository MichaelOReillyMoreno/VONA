using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Inventario del jugador
/// </summary>

public class Inventory : MonoBehaviour
{

    public int IdDraggedObj { get; set; }
    public List<InventoryObject> Slots { get; set; }

    private CompoundObjectsCreator compoundObjs;
    private int numSlotsInInventory;

    public static Inventory Instance = null;

    void Awake()
    {
        Instance = this;

        IdDraggedObj = -1;

        compoundObjs = GetComponent<CompoundObjectsCreator>();
        Slots = new List<InventoryObject>();

        foreach (RectTransform obj in GetComponent<RectTransform>())
        {
            Slots.Add(obj.GetComponent<InventoryObject>());
        }

        numSlotsInInventory = Slots.Count;
    }

    /// <summary>
    /// Coloca un objeto nuevo en el inventario
    /// </summary>
    /// <param name="img"> imagen del objeto</param>
    /// <param name="id"> id del objeto</param>
    /// <returns>Si se puede colocar o no en el inventario</returns>

    public bool SetObject(Sprite img, int id)
    {
        InventoryObject obj;
        for (int i = 0; i < numSlotsInInventory; i++)
        {
            obj = Slots[i].GetComponent<InventoryObject>();
            if (obj.IDObject == -1)
            {
                obj.PutObj(img, id);
                StartCoroutine(compoundObjs.CheckCompositions(obj));
                return true;
            }
        }
        return false;
    }

    public void RemoveDraggedObjFromInv()
    {
        for (int i = 0; i < numSlotsInInventory; i++)
        {
            if (Slots[i].IDObject == IdDraggedObj)
            {
                Slots[i].RemoveObj();
                IdDraggedObj = -1;
                break;
            }
        }
    }

    public void RelocateDraggedObjInInv()
    {
        for (int i = 0; i < numSlotsInInventory; i++)
        {
            if (Slots[i].IDObject == IdDraggedObj)
            {
                Slots[i].ReActiveObj();
                IdDraggedObj = -1;
                break;
            }
        }
    }

    /// <summary>
    /// Comprueba si un objeto esta ya en en el inventario
    /// </summary>
    /// <param name="id"> id del objeto</param>

    public bool ContainsThisObj(int id)
    {
        for (int i = 0; i < numSlotsInInventory; i++)
        {
            if (Slots[i].IDObject == id)
            {
                return true;
            }
        }
        return false;
    }
}