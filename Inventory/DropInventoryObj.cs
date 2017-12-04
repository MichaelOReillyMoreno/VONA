using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DropInventoryObj : MonoBehaviour {

    public InputManager InputMan;

    public RectTransform RectTr;
    public Image BackGroundImg;
    public Image InvObjImg;
    public float speed;

    public bool IsBeinDragged { get; set; }

    private Vector3 inputPosition;
    private float step;


    void Awake ()
    {
        BackGroundImg.enabled = false;
        InvObjImg.enabled = false;
    }

    void Update()
    {
        if (IsBeinDragged)
        {
            step = speed * Time.deltaTime;
            inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RectTr.position = Vector3.MoveTowards(RectTr.position,
                                                  RectTransformUtility.WorldToScreenPoint(Camera.main, inputPosition),
                                                  step);
        }
    }

    public void DraggToWorld (Vector2 PositionObjInv, Sprite imgObjInv)
    {
        IsBeinDragged = true;
        RectTr.position = PositionObjInv;

        BackGroundImg.enabled = true;
        InvObjImg.sprite = imgObjInv;
        InvObjImg.enabled = true;
    }

    public void DropInWorld()
    {
        InputMan.InputActionsManager(inputPosition, true);
        StopDragg();
    }

    private void StopDragg()
    {
        IsBeinDragged = false;
        BackGroundImg.enabled = false;
        InvObjImg.enabled = false;
    }

}
