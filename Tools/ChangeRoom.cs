using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Cambia de habitacion
/// </summary>

public class ChangeRoom : MonoBehaviour
{

    public enum Room { kitchen = 0, bathroom = 1, exterior = 2, dinningRoom = 3, workSpace = 4, hall = 5, bedRoom = 6, musseum = 7 };

    [SerializeField]
    private Transform [] rooms;

    [SerializeField]
    private Transform cameraTr;

    [SerializeField]
    private Image arrowBackDinningRoom;

    [SerializeField]
    private Image arrowBackMusseum;

    [SerializeField]
    private Image arrowRight;

    [SerializeField]
    private Image arrowLeft;

    private Vector3[] CameraPositions;

    private Room currentRoom;
    private Room currentRoomAux;

    private bool isMoving;

    private int day;

    void Start()
    {
        //empiezo en la cocina pero este valor solo se usa para las flechas laterales, por lo tanto empieza en el comedor
        currentRoom = currentRoomAux = Room.dinningRoom;
        day = DaysManager.Instance.Day;
        isMoving = false;

        CameraPositions = new Vector3[rooms.Length];

        for (int i = 0; i < rooms.Length; i++)
        { 
            CameraPositions[i] = new Vector3(rooms[i].position.x + 1.14f, cameraTr.position.y, cameraTr.position.z);
        }
    }

    public void ChangeLeft ()
    {
        currentRoomAux = GetRoomLeft();
        GoTo((int)currentRoomAux);
    }

    public void ChangeRight()
    {
        currentRoomAux = GetRoomRight();
        GoTo((int)currentRoomAux);
    }

    public void GoTo(int nRoom)
    {
        if (day != 0 || (day == 0 && DaysManager.Instance.isRoomEnabled(nRoom)))
        {
            if (!isMoving)
            {
                currentRoom = currentRoomAux;
                StartCoroutine(TransitionToAnOtherRoom(CameraPositions[nRoom], (Room)nRoom));

                if ((Room)nRoom == Room.dinningRoom && day == 0)
                    DaysManager.Instance.SetPhaseOfTheDay(DayPhase.phase5);
            }
        }
        else SetMessage_WrongAction();
    }

    public IEnumerator TransitionToAnOtherRoom(Vector3 roomPosition, Room room)
    {
        isMoving = true;
        yield return StartCoroutine(FadeInOut.Instance.CrossFadeAlpha(1f, 1f));
        cameraTr.position = roomPosition;

        //si se quiere ir a una habiatacion con flecha hacia atras:

        if ((int)room <= 2)
        {
            //si se quiere ir al baño o cocina, quita el comedor de encima:

            if ((int)room <= 1)
                MoveDinningRoomYAxis(30);

            ChangeArrows(true);
        }
        else if ((room == Room.dinningRoom || room == Room.hall) && !arrowLeft.enabled)
        {
            //si se entra a una habitacion con flechas laterales desde una con flecha hacia atras

            if (room == Room.dinningRoom)
            {
                //si se entra al comedor desde la cocina o el baño coloca el comedor encima para que se vea la cocina y baño desde las puertas

                MoveDinningRoomYAxis(-30);
            }

            ChangeArrows(false);
        }
        else if (room == Room.musseum)
        {
            //se entra en el museo:

            arrowBackDinningRoom.enabled = false;
        }

        yield return StartCoroutine(FadeInOut.Instance.CrossFadeAlpha(0f, 1f));
        isMoving = false;
    }

    private void SetMessage_WrongAction()
    {
        switch (DaysManager.Instance.PhaseOfTheDay)
        {
            case DayPhase.initPhase:
            case DayPhase.phase1:
            case DayPhase.phase2:
                SubtitlesManager.Instance.LoadSubtitles(25);
                break;

            case DayPhase.phase3:
                SubtitlesManager.Instance.LoadSubtitles(39);
                break;

            case DayPhase.phase5:
                SubtitlesManager.Instance.LoadSubtitles(33);
                break;

            default:
                SubtitlesManager.Instance.LoadSubtitles(40);
                break;
        }
    }

    private void MoveDinningRoomYAxis(int amount)
    {
        rooms[3].position = new Vector3(rooms[3].position.x, rooms[3].position.y + amount, rooms[3].position.z);
    }

    private void ChangeArrows(bool b)//true ->kitchen
    {
        arrowBackDinningRoom.enabled = b;
        arrowBackMusseum.enabled = false;
        arrowLeft.enabled = arrowRight.enabled = !b;
    }

    private Room GetRoomLeft()
    {
        return ((int)currentRoom < 6) ? ++currentRoom : Room.dinningRoom;
    }

    private Room GetRoomRight()
    {
        return ((int)currentRoom > 3) ? --currentRoom : Room.bedRoom;
    }
}
