using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que representa las cortinas
/// </summary>

public class Courtains : InteractiveObject
{
    [SerializeField]
    private Sprite[] courtainsStates;

    [SerializeField]
    private SpriteRenderer courtains_SprtRend;

    private bool courtainsAreOpen;

    /// <summary>
    /// Abre o cierra cortinas
    /// </summary>
    /// <returns>Si borra objeto arrastrado del inventario</returns>

    public override bool InteractWith()
    {
        courtainsAreOpen = !courtainsAreOpen;
        courtains_SprtRend.sprite = (courtainsAreOpen) ? courtainsStates[1] : courtainsStates[0];

        return false;
    }
}
