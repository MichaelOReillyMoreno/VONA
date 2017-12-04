using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

/// <summary>
/// Clase encargada de lanzar subtitulos en distintos formatos
/// </summary>

public class SubtitlesManager : MonoBehaviour
{
    static SubtitlesManager instance;

    public static SubtitlesManager Instance
    {
        get
        {
            return instance;
        }
    }

    public bool SkipSubtitles { get; set; }

    //activa los subtitulos de borracho
    public bool AreYouDrunk { get; set; }

    [SerializeField]
    private Text subtitleText;

    [SerializeField]
    private CanvasGroup subtitlesGroup;

    [SerializeField]
    private Text cartelText;

    //Flecha hacia atras que tiene que desaparecer al lanzar subtitulos si esta activa
    [SerializeField]
    private Button arrowBack;

    private IEnumerator currentCorutine;
    private Subtitles currentSubtitle;
    private Subtitles currentCartel;

    private float subDuration;
    private float timeWaitedSub;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    /// <summary>
    /// Carga unos subtitulos por su ID, si hay unos subtitulos activos, los detiene
    /// </summary>
    /// <param name="id">identificador del subtitulo</param>

    public void LoadSubtitles(int id)
    {
        if (AreYouDrunk)
            this.currentSubtitle = TextsManager.Instance.MixLanguagesInSubtitles(id);
        else
            this.currentSubtitle = TextsManager.Instance.GameSubtitles.Single(x => x.Id == id);

        //Detengo la corutina si esta activa
        if (this.currentCorutine != null)
            StopCoroutine(this.currentCorutine);

        this.currentCorutine = (id == 0) ? this.ShowSubtitles(this.currentSubtitle, true) : this.ShowSubtitles(this.currentSubtitle, false);

        StartCoroutine(this.currentCorutine);
    }

    /// <summary>
    /// Muestra los subtitulos por pantalla en la parte de abajo
    /// </summary>
    /// <param name="subs">subtitulos que se quieren mostrar</param>
    /// <param name="isDaySub"> si es el subtitulo que muestra el dia le añadade su numero</param>
    /// <returns></returns>

    public IEnumerator ShowSubtitles(Subtitles subs, bool isDaySub)
    {
        int cont = 0;

        if (arrowBack.enabled)
            arrowBack.interactable = false;

        yield return StartCoroutine(CorgiTools.FadeCanvasGroup(subtitlesGroup, 0.5f, 1f));

        while (cont < subs.SubList.Count)
        {
            this.subtitleText.text = subs.SubList[cont].Txt;
            if (isDaySub)
            {
                this.subtitleText.text += (DaysManager.Instance.Day + 1);
            }

            yield return new WaitForSeconds(subs.SubList[cont].Duration);

            cont++;
        }

        this.subtitleText.text = "";

        yield return StartCoroutine(CorgiTools.FadeCanvasGroup(subtitlesGroup, 0.5f, 0f));

        if (arrowBack.enabled)
            arrowBack.interactable = true;

        this.currentCorutine = null;
    }

    /// <summary>
    /// Muestra los subtitulos por pantalla en la parte central de la pantalla
    /// </summary>
    /// <param name="id">identificador de los subtitulos que se quieren mostrar</param>
    /// <returns></returns>

    public IEnumerator ShowSubsInCartel(int id, bool endDay)
    {
        this.currentCartel = TextsManager.Instance.GameSubtitles.Single(x => x.Id == id);
        int cont = 0;

        yield return StartCoroutine(FadeInOut.Instance.CrossFadeAlpha(1, 0.5f));
        yield return StartCoroutine(CorgiTools.FadeText(cartelText, 0.5f, Color.white));

        while (cont < currentCartel.SubList.Count)
        {
            subDuration = currentCartel.SubList[cont].Duration;
            SkipSubtitles = false;
            timeWaitedSub = 0;

            this.cartelText.text = currentCartel.SubList[cont].Txt;

            while (timeWaitedSub < subDuration && SkipSubtitles == false)
            {
                timeWaitedSub += 0.3f;
                yield return new WaitForSeconds(0.3f);
            }
            cont++;
        }

        this.cartelText.text = "";

        if (!endDay)
        {
            yield return StartCoroutine(CorgiTools.FadeText(cartelText, 0.5f, Color.white));
            yield return StartCoroutine(FadeInOut.Instance.CrossFadeAlpha(0, 0.5f));
        }
        this.currentCorutine = null;
    }
}