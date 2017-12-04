using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Clase que getiona la forma basica de vestir a los niños y lanza la forma compleja (pop up) si se ha desbloqueado
/// </summary>

public class DressKids : MonoBehaviour
{
    //Si se ha desbloqueado el acceso al pop up
    public bool PopUpIsUnlock { get; set; }

    //Si se puede vestir a los niños en este momento
    public bool TimeToDressKids { get; set; }

    public bool IsNecessaryDraggObjToInteract { get; set; }

    [SerializeField]
    private DressPopUp dresZone;

    //sprites que es necesario activar y desactivar al vestir y terminar de vestir a los niños
    [SerializeField]
    private GameObject[] kidsMirrorAndClothes;

    //Si la ropa de los niños esta preparada en el armario
    private bool clothesAreInside;

    //si cada uno de los niños esta vestido
    private bool boyIsDress;
    private bool GirlIsDress;

    void Start ()
    {
        IsNecessaryDraggObjToInteract = true;
        //PROVISIONAL
        if (DaysManager.Instance.Day == 0)
        {
            EnableDress();
            PutClothesInCloset();
        }
    }

    /// <summary>
    /// Clase que lanza el pop up si esta desbloqueado, si no gestina la forma simple de vestirlos
    /// </summary>
    /// <param name="isBoy"> Si se ha hehco click sobre la ropa de niña o niño</param>

    public bool DressThem(bool isBoy)
    {
        if (TimeToDressKids && clothesAreInside)
        {
            if (PopUpIsUnlock)
            {
                InputManager.Instance.LockInputsOutsidePopUps = true;
                GetComponent<PopUpOpener>().OpenPopUp();
            }
            else
            {
                //si esta funcion es llamada desde el evento del collider de la ropa de chico y no tiene la ropa puesta lo viste
                if (isBoy && !boyIsDress)
                {
                    DressBoy();
                }
                //si esta funcion es llamada desde el vento del collider de la ropa de chica y no tiene la ropa puesta la viste
                else if (!isBoy && !GirlIsDress)
                {
                    DressGirl();
                }

                //Si los dos estan vestidos
                if (boyIsDress && GirlIsDress)
                {
                    StartCoroutine(FinishDressingChildren());
                }
            }
        }
        else if (!clothesAreInside && Inventory.Instance.IdDraggedObj == 100)
        {
            PutClothesInCloset();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Activa todo lo necesario para poder vestir a los niños
    /// </summary>

    public void EnableDress()
    {
        TimeToDressKids = true;

        kidsMirrorAndClothes[2].SetActive(true);
        kidsMirrorAndClothes[3].SetActive(true);

        //Si el armario esta abierto tiene que activar sus sprites tambien ya que no han sido activados por el, al estar deshabilitados
        if (GetComponent<ClosetContainer>().IsOpen)
        {
            kidsMirrorAndClothes[2].GetComponent<SpriteRenderer>().enabled = true;
            kidsMirrorAndClothes[3].GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    /// <summary>
    /// Al terminar de vestir a los niños hace desaparecer sus ropas y reflejo
    /// </summary>
    /// <returns></returns>

    public IEnumerator FinishDressingChildren()
    {
        TimeToDressKids = false;
        clothesAreInside = false;
        IsNecessaryDraggObjToInteract = true;

        if (DaysManager.Instance.Day != 0)
        {
            SubtitlesManager.Instance.LoadSubtitles(22);
            yield return new WaitForSeconds(3f);

            StartCoroutine(FadeInOut.Instance.FadeInOutCo(1, 2));
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < kidsMirrorAndClothes.Length; i++)
            {
                kidsMirrorAndClothes[i].SetActive(false);
            }

            dresZone.DisableReflectClothes();
        }
        else
        {
            StartCoroutine(EndTuturialDay());
        }

    }

    /// <summary>
    /// Coloca ropa en armario
    /// </summary>

    private void PutClothesInCloset()
    {
        kidsMirrorAndClothes[0].SetActive(true);
        kidsMirrorAndClothes[1].SetActive(true);

        //Si el armario esta abierto tiene que activar sus sprites y colliders tambien ya que no han sido activados por el, al estar deshabilitados
        if (GetComponent<ClosetContainer>().IsOpen)
        {
            kidsMirrorAndClothes[0].GetComponent<SpriteRenderer>().enabled = true;
            kidsMirrorAndClothes[1].GetComponent<SpriteRenderer>().enabled = true;
        }

        Inventory.Instance.RemoveDraggedObjFromInv();

        IsNecessaryDraggObjToInteract = false;
        clothesAreInside = true;
    }

    private void DressBoy()
    {
        dresZone.ReflectClothesMirrorBoy(true);
        kidsMirrorAndClothes[0].SetActive(false);
        boyIsDress = true;
    }

    private void DressGirl()
    {
        dresZone.ReflectClothesMirrorGirl(true);
        kidsMirrorAndClothes[1].SetActive(false);
        GirlIsDress = true;
    }

    /// <summary>
    /// Acaba el tutorial, termina el dia
    /// </summary>
    /// <returns></returns>

    private IEnumerator EndTuturialDay()
    {
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(SubtitlesManager.Instance.ShowSubsInCartel(27, true));
        //StartCoroutine(FadeInOut.Instance.ResetDay());
        SceneManager.LoadScene("Main");

    }

}
