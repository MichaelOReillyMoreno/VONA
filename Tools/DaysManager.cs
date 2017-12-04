using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controla todas las variables y comportamientos que varian con el paso de los dias
/// </summary>

public enum DayPhase { initPhase = 0, phase1 = 1, phase2 = 2, phase3 = 3, phase4 = 4, phase5 = 5, phase6 = 6, phase7 = 7, phase8 = 8, phase9 = 9, endPhase = 10 };

public class DaysManager : MonoBehaviour {


    static DaysManager instance;

    public static DaysManager Instance
    {
        get
        {
            return instance;
        }
    }

    //linea 22 inputmanager Los instacate a veces NULL
    //toybox sub 38? siempre ?

    //ES UNA TONTERIA MANTENER CURRENT LANGUAGE??

    //SPRITES BORSCH
    //VODKA IR A HABITACION RANDOM AL INTENTAR MOVERTE
    //las manchas de la sarten no se como colocarlas
    //la lavadora nueva cerrada tiene un botón diferente a la lavadora abierta
    //que no sea capaz de leer textos de museo en estado borracho
    //estblecer disintos finales de vestir niños
    //IDs 111 sin usar
    //subtitles   this.currentSubtitle = TextsManager.Instance.GameSubtitles.Single(x => x.Id == id); esto ineficiente

    [Header("Variable para forzar un dia (0 <= tutorial"), SerializeField]
    private int dayToSet = -1;//DEBUG ONLY

    public int Day { get; set;}
    public DayPhase PhaseOfTheDay { get; set; }

    private List<int> idUsableObjects;
    private List<int> idUsableArrows;

    //mensaje que se repite al realizar acciones incorrectas
    private int[] messageRepeatByActions;
    private int messageActionsIndex;

    void Awake ()
    {
        if(instance == null)
            instance = this;
        else
            Destroy (this);

        //DEBUG ONLY
        if (dayToSet < 0) PlayerPrefs.SetInt("Day", 0); else PlayerPrefs.SetInt("Day", dayToSet);
        //DEBUG ONLY

        Day = PlayerPrefs.GetInt("Day");
        print("day "+Day);
        //DEBUG ONLY
    }

    private void Start()
    {
        PhaseOfTheDay = 0;
        SetPhaseOfTheDay(0);
    }

    ///<summary>
    ///Establece la fase del dia, dependiendo de esta se pueden hacer unas cosas u otras
    /// <param name="phaseOfTheDay">fase del dia</param>
    ///</summary>

    public void SetPhaseOfTheDay(DayPhase nextPhaseOfTheDay)
    {
        if (nextPhaseOfTheDay >= this.PhaseOfTheDay)
        {
            PhaseOfTheDay = nextPhaseOfTheDay;      
            print("Fase del dia : " + PhaseOfTheDay);

            if (Day == 0)
            {
                ExecuteEventsTutorial();
            }
            else
            {
                ExecuteEventsNormalDay();
            }
        }
    }

    ///<summary>
    ///Lanza un mesaje al relizar una accion equivocada
    ///</summary>

    public void RepeatMessageOnWorngActions()
    {
        if (messageRepeatByActions.Length > 0 && messageActionsIndex < messageRepeatByActions.Length)
        {
            SubtitlesManager.Instance.LoadSubtitles(messageRepeatByActions[messageActionsIndex]);
            messageActionsIndex++;
        }
    }

    ///<summary>
    ///Controla si se puede se puede interactuar con un objeto ese dia
    /// <param name="id">Id del objeto o accion que se quiere comprobar</param>
    ///</summary>

    public bool isObjectUsable(int id)
    {
        bool canUseIt = false;

        if (Day == 0)
        {
            canUseIt = idUsableObjects.Exists(element => element == id);
        }
        else
        {
            canUseIt = true;
        }
        return canUseIt;
    }

    ///<summary>
    ///Controla a que habitaciones se puede ir ese dia
    ///</summary>
    /// <param name="room">numero de la habitacion que se quiere comprobar</param> 

    public bool isRoomEnabled(int room)
    {
        bool canDoIt = false;

        if (Day == 0)
        {
            canDoIt = idUsableArrows.Exists(element => element == room);

        }
        else
        {
            canDoIt = true;
        }
        return canDoIt;
    }
    public void ExecuteEventsNormalDay()
    {
        switch (PhaseOfTheDay)
        {
            case 0://al inicio
                StartCoroutine(FadeInOut.Instance.FadeInOutCo(0, 0.5f));
                SubtitlesManager.Instance.LoadSubtitles(0);
                break;
        }
    }
        public void ExecuteEventsTutorial()
    {
        switch (PhaseOfTheDay)
        {
            case DayPhase.initPhase:

                idUsableObjects = new List<int> { 106, 109 };
                idUsableArrows = new List<int> { 0 };

                messageRepeatByActions = new int[] { 18, 19, 20 };
                messageActionsIndex = 0;

                StartCoroutine(SubtitlesManager.Instance.ShowSubsInCartel(24, false));

                break;

            case DayPhase.phase1:// al ver la lista
                idUsableObjects.Add(110);

                messageRepeatByActions = new int[] { 28, 29, 30 };
                messageActionsIndex = 0;

                break;

            case DayPhase.phase2:// al ver las recetas

                idUsableObjects = new List<int> { 42, 106, 101, 104 };
                messageRepeatByActions = new int[] { 13 };
                messageActionsIndex = 0;

                break;

            case DayPhase.phase3: // al terminar los huevos

                idUsableObjects.Add(33);

                break;

            case DayPhase.phase4: // al coger los platos

                idUsableArrows.Add(3);

                break;

            case DayPhase.phase5: // al salir de la cocina

                idUsableArrows = new List<int>();
                messageRepeatByActions = new int[] { 35, 36, 37 };
                messageActionsIndex = 0;

                break;

            case DayPhase.phase6: // al servir la mesa
                messageRepeatByActions = new int[] { 10 };
                messageActionsIndex = 0;
                idUsableArrows.Add(6);

                break;
        }
    }
}
