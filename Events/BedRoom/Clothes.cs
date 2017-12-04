using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Clase que representa una prenda del pop up
/// </summary>

public class Clothes : MonoBehaviour
{
    private enum typeClothes { shirt = 0, pants_Skirt = 1, shoes = 2, vest = 3 };

    [SerializeField]
    private int id;

    [SerializeField]
    private typeClothes typeOfClothes;

    [SerializeField]
    private RectTransform boyBodyPartToAttach;

    [SerializeField]
    private RectTransform girlBodyPartToAttach;

    [SerializeField]
    private DressPopUp dressZone;

    [SerializeField]
    private RectTransform areaPopUp;

    //poscion inicial de la prenda
    private Vector2 initialPos;

    //posicion local actual de la imagen
    private Vector2 currentLocalPos;
    private Vector2 inputPos;

    //limites del pop up por los que se puede mover
    private float[] limits;
    private RectTransform trClothe;

    //si esta siendo arrastrada
    private bool IsBeinDragged;

    private float speed;
    private float step;

    void Start()
    {
        speed = 700;
        trClothe = GetComponent<RectTransform>();
        initialPos = trClothe.localPosition;
        currentLocalPos = initialPos;
        IsBeinDragged = false;

        limits = new float[] { (areaPopUp.rect.width * 0.5f) - (trClothe.rect.width * 0.4f), -((areaPopUp.rect.width * 0.5f) - (trClothe.rect.width * 0.4f)),
                               (areaPopUp.rect.height * 0.5f) - (trClothe.rect.height * 0.4f), -((areaPopUp.rect.height * 0.5f) - (trClothe.rect.height * 0.4f)) };
    }

    void Update()
    {
        //si se hace click sobre ella la prenda se mueve con el
        if (IsBeinDragged)
        {
            inputPos = areaPopUp.InverseTransformPoint(inputPos = Input.mousePosition);

            if (((currentLocalPos.x < limits[0] && currentLocalPos.x > limits[1] && currentLocalPos.y < limits[2] && currentLocalPos.y > limits[3]) 
                || (inputPos.x < limits[0] && inputPos.x > limits[1] && inputPos.y < limits[2] && inputPos.y > limits[3])))
            {
                currentLocalPos = trClothe.localPosition;
                step = speed * Time.deltaTime;
                trClothe.localPosition = Vector3.MoveTowards(currentLocalPos, inputPos, step);
            }
        }
    }

    /// <summary>
    /// mouse dragg On
    /// </summary>
    
    public void DragClothes()
    {
        IsBeinDragged = true;
    }

    /// <summary>
    /// (Mouse dragg Off) Al deseleccionar una prenda, comprueba si esta sobre un area sobre la que pueda ser colocada y la coloca
    /// </summary>

    public void TryPositionClothesOnBody()
    {
        IsBeinDragged = false;

        if (typeOfClothes == typeClothes.shirt || typeOfClothes == typeClothes.vest)
        {
            TryToPositionOnChests();
        }
        else if (typeOfClothes == typeClothes.pants_Skirt)
        {
            TryToPositionOnLegs();
        }
        else if (typeOfClothes == typeClothes.shoes)
        {
            TryToPositionOnFeets();
        }
    }

    /// <summary>
    /// Resetea su posición a la inicial
    /// </summary>

    public void ResetPos()
    {
        GetComponent<RectTransform>().localPosition = initialPos;
        GetComponent<Image>().enabled = true;
    }

    /// <summary>
    /// Si se selecciona la prenda ya colocada en el cuerpo, se activa la prenda correspondiente con tags y se coloca en el ratón
    /// </summary>

    public void PlaceInInput()
    {
        IsBeinDragged = true;
        GetComponent<Image>().enabled = true;
    }

    /// <summary>
    ///Si el chaleco o camisa estas sobre las piernas de alguno de los niños los coloca
    /// </summary>

    private void TryToPositionOnChests()
    {
        if (IsOnBodyPartToAttach(boyBodyPartToAttach.position))
        {
            GetComponent<Image>().enabled = false;
            dressZone.PlaceClothesInBoy(id, (int)typeOfClothes);
        }
        else if (IsOnBodyPartToAttach(girlBodyPartToAttach.position))
        {
            GetComponent<Image>().enabled = false;
            dressZone.PlaceClothesInGirl(id, (int)typeOfClothes);
        }
    }

    /// <summary>
    ///Si los pantalones estan sobre las piernas de alguno de los niños los coloca
    /// </summary>

    private void TryToPositionOnLegs()
    {
        if (IsOnBodyPartToAttach(boyBodyPartToAttach.position))
        {
            GetComponent<Image>().enabled = false;
            dressZone.PlaceClothesInBoy(id, (int)typeOfClothes);
        }
        else if (IsOnBodyPartToAttach(girlBodyPartToAttach.position))
        {
            GetComponent<Image>().enabled = false;
            dressZone.PlaceClothesInGirl(id, (int)typeOfClothes);
        }
    }

    /// <summary>
    ///Si los zapatos estan sobre los pies de alguno de los niños los coloca
    /// </summary>

    private void TryToPositionOnFeets()
    {
        if (IsOnBodyPartToAttach(boyBodyPartToAttach.position))
        {
            GetComponent<Image>().enabled = false;
            dressZone.PlaceClothesInBoy(id, (int)typeOfClothes);
        }
        else if (IsOnBodyPartToAttach(girlBodyPartToAttach.position))
        {
            GetComponent<Image>().enabled = false;
            dressZone.PlaceClothesInGirl(id, (int)typeOfClothes);
        }
    }

    /// <summary>
    ///Comprueba si esta prenda esta sobre una parte del cuerpo sobre la que pueda ser colocada
    /// </summary>

    private bool IsOnBodyPartToAttach(Vector2 bodyPartPos)
    {
        if (Vector3.Distance(trClothe.position, bodyPartPos) < 30)
        {
            return true;
        }
        return false;
    }
}

