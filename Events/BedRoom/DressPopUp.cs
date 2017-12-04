using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gestiona el pop up de vestir a los niños
/// </summary>

public class DressPopUp : MonoBehaviour
{
    //arrays de ropas con tags que se mueven con el raton y sin tags que se colocan sobre el cuerpo
    [SerializeField]
    private RectTransform[] clothes;

    [SerializeField]
    private Clothes[] clothesTags;

    //ropa de los niños que se reflejan en el espejo
    [SerializeField]
    private SpriteRenderer[] clothesInBoyMirror;

    [SerializeField]
    private SpriteRenderer[] clothesInGirlMirror;

    [SerializeField]
    private Sprite[] clothesMirror;

    [SerializeField]
    private DressKids dressKinds;

    //ropas colocadas sobre los niños, 99 = vacio
    private int[] clothesInBoy;
    private int[] clothesInGirl;

    //posicion en que se coloca cada prenda sobre cada niño en el pop up
    private Vector2[] posClothesBoy;
    private Vector2[] posClotheGirl;

    //posicion en que se coloca cada prenda sobre cada niño en el espelo
    private Vector2[] posClothesBoyMirror;
    private Vector2[] posClotheGirlMirror;

    private Clothes clothePick;

    void Awake()
    {
        clothesInBoy = new int[] { 99, 99, 99, 99 };
        clothesInGirl = new int[] { 99, 99, 99, 99 };

        posClothesBoy = new[] { new Vector2(-118.8f, 20.8f), new Vector2(-118.8f, 32.1f), new Vector2(-119.3f, 17.1f),
                                new Vector2(-120.2f, 30.6f), new Vector2(-114f, -60.7f), new Vector2(-118.7f, -85.35f),
                                new Vector2(-113.9f, -191f), new Vector2(-113.9f, -191f) };

        posClotheGirl = new[] { new Vector2(116.9f, 20.8f), new Vector2(116.9f, 32.1f), new Vector2(117.3f, 15.3f),
                                new Vector2(116.25f, 30.6f), new Vector2(120.8f, -48f), new Vector2(120.2f, -73.4f),
                                new Vector2(122.2f, -191f), new Vector2(122.2f, -191f) };

        posClothesBoyMirror = new[] { new Vector2(3.251f, 0.343f), new Vector2(3.271f, 0.412f), new Vector2(3.26f, 0.34f),
                                      new Vector2(3.271f, 0.47f), new Vector2(3.24f, -0.854f), new Vector2(3.297f, -1.229f) };

        posClotheGirlMirror = new[] { new Vector2(-3.339f, 0.24f), new Vector2(-3.29f, 0.27f), new Vector2(-3.31f, 0.19f),
                                      new Vector2(-3.29f, 0.32f), new Vector2(-3.34f, -0.996f), new Vector2(-3.278f, -1.401f) };
    }

    /// <summary>
    /// Si el niño tiene una prenda sobre una parte del cuerpo sobre la que se hace click se quita y coloca en la posicion del raton
    /// </summary>
    /// <param name="nBodyPart"> tipo de parte de cuerpo. pecho = 0, piernas = 1, pies = 2, chaleco = 3</param>

    public void PickAClothOfTheBoy(int nBodyPart)
    {
        //si se hace click sobre el pecho pero hay un chaleco quita el chaleco primero
        if (nBodyPart == 0 && clothesInBoy[3] != 99)
        {
            nBodyPart = 3;
        }

        if (clothesInBoy[nBodyPart] != 99)
        {
            clothesTags[clothesInBoy[nBodyPart]].PlaceInInput();
            clothes[clothesInBoy[nBodyPart]].GetComponent<Image>().enabled = false;
            clothePick = clothesTags[clothesInBoy[nBodyPart]];
            clothesInBoy[nBodyPart] = 99;
        }
    }

    /// <summary>
    /// Si la niña tiene una prenda sobre una parte del cuerpo sobre la que se hace click se quita y coloca en la posicion del raton
    /// </summary>
    /// <param name="nBodyPart"> tipo de parte de cuerpo. pecho = 0, piernas = 1, pies = 2, chaleco = 3</param>

    public void PickAClothOfTheGirl(int nBodyPart)
    {
        //si se hace click sobre el pecho pero hay un chaleco quita el chaleco primero
        if (nBodyPart == 0 && clothesInGirl[3] != 99)
        {
            nBodyPart = 3;
        }

        if (clothesInGirl[nBodyPart] != 99)
        {
            clothesTags[clothesInGirl[nBodyPart]].PlaceInInput();
            clothes[clothesInGirl[nBodyPart]].GetComponent<Image>().enabled = false;
            clothePick = clothesTags[clothesInGirl[nBodyPart]];
            clothesInGirl[nBodyPart] = 99;
        }
    }

    /// <summary>
    /// onMouseUp se deselecciona la prenda que se estaba arrastrando
    /// </summary>

    public void DropKidsClothePicked()
    {
        if (clothePick)
        {
            clothePick.TryPositionClothesOnBody();
            clothePick = null;
        }
    }

    /// <summary>
    /// Coloca una prenda sobre el cuerpo del niño/a
    /// </summary>
    /// <param name="id">id de prenda</param>
    /// <param name="bodyPart">parte del cuerpo a la que pertenece</param>

