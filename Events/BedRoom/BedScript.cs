using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cama, se puede hacer y recoger para convertirla en sofa. Si no esta cerrada no se puede abrir el armario.
/// </summary>

public class BedScript : InteractiveObject {

    public bool IsOpen { get; set; }

    [SerializeField]
    private SpriteRenderer bedOpen;

    [SerializeField]
    private SpriteRenderer bedOpenMessedUp;

    [SerializeField]
    private ClosetContainer closet;

    public void Awake()
    {
        IsOpen = true;
    }

    /// <summary>
    /// Abre,cierra y recoge la cama
    /// </summary>
    /// <returns>Si borra objeto arrastrado del inventario</returns>
    
    public override bool InteractWith () {

        if (!closet.IsOpen)
        {
            if (bedOpenMessedUp.enabled)
            {
                bedOpenMessedUp.enabled = false;
                bedOpen.enabled = true;
            }
            else if (bedOpen.enabled)
            {
                bedOpen.enabled = false;
                IsOpen = false;
            }
            else
            {
                bedOpen.enabled = true;
                IsOpen = true;
            }
        }
        else
        {
            SubtitlesManager.Instance.LoadSubtitles(45);
        }
        return false;
	}
}
