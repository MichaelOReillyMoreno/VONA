using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objeto que se puede mover(cambiar entre dos sprites) y/o lanza un subtitulo
/// </summary>

public class IrrelevantObject : InteractiveObject
{
    [SerializeField]
    private int idSubtitle;

    [SerializeField]
    private bool canBeMoved;

    [SerializeField]
    private Sprite rightPosition;

    [SerializeField]
    private Sprite wrongPosition;

    private SpriteRenderer sprtRend;
    private bool isInWrongPos;

    private void Start()
    {
        isInWrongPos = false;
        sprtRend = GetComponent<SpriteRenderer>();
    }

    public override bool InteractWith ()
    {
        if(idSubtitle != 0)
            SubtitlesManager.Instance.LoadSubtitles(idSubtitle);

        if (canBeMoved)
        {
            sprtRend.sprite = (isInWrongPos) ? rightPosition : wrongPosition;
            isInWrongPos = !isInWrongPos;
        }

        return false;
    }

    public void ForceWrongPos()
    {
        sprtRend.sprite = wrongPosition;
        isInWrongPos = true;
    }

}
