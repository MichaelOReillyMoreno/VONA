public class CompoundObj3IngRecipe : ICompoundObjRecipe
{

    private int[] basicIngredients;
    private ResultingCompoundIngredient[] compoundIngredients;
    private ResultingCompoundIngredient compoundFinalObj;

    public CompoundObj3IngRecipe(int[] BasicIngredients, ResultingCompoundIngredient[] compoundIngredients, ResultingCompoundIngredient compoundFinalObj)
    {
        this.basicIngredients = BasicIngredients;
        this.compoundIngredients = compoundIngredients;
        this.compoundFinalObj = compoundFinalObj;
    }

    public ResultingCompoundIngredient MixIngredients(int idObjAdded, int idObjInventory)
    {
        if (idObjAdded == basicIngredients[2] && idObjInventory == basicIngredients[0] || idObjAdded == basicIngredients[0] && idObjInventory == basicIngredients[2])//ingredient1 y ingredient3 = mixIng1_3
        {
            return compoundIngredients[0];
        }
        else if (idObjAdded == basicIngredients[1] && idObjInventory == basicIngredients[0] || idObjAdded == basicIngredients[0] && idObjInventory == basicIngredients[1])//ingredient1 y ingredient2 = mixIng1_2
        {
            return compoundIngredients[1];
        }
        else if (idObjAdded == basicIngredients[2] && idObjInventory == basicIngredients[1] || idObjAdded == basicIngredients[1] && idObjInventory == basicIngredients[2])//ingredient2 y ingredient3 = mixIng2_3
        {
            return compoundIngredients[2];
        }
        else if ((idObjAdded == basicIngredients[0] && idObjInventory == compoundIngredients[2].Id || idObjAdded == compoundIngredients[2].Id && idObjInventory == basicIngredients[0]) || //ingredient1 y mixIng2_3
                 (idObjAdded == basicIngredients[1] && idObjInventory == compoundIngredients[0].Id || idObjAdded == compoundIngredients[0].Id && idObjInventory == basicIngredients[1]) ||//ingredient2 y mixIng1_3
                 (idObjAdded == basicIngredients[2] && idObjInventory == compoundIngredients[1].Id || idObjAdded == compoundIngredients[1].Id && idObjInventory == basicIngredients[2])) //ingredient3 y mixIng1_2
        {
            return compoundFinalObj;
        }
        return new ResultingCompoundIngredient(999, null);
    }
}