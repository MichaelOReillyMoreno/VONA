using UnityEngine;

/// <summary>
/// Clase que representa un objeto que se puede coger, dejar y usar.
/// </summary>

public class CatchableObject : InteractiveObject
{
    //si esta desactivado, es decir si el objeto ha sido cogido
    public bool IsDisable { get; set; }

    //ID del objeto
    [SerializeField]
    private int IDObject;

    //si es los platos
    [SerializeField]
    private bool isDishes;

    private SpriteRenderer img;

    void Start()
    {
        img = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Coge el objeto del escenario
    /// </summary>
    /// <returns>Si borra objeto arrastrado del inventario</returns>
    
    public override bool InteractWith()
    {
        //si no esta desactivado y puede cogerse ese dia
        if (!IsDisable)
        {
            if (DaysManager.Instance.isObjectUsable(IDObject))
            {

                bool canPutInInv = Inventory.Instance.SetObject(img.sprite, IDObject);

                if (canPutInInv)
                {
                    img.enabled = false;
                    IsDisable = true;

                    if (isDishes && DaysManager.Instance.Day == 0)
                    {
                        DaysManager.Instance.SetPhaseOfTheDay(DayPhase.phase4);
                    }
                }
            }
            else if (DaysManager.Instance.Day == 0)
            {
                DaysManager.Instance.RepeatMessageOnWorngActions();
            }
        }
        return false;
        // si este objeto esta activo en el inventario se vuelve a dejar en su stio
        //else if (IDObject == Inventory.Instance.IdActiveObj)
        //{
        //    Inventory.Instance.RemoveObject();
        //    img.enabled = true;
        //    IsDisable = false;
        //}
    }
}
