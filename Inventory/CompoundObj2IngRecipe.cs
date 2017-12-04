using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundObj2IngRecipe : ICompoundObjRecipe
{

    private int[] basicIngredients;
    private ResultingCompoundIngredient compoundFinalObj;

    public CompoundObj2IngRecipe(int[] BasicIngredients, ResultingCompoundIngredient compoundFinalObj)
    {
        this.basicIngredients = BasicIngredients;
        this.compoundFinalObj = compoundFinalObj;
    }

    public ResultingCompoundIngredient MixIngredients(int idObjAdded, int idObjInventory)
    {
        if (idObjAdded == basicIngredients[1] && idObjInventory == basicIngredients[0] || idObjAdded == basicIngredients[0] && idObjInventory == basicIngredients[1])
        {
            return compoundFinalObj;
        }
        return new ResultingCompoundIngredient(999, null);
    }
}