using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusseumObject : InteractiveObject {

    [SerializeField]
    private int id;

    [SerializeField]
    private GameObject descriptionPopUp;

    [SerializeField]
    private Text descriptionTxt;

    /// <summary>
    /// Abre el texto de un objeto del museo
    /// </summary>
    /// <returns>Si borra objeto arrastrado del inventario</returns>

    public override bool InteractWith()
    {
        InputManager.Instance.LockInputsOutsidePopUps = true;
        descriptionTxt.text = TextsManager.Instance.GetGameText(id);
        descriptionPopUp.SetActive(true);

        return false;
    }

    public int GetId() { return id;  }
}
