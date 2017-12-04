using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundObjectsCreator : MonoBehaviour
{
    public Sprite[] CompoundObjectsSprts;

    private Inventory inventory;
    private List<InventoryObject> inventoryObjs;
    private int numSlotsInv;

    //si se esta chequeando una composicion en el momento
    public bool IsCheckingCompositions { get; set; }

    //ids de los objetos que se comparan
    private int objInvId;
    private int objAddId;

    // si se ha encontrado una composicion
    private bool hasFoundComposition;

    private ICompoundObjRecipe[] recipes;
    ResultingCompoundIngredient compoundObj;

    private void Start()
    {
        inventory = GetComponent<Inventory>();

        inventoryObjs = inventory.Slots;
        numSlotsInv = inventoryObjs.Count;

        recipes = new ICompoundObjRecipe[4] {
            new CompoundObj3IngRecipe(new int[3] { 4, 60, 16 }, new ResultingCompoundIngredient[3] {//Cake
                         new ResultingCompoundIngredient(62, CompoundObjectsSprts[1]),
                         new ResultingCompoundIngredient(63, CompoundObjectsSprts[3]),
                         new ResultingCompoundIngredient(64, CompoundObjectsSprts[2])},
                         new ResultingCompoundIngredient(65, CompoundObjectsSprts[4])),
                  new CompoundObj3IngRecipe(new int[3] { 67, 68, 66 }, new ResultingCompoundIngredient[3] {//Varensky
                          new ResultingCompoundIngredient(69, CompoundObjectsSprts[5]),
                          new ResultingCompoundIngredient(70, CompoundObjectsSprts[7]),
                          new ResultingCompoundIngredient(71, CompoundObjectsSprts[6])},
                          new ResultingCompoundIngredient(72, CompoundObjectsSprts[8])),
            new CompoundObj3IngRecipe(new int[3] { 73, 74, 21 }, new ResultingCompoundIngredient[3] {//Borsch
                          new ResultingCompoundIngredient(75, CompoundObjectsSprts[9]),
                          new ResultingCompoundIngredient(76, CompoundObjectsSprts[11]),
                          new ResultingCompoundIngredient(77, CompoundObjectsSprts[10])},
                          new ResultingCompoundIngredient(78, CompoundObjectsSprts[12])),
            new CompoundObj2IngRecipe(new int[2] { 4, 42 }, new ResultingCompoundIngredient(61, CompoundObjectsSprts[0])) };//Eggs
    }

    /// <summary>
    /// Comprueba si al añadir un objeto este se puede combinar con otro del inventario
    /// </summary>
    /// <param name="objAdd"> Objeto nuevo que se ha añadido al inventario</param>
    /// <returns></returns>

    public IEnumerator CheckCompositions(InventoryObject objAdd)
    {
        //comprueba que no se este buscando otra composicion en el momento, si es asi, espera
        while (IsCheckingCompositions)
        {
            yield return new WaitForSeconds(0.2f);
        }

        IsCheckingCompositions = true;
        objAddId = objAdd.IDObject;

        for (int i = 0; i < numSlotsInv; i++)
        {
            objInvId = inventoryObjs[i].IDObject;
            if (objInvId != -1 && objInvId != objAddId)
            {
                hasFoundComposition = CheckTwoIngredientsComposition();
                if (hasFoundComposition)
                {
                    StartCoroutine(CreateCompoundObj(objAdd, inventoryObjs[i]));
                    break;
                }
            }
        }

        if (!hasFoundComposition)
            IsCheckingCompositions = false;
    }

    public bool CheckTwoIngredientsComposition()
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            compoundObj = recipes[i].MixIngredients(objAddId, objInvId);
            if (compoundObj.Id != 999)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Borra los objetos utiizados para la composicion y crea uno nuevo
    /// </summary>

    private IEnumerator CreateCompoundObj(InventoryObject objAdded, InventoryObject objInventory)
    {
        yield return new WaitForSeconds(0.5f);

        objInventory.RemoveObjExtended();
        objAdded.RemoveObjExtended();

        inventory.SetObject(compoundObj.SpriteInv, compoundObj.Id);

        IsCheckingCompositions = false;
        hasFoundComposition = false;
    }

}
