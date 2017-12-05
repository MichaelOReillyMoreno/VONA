using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Clase que representa un contenedor de algun tipo, con objetos dentro.
/// </summary>

public class ObjectsContainer : MonoBehaviour
{
    public bool IsOpen { get; set; }

    [Header("Identificador que controla si se puede usar"), SerializeField]
    private int id;

    [Header("Tr que contiene los objs e img abierto"), SerializeField]
    private Transform content;

    [Header("Colliders de contenedor"), SerializeField]
    private BoxCollider2D collider_Closed;

    [SerializeField]
    private BoxCollider2D collider_Open_1;

    [SerializeField]
    private BoxCollider2D collider_Ope_2;

    private SpriteRenderer openSprite;

    protected virtual void Awake()
    {
        IsOpen = false;
        openSprite = content.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Cambia el estado de un objeto contenedor
    /// </summary>

    public void OpenCloseContainer()
    {
        //si esta entre los objetos con los que puedes interactuar ese dia continua el proceso
        if (DaysManager.Instance.isObjectUsable(id))
        {
            IsOpen = !IsOpen;
            OpenCloseDoors();

            if (IsOpen)
                EnableContent();
            else
                DisableContent();

            ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            ExecuteEvents.Execute((IsOpen) ? this.collider_Closed.gameObject : this.collider_Open_1.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
        //si no esta entre los objetos con los que puede interactuar ese dia repite un mesaje que avisa al jugador que no debe hacer eso
        else if (DaysManager.Instance.Day == 0)
        {
            DaysManager.Instance.RepeatMessageOnWorngActions();
        }
    }

    private void EnableContent()
    {
        foreach (Transform objInside in content)
        {
            //si es un objeto interactuable, comprueba si esta desactivado para no habilitar su sprite
            if ((!objInside.GetComponent<CatchableObject>() && objInside.GetComponent<SpriteRenderer>()) || (objInside.GetComponent<CatchableObject>() && !objInside.GetComponent<CatchableObject>().IsDisable))
                objInside.GetComponent<SpriteRenderer>().enabled = true;

            if (objInside.GetComponent<Collider2D>())
                objInside.GetComponent<Collider2D>().enabled = true;
        }
    }

    private void DisableContent()
    {
        //al cerrarlo los desactiva
        foreach (Transform objInside in content)
        {
            if (objInside.GetComponent<SpriteRenderer>())
                objInside.GetComponent<SpriteRenderer>().enabled = false;

            if (objInside.GetComponent<Collider2D>())
                objInside.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OpenCloseDoors()
    {
        openSprite.enabled = (IsOpen) ? true : false;

        collider_Closed.enabled = (IsOpen) ? false : true;
        collider_Open_1.enabled = (IsOpen) ? true : false;

        //si tiene dos puertas cambia el estado de la otra
        if (collider_Ope_2)
        {
            collider_Ope_2.enabled = (IsOpen) ? true : false;
        }
    }
}