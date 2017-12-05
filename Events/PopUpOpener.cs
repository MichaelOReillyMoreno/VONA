using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Abre un pop up
/// </summary>

public class PopUpOpener : MonoBehaviour
{
    public bool PopUpIsActive { get; set; }
    public bool isNecessaryDraggObjToInteract { get; set; }

    [SerializeField]
    private int id;

    [SerializeField]
    private int phaseDayToChange;

    //panel del pop up que se desea abrir en el canvas
    [SerializeField]
    private GameObject popUpPanel;

    [SerializeField]
    private int[] idRequiredObj;

    private void Start()
    {
        isNecessaryDraggObjToInteract = (idRequiredObj.Length != 0) ? true : false;
    }

    public bool OpenPopUp()
    {
        int invObj = Inventory.Instance.IdDraggedObj;
        bool hasToRemoveObjDragged = false;

        //abre el pop up, si no se requiere ningun objeto o cualquiera de los objetos que se requieren estan o  el pop up ya esta activo
        if ((!isNecessaryDraggObjToInteract && DaysManager.Instance.isObjectUsable(id)) || Array.Exists(idRequiredObj, element => element == invObj) || PopUpIsActive)
        {
            if (DaysManager.Instance.Day == 0 && phaseDayToChange != 0)
                DaysManager.Instance.SetPhaseOfTheDay((DayPhase) phaseDayToChange);

            //si es el pop up no esta activo, es decir ya ha sido abierto pero no ha sido completada la tarea, se ejecuta el submitHandler
            if (!PopUpIsActive)
            {
                ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
                PopUpIsActive = true;

                if (isNecessaryDraggObjToInteract)
                {
                    hasToRemoveObjDragged = true;
                    isNecessaryDraggObjToInteract = false;
                }
            }

            popUpPanel.SetActive(true);
            InputManager.Instance.LockInputsOutsidePopUps = true;
        }
        else if(DaysManager.Instance.Day == 0)
        {
            DaysManager.Instance.RepeatMessageOnWorngActions();
        }

        return hasToRemoveObjDragged;
    }
}