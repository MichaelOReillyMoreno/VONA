using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Clase que controla todos los canvas
/// </summary>
public class MenuCanvas : MonoBehaviour
{
    public static MenuCanvas Instance { get; private set; }

    //Tiempo de fundido 
    [SerializeField]
    private float fadeTime;

    [SerializeField]
    public float musicVolume;

    [SerializeField]
    public float ambientMusicVolume;

    public bool FadeOnStart = true;

    //Imagen de fundido
    [SerializeField]
    private Image faderImage;

    [SerializeField]
    private Dropdown language;

    [SerializeField]
    private TextsManager textsMan;

    //Canvas propio
    private Canvas myCanvas;

    void Awake()
    {
        Instance = this;
        this.myCanvas = this.GetComponent<Canvas>();

        if (this.FadeOnStart)
            this.Fade(0, " ");
    }
    private void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
    }
    /// <summary>
    /// Metodo que pausa el juego
    /// </summary>
    public void Pause()
    {
        if (Time.timeScale > 0.0f)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }

    /// <summary>
    /// Metodo para iniciar un nivel
    /// </summary>
    /// <param name="level"></param>
    public void PlayLevel(string level)
    {
        this.Fade(1, level);
    }

    /// <summary>
    /// Metodo para cambiar entre canvas
    /// </summary>
    public void ChangeCanvas(Canvas myFutureCanvas)
    {
        this.myCanvas.enabled = false;
        myFutureCanvas.enabled = true;
    }

    /// <summary>
    /// Metodo para hacer un fundido en la pantalla
    /// </summary>
    /// <param name="alpha"></param>
    /// <param name="nameLevel"></param>
    private void Fade(int alpha, string nameLevel)
    {
        this.faderImage.gameObject.SetActive(true);
        faderImage.enabled = true;
        this.faderImage.CrossFadeAlpha(alpha, this.fadeTime, false);

        if (alpha == 1)
            StartCoroutine(this.PlayLevelCo(this.fadeTime, nameLevel));
        else
        {
            StartCoroutine(MusicManager.Instance.FadeSound(this.fadeTime, FadeType.IN, musicVolume));
            if (AmbientMusicManager.Instance)
                StartCoroutine(AmbientMusicManager.Instance.FadeSound(this.fadeTime, FadeType.IN, ambientMusicVolume));
        }
    }

    /// <summary>
    /// Metodo para cargar un nivel pasado un tiempo determinado
    /// </summary>
    /// <param name="time"></param>
    /// <param name="nameLevel"></param>
    /// <returns></returns>
    private IEnumerator PlayLevelCo(float time, string nameLevel)
    {
        StartCoroutine(MusicManager.Instance.FadeSound(this.fadeTime, FadeType.OUT, musicVolume));
        if (AmbientMusicManager.Instance)
            StartCoroutine(AmbientMusicManager.Instance.FadeSound(this.fadeTime, FadeType.OUT, ambientMusicVolume));

        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(nameLevel);
    }

    public void SetDay(int num)
    {
        PlayerPrefs.SetInt("Day", num);
    }

    public void ChangeLanguage()
    {
        textsMan.ChangeLanguage(language.value);
    }
}