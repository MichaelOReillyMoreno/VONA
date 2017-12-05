using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public bool IsNecessaryDraggObjToInteract { get; set; }

    public abstract bool InteractWith();
}
