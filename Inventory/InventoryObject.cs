using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Clase que representa una casilla del inventario
/// </summary>

public class InventoryObject : MonoBehaviour
{
    public DropInventoryObj DropInvObj;
    public CompoundObjectsCreator CompoundObjsCreator;

    public Image ObjImg;
    public Image ContainerImg;

    public Color EnableColor;
    public Color DisableColor;

    public int IDObject { get; set; }

    void Start()
    {
        ContainerImg.color = DisableColor;
        IDObject = -1;
    }

    public void DragToWorld()
    {
        if (IDObject != -1 && !CompoundObjsCreator.IsCheckingCompositions)
        {
            ObjImg.enabled = false;
            ContainerImg.color = DisableColor;
            Inventory.Instance.IdDraggedObj = IDObject;
            DropInvObj.DraggToWorld(GetComponent<RectTransform>().position, ObjImg.sprite);
        }
    }

    /// <summary>
    /// Coloca un nuevo objeto en el inventario
    /// </summary>
    /// <param name="img">imagen del nuevo objeto</param>
    /// <param name="idImg">id del nuevo objeto</param>

    public void PutObj(Sprite img, int idImg)
    {
        ObjImg.sprite = img;
        ObjImg.enabled = true;
        IDObject = idImg;
        ContainerImg.color = EnableColor;
    }

    /// <summary>
    /// Quita el objeto de la casilla
    /// </summary>

    public void RemoveObjExtended()
    {
        IDObject = -1;
        ContainerImg.color = DisableColor;
        ObjImg.enabled = false;
    }

    public void RemoveObj()
    {
        IDObject = -1;
    }

    public void ReActiveObj()
    {
        ContainerImg.color = EnableColor;
        ObjImg.enabled = true;
    }
}