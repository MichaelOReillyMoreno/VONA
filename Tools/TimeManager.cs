using UnityEngine;

public class TimeManager : MonoBehaviour
{
    static TimeManager instance;

    public static TimeManager Instance
    {
        get
        {
            return instance;
        }
    }

    [Header("Duracion en seg de una unidad del tiempo"), SerializeField]
    private int timeUnitDuration;

    [Header("Duracion en unidades de tiempo de un dia"), SerializeField]
    private int dayDuration;

    [Header("Cuanto tarda en hacerse un huevo?"), SerializeField]
    private int eggtDuration;

    [Header("Cuanto tarda en hacerse un Borsch?"), SerializeField]
    private int borschtDuration;

    [Header("Cuanto tarda en hacerse un Barensky?"), SerializeField]
    private int barenskyDuration;

    [Header("Cuanto tarda en hacerse una tarta?"), SerializeField]
    private int cakeDuration;

    [Header("Cuanto tarda en lavar los platos?"), SerializeField]
    private int dishesDuration;

    [Header("Cuanto tarda en lavar la ropa?"), SerializeField]
    private int clothesDuration;

    [Header("Cuanto tarda en secarse la ropa?"), SerializeField]
    private int dryingClothesDuration;

    //objetos que funcionan con un temporizador
    [SerializeField]
    private DishWashing_WashingMachine dishWashingMachine;

    [SerializeField]
    private DishWashing_WashingMachine washingMachine;

    [SerializeField]
    private Clothesline clothesline;

    [SerializeField]
    private StolvePopUp stolve;

    [SerializeField]
    private StolvePopUp oven;

    //Clocks vars
    [SerializeField]
    private Transform[] clockHands;

    private Vector3 localRotationClock;
    private int nClocks;

    private bool isCookingEggs, isCookingBorsch, isCookingBarensky, isCookingCake, isWorkingWashingMachine, isWorkingDishesWashingMachine, clothesAreDrying;
    private int endCookingEggs, endCookingBorsch, endCookingBarensky, endCookingCake, endhWashingMachine, endDishesWashingMachine, endDryingClothes;

    //hora actual en unidades de tiempo del juego
    private int currentTimeInUnits;
    //segundos en los que cambiara de hora
    private int secsToChangeTUnits;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        currentTimeInUnits = 0;
        secsToChangeTUnits = timeUnitDuration;
        nClocks = clockHands.Length;
    }

    void Update()
    {
        if (secsToChangeTUnits < Time.time)
        {
            secsToChangeTUnits = (int)Time.time + timeUnitDuration;
            currentTimeInUnits++;
            CheckTime();

            if (currentTimeInUnits % 4 == 0)
            {
                for (int i = 0; i < nClocks; i++)
                {
                    MoveClockHand(clockHands[i]);
                }
            }
            print("Hora "+ currentTimeInUnits);
        }
    }

    /// <summary>
    /// Comprueba si en esta hora ha terminado algun proceso (o el dia)
    /// </summary>

    private void CheckTime()
    {
        //if (day != 0)
       //Si no estamos en el tutorial comprobar si hay algun subtitulo que deba ser lanzado

        //Comprueba si hay algo cocinandose en el fuego y si ha terminado
        if (isCookingBorsch && (endCookingBorsch <= currentTimeInUnits))
        {
            stolve.CookingTimeFinish();
            isCookingBorsch = false;
        }
        else if (isCookingBarensky && (endCookingBarensky <= currentTimeInUnits))
        {
            stolve.CookingTimeFinish();
            isCookingBarensky = false;
        }
        else if (isCookingEggs && (endCookingEggs <= currentTimeInUnits))
        {
            stolve.CookingTimeFinish();
            isCookingEggs = false;
        }

        //Comprueba si hay una tarta cocinandose y si ha terminado
        if (isCookingCake && (endCookingCake <= currentTimeInUnits))
        {
            oven.CookingTimeFinish();
            isCookingCake = false;
        }

        //Comprueba si se estan lavando los paltos y si han terminado
        if (isWorkingDishesWashingMachine && (endDishesWashingMachine <= currentTimeInUnits))
        {
            dishWashingMachine.WorkDone();
            isWorkingDishesWashingMachine = false;
        }

        //Comprueba si se esta lavando la ropa y si ha terminado
        if (isWorkingWashingMachine && (endhWashingMachine <= currentTimeInUnits))
        {
            washingMachine.WorkDone();
            isWorkingWashingMachine = false;
        }

        //Comprueba si se esta secando la ropa y si ha terminado
        if (clothesAreDrying && (endDryingClothes <= currentTimeInUnits))
        {
            clothesline.ClothesAreDryNow();
            clothesAreDrying = false;
        }
        //Comprueba si el dia ha terminado
        if (currentTimeInUnits >= dayDuration)
        {
            StartCoroutine(FadeInOut.Instance.ResetDay());
        }
    }

    /// <summary>
    /// Establece cuando terminara de cocinar los huevos, retorna cada cuanto tiene que actualizar el srpite que refleja el estado de coccion
    /// </summary>

    public float CountdownEggs()
    {
        isCookingEggs = true;
        endCookingEggs = eggtDuration + currentTimeInUnits;

        return (((eggtDuration * timeUnitDuration) - (secsToChangeTUnits - Time.time)) / 3) * 0.9f;
    }

    /// <summary>
    /// Establece cuando terminara de cocinar el Borsch
    /// </summary>

    public void CountdownBorsch ()
    {
        isCookingBorsch = true;
        endCookingBorsch = borschtDuration + currentTimeInUnits;
    }

    /// <summary>
    /// Establece cuando terminara de cocinar el Borsch, retorna cada cuanto tiene que actualizar el srpite que refleja el estado de coccion
    /// </summary>

    public float CountdownBarensky()
    {
        isCookingBarensky = true;
        endCookingBarensky = barenskyDuration + currentTimeInUnits;

        return ((barenskyDuration * timeUnitDuration) - (secsToChangeTUnits - Time.time)) / 3;
    }

    /// <summary>
    /// Establece cuando terminara de cocinar el horno, retorna cada cuanto tiene que actualizar el srpite que refleja el estado de coccion
    /// </summary>

    public float CountdownCake()
    {
        isCookingCake = true;
        endCookingCake = cakeDuration + currentTimeInUnits;

        return ((cakeDuration * timeUnitDuration) - (secsToChangeTUnits - Time.time)) / 3;
    }

    /// <summary>
    /// Establece cuando terminara de lavar los platos
    /// </summary>

    public void CountdownDishes()
    {
        isWorkingDishesWashingMachine = true;
        endDishesWashingMachine = dishesDuration + currentTimeInUnits;
    }

    /// <summary>
    /// Establece cuando terminara de lavar la ropa
    /// </summary>

    public void CountdownClothes()
    {
        isWorkingWashingMachine = true;
        endhWashingMachine = dishesDuration + currentTimeInUnits;
    }

    /// <summary>
    /// Establece cuando terminara de secarse la ropa
    /// </summary>

    public void CountdownDryingClothes()
    {
        clothesAreDrying = true;
        endDryingClothes = dryingClothesDuration + currentTimeInUnits;
    }

    private void MoveClockHand(Transform hand)
    {
        localRotationClock = hand.localRotation.eulerAngles;
        localRotationClock = new Vector3(localRotationClock.x, localRotationClock.y, localRotationClock.z - 30f);
        hand.localRotation = Quaternion.Euler(localRotationClock);
    }
}