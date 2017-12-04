using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Clase que recoge los inputs del usuario
/// </summary>

public class InputManager : MonoBehaviour
{
    static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            return instance;
        }
    }

    public int State { get; set; }
    public bool LockInputsOutsidePopUps { get; set; }

    [SerializeField]
    private RectTransform inventoryTr;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        State = 0;
        LockInputsOutsidePopUps = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !MouseIsOverInventory(Input.mousePosition))
        {
            this.InputActionsManager(Camera.main.ScreenToWorldPoint(Input.mousePosition), false);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !MouseIsOverInventory(Input.touches[0].position))
        {
            this.InputActionsManager(Camera.main.ScreenToWorldPoint(Input.touches[0].position), false);
        }
    }

    /// <summary>
    /// Comprueba si se puede realizar alguna accion sobre el objeto que se ha tocado
    /// </summary>
    /// <param name="inputPosition"> posicion del input</param>
    /// <param name="isTryingToDraggObj"> si se esta intentando interactuar con un objeto arrastrado o haciendo click</param>

    public void InputActionsManager(Vector3 inputPosition, bool isTryingToDraggObj)//INTERFAZ
    {
        bool hasToRemoveObjDragged = false;

        if (!LockInputsOutsidePopUps && !FadeInOut.Instance.IsFading)
        {
            RaycastHit2D hit = Physics2D.Raycast(inputPosition, Vector3.forward);          

            if (hit)
            {
                //Caso generico
                if (hit.collider.GetComponent<InteractiveObject>())
                {
                    hasToRemoveObjDragged = InteractWithInteractiveObject(hit.collider.GetComponent<InteractiveObject>(), isTryingToDraggObj);
                }
                //Casos Especiales
                else if (hit.collider.GetComponent<PopUpOpnenerWithSwitch>())
                {
                    hasToRemoveObjDragged = hit.transform.GetComponent<PopUpOpnenerWithSwitch>().OpenPopUpWithSwitch();
                }
                else if (hit.collider.GetComponent<PopUpOpener>())//abridor de pop up
                {
                    hasToRemoveObjDragged = hit.transform.GetComponent<PopUpOpener>().OpenPopUp();
                }
                else if (hit.collider.transform.parent.GetComponent<DishWashing_WashingMachine>())
                {
                    hasToRemoveObjDragged = InteractWithWashingMachines(hit.collider, isTryingToDraggObj);
                }
                else if (hit.collider.transform.parent.transform.parent.GetComponent<DressKids>())
                {
                    hasToRemoveObjDragged = InteractWithDressKids(hit.collider, isTryingToDraggObj);
                }
                else if (!isTryingToDraggObj && hit.collider.transform.parent.tag == "ObjectsContainer")
                {
                    InteractWitContainer(hit.collider.transform.parent);
                }
                else
                {
                    ExecuteEvents.Execute(hit.collider.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
                }
            }
        }

        if (isTryingToDraggObj)
        {
            if (hasToRemoveObjDragged)
                Inventory.Instance.RemoveDraggedObjFromInv();
            else
                Inventory.Instance.RelocateDraggedObjInInv();
        }
    }


    private bool InteractWithInteractiveObject(InteractiveObject interactObj, bool isTryingToDraggObj)
    {
        if (interactObj.IsNecessaryDraggObjToInteract == isTryingToDraggObj)
        {
            return interactObj.InteractWith();
        }
        return false;
    }

    private bool InteractWithDressKids(Collider2D hitCol, bool isTryingToDraggObj)
    {
        DressKids dressKids = hitCol.transform.parent.transform.parent.GetComponent<DressKids>();

        if (dressKids.IsNecessaryDraggObjToInteract == isTryingToDraggObj)
        {
            if (hitCol.tag == "DressBoy")
            {
                return dressKids.DressThem(true);
            }
            else
            {
                return dressKids.DressThem(false);
            }
        }
        return false;
    }

    private bool InteractWithWashingMachines(Collider2D hitCol, bool isTryingToDraggObj)
    {
        DishWashing_WashingMachine WashingMach = hitCol.transform.parent.GetComponent<DishWashing_WashingMachine>();

        if (hitCol.tag == "OutsideWashingMachine" && !isTryingToDraggObj)
        {
            WashingMach.InteractWithOutsideDoor();
        }
        else if (hitCol.tag == "InsideWashingMachine" && (isTryingToDraggObj == WashingMach.RequireDraggInside()))
        {
            return WashingMach.InteractWithInsideDoor();
        }
        return false;
    }

    private void InteractWitContainer(Transform parentTr)
    {
        if (parentTr.GetComponentInChildren<ClosetContainer>())//Armario de ropa, hereda de container
        {
            parentTr.GetComponentInChildren<ClosetContainer>().OpenCloseCloset();
        }
        else if (parentTr.GetComponentInChildren<SinkContainer>())//Armario de fregadero, hereda de container
        {
            parentTr.GetComponentInChildren<SinkContainer>().OpenCloseSink();
        }
        else if (parentTr.GetComponentInChildren<ObjectsContainer>())//contenedor de objetos
        {
            parentTr.GetComponentInChildren<ObjectsContainer>().OpenCloseContainer();
        }
    }

    public bool MouseIsOverInventory(Vector2 InputPos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryTr, InputPos, null);
    }
}