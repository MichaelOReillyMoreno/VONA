using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase hija de ZoomObject que abre un pop up, incluye un interruptor ademas (fuego o grifo)
/// </summary>

public class PopUpOpnenerWithSwitch : PopUpOpener
{
    [SerializeField]
    private SpriteRenderer switchSprt;

    private bool SwitchIsOn;

    private void Awake()
    {
        SwitchIsOn = false;
    }

    /// <summary>
    /// Antes de intentar hacer zoom comprueba si el switch esta abierto, es ese caso lo cierra
    /// </summary>
    /// <returns></returns>

    public bool OpenPopUpWithSwitch()
    {
        if (!PopUpIsActive && SwitchIsOn)
        {
            switchSprt.enabled = false;
            SwitchIsOn = false;

            return false;
        }
        else
        {
            return OpenPopUp();
        }
    }

    /// <summary>
    /// Cambia el estado del switch(fuego o grifo)
    /// </summary>
    /// <param name="isSwitchOpen"> se quiere abrir el switch? </param>

    public void ChangeStateSwitch(bool isSwitchOpen)
    {
        switchSprt.enabled = isSwitchOpen;
        SwitchIsOn = isSwitchOpen;
    }
}
