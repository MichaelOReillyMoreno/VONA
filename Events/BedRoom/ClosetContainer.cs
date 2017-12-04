using UnityEngine;

/// <summary>
/// Clase que hereda de ObjectsContainer, no se abre y esta abierta la cama
/// </summary>
public class ClosetContainer : ObjectsContainer
{
    [SerializeField]
    private BedScript bed;

    public void OpenCloseCloset()
    {
        if (!bed.IsOpen)
        {
            OpenCloseContainer();
        }
        else
        {
            SubtitlesManager.Instance.LoadSubtitles(12);
        }
    }
}
