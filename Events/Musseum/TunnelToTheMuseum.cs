using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TunnelToTheMuseum : MonoBehaviour {

    [SerializeField]
    private Sprite[] tunelSprts;

    [SerializeField]
    private SpriteRenderer tunelSprtRnd;

    [SerializeField]
    private Collider2D tunelCol;

    [SerializeField]
    private Image arrowBackMusseum;

    private int tunelProgressionCont;

    public void TunelProgression()
    {
        if (tunelProgressionCont < tunelSprts.Length)
        {
            tunelSprtRnd.sprite = tunelSprts[tunelProgressionCont];
            tunelProgressionCont++;
        }
        else
        {
            tunelSprtRnd.enabled = false;
            tunelCol.enabled = false;
            arrowBackMusseum.enabled = true;
        }
    }

    public void ResetTunel()
    {
        tunelSprtRnd.enabled = true;
        tunelCol.enabled = true;
        tunelProgressionCont = 0;
    }
}
