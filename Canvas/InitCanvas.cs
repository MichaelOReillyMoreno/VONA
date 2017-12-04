using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Clase que inicia las cartelas del principio del juego
/// </summary>
public class InitCanvas : MonoBehaviour
{
    //Cartelas
    [SerializeField]
    private List<Sprite> myCartelas;

    //Tiempo entre cartelas
    [SerializeField]
    private float timeBetweenCartelas;

    //Atributo propio
    [SerializeField]
    private SpriteRenderer myCartela;

    IEnumerator Start()
    {
        this.myCartela = this.GetComponent<SpriteRenderer>();
        foreach (Sprite im in this.myCartelas)
        {
            this.myCartela.sprite = im;
            yield return new WaitForSeconds(this.timeBetweenCartelas);
        }
    }
}