    public void PlaceClothesInBoy(int id, int bodyPart)
    {
        //si la ropa no es un chaleco(1,3) o la ropa es un chaleco y ya tiene una camisa puesta
        if ((id != 1 && id != 3) || ((id == 1 || id == 3) && (clothesInBoy[0] == 2 || clothesInBoy[0] == 0)))
        {
            if (clothesInBoy[bodyPart] != 99)//si hay alguna ropa puesta en el mismo lugar la envio a su posicion inicial
            {
                clothes[clothesInBoy[bodyPart]].GetComponent<Image>().enabled = false;
                clothesTags[clothesInBoy[bodyPart]].ResetPos();

                if (bodyPart == 0 && clothesInBoy[3] != 99)//si se coloca una camiseta y ya hay un chaleco encima, se quita tambien el chaleco
                {
                    clothes[clothesInBoy[3]].GetComponent<Image>().enabled = false;
                    clothesTags[clothesInBoy[3]].ResetPos();
                }
            }

            //se coloca en su posicion
            clothesInBoy[bodyPart] = id;
            clothes[id].localPosition = posClothesBoy[id];
            clothes[id].GetComponent<Image>().enabled = true;
        }
        else
        {
            clothesTags[id].ResetPos();
        }
    }

    /// <summary>
    /// Coloca una prenda sobre el cuerpo del niña
    /// </summary>
    /// <param name="id">id de prenda</param>
    /// <param name="bodyPart">parte del cuerpo a la que pertenece</param>

    public void PlaceClothesInGirl(int id, int bodyPart)
    {
        //si la ropa no es un chaleco(1,3) o la ropa es un chaleco y ya tiene una camisa puesta
        if ((id != 1 && id != 3) || ((id == 1 || id == 3) && (clothesInGirl[0] == 2 || clothesInGirl[0] == 0)))
        {
            if (clothesInGirl[bodyPart] != 99)
            {
                clothes[clothesInGirl[bodyPart]].GetComponent<Image>().enabled = false;
                clothesTags[clothesInGirl[bodyPart]].ResetPos();

                if (bodyPart == 0 && clothesInGirl[3] != 99)
                {
                    clothes[clothesInGirl[3]].GetComponent<Image>().enabled = false;
                    clothesTags[clothesInGirl[3]].ResetPos();
                }
            }

            clothesInGirl[bodyPart] = id;
            clothes[id].localPosition = posClotheGirl[id];
            clothes[id].GetComponent<Image>().enabled = true;
        }
        else
        {
            clothesTags[id].ResetPos();
        }
    }

    /// <summary>
    /// Comprueba si los niños han sido vestidos de acuerdo a su genero al cerrar el pop up y refleja en los espejos como han sido vestidos en el pop up
    /// </summary>

    public void ShowDressingResult()
    {
        bool areDessed = true;

        for (int i = 0; i < 3; i++)
        {
            if (clothesInGirl[i] == 99 || clothesInBoy[i] == 99)
            {
                areDessed = false;
            }
        }
        //ESTABLECER DISTINTOS FINALES!!!!!!
        //Si estan completamente vestidos
        if (areDessed)
            SubtitlesManager.Instance.LoadSubtitles(22);

        ReflectClothesMirrorBoy(false);
        ReflectClothesMirrorGirl(false);

        //llamo a la clase que se encarga de elegir el modo de vestir a los niños para hacer desaparecer las prendas y niños del espejo
        StartCoroutine(dressKinds.FinishDressingChildren());
    }

    /// <summary>
    /// Refleja las ropas de los niños en lso espejos laterales
    /// </summary>
    /// <param name="putGenderClothes">si se quiere colocar los uniformes propios de su genero automaticamente</param>

    public void ReflectClothesMirrorBoy(bool putGenderClothes)
    {
        if (putGenderClothes)
        {
            clothesInBoy = new int[] { 0, 5, 7, 1 };
        }
        //ropa espejo chico
        for (int i = 0; i <= 2; i++)
        {
            //se salta los zapatos ya que no son reflejado y coloca el chaleco si hay
            if (i == 2 && clothesInBoy[i + 1] != 99)
            {
                clothesInBoyMirror[i].sprite = clothesMirror[clothesInBoy[i + 1]];
                clothesInBoyMirror[i].gameObject.GetComponent<Transform>().localPosition = posClothesBoyMirror[clothesInBoy[i + 1]];
            }
            else if (i != 2 && clothesInBoy[i] != 99)
            {
                clothesInBoyMirror[i].sprite = clothesMirror[clothesInBoy[i]];
                clothesInBoyMirror[i].gameObject.GetComponent<Transform>().localPosition = posClothesBoyMirror[clothesInBoy[i]];
            }
            clothesInBoyMirror[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Refleja las ropas de los niños en lso espejos laterales
    /// </summary>
    /// <param name="putGenderClothes">si se quiere colocar los uniformes propios de su genero automaticamente</param>

    public void ReflectClothesMirrorGirl(bool putGenderClothes)
    {
        if (putGenderClothes)
        {
            clothesInGirl = new int[] { 2, 4, 6, 3 };
        }
        //ropa espejo chica
        for (int i = 0; i <= 2; i++)
        {
            if (i == 2 && clothesInGirl[i + 1] != 99)
            {
                clothesInGirlMirror[i].sprite = clothesMirror[clothesInGirl[i + 1]];
                clothesInGirlMirror[i].gameObject.GetComponent<Transform>().localPosition = posClotheGirlMirror[clothesInGirl[i + 1]];
            }
            else if (i != 2 && clothesInGirl[i] != 99)
            {
                clothesInGirlMirror[i].sprite = clothesMirror[clothesInGirl[i]];
                clothesInGirlMirror[i].gameObject.GetComponent<Transform>().localPosition = posClotheGirlMirror[clothesInGirl[i]];
            }
            clothesInGirlMirror[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Hace desaparecer todas las prendas del espejo
    /// </summary>

    public void DisableReflectClothes()
    {
        for (int i = 0; i <= 2; i++)
        {
            clothesInBoyMirror[i].gameObject.SetActive(false);
            clothesInGirlMirror[i].gameObject.SetActive(false);
        }
    }
}

