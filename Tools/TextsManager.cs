using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

/// <summary>
/// Clase que carga los datos de textos del XML
/// </summary>
public class TextsManager : MonoBehaviour
{
    static TextsManager instance;

    public static TextsManager Instance
    {
        get
        {
            return instance;
        }
    }

    //Subtitulos del juego
    public List<Subtitles> GameSubtitles { get; set; }

    //Textos del juego
    public List<GameText> GameTexts { get; set; }

    //Archivo XML
    [SerializeField]
    private TextAsset file;

    //Idiomas del juego
    private List<LanguageTexts> languagesGame;

    private int idCurrentLanguage;

    //subtitulos para crear el efecto de borrachera, en el idioma correcto e incorrecto
    private Subtitles currentLanguajeSub;
    private Subtitles wrongLanguajeSub;

    //arrays de papabras del subtitulos bien y mal
    private string[] wordsRightIdiom;
    private string[] wordsWrongIdiom;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this.gameObject);

        LoadTexts();
    }

    /// <summary>
    /// Metodo para obtener un texto del juego
    /// </summary>

    public string GetGameText(int id)
    {
        foreach (GameText text in GameTexts)
        {
            if (text.Id == id)
                return text.Content;
        }
        return null;
    }

    /// <summary>
    /// Metodo para cambiar el idioma predefinido, en el futuro cambiara de idioma los textos del menu tambien 
    /// </summary>

    public void ChangeLanguage(int idLanguage)
    {
        GameSubtitles = languagesGame[idLanguage].Subtitles;
        GameTexts = languagesGame[idLanguage].Texts;
        idCurrentLanguage = idLanguage;
    }

    /// <summary>
    /// Metodo para cargar los textos
    /// </summary>
    
    public void LoadTexts()
    {
        this.languagesGame = new List<LanguageTexts>();

        this.GetInfoLevel();

        this.GameSubtitles = languagesGame[0].Subtitles;
        this.GameTexts = languagesGame[0].Texts;
    }


    /// <summary>
    /// Cambia el lenguaje del juego si se esta borracho
    /// </summary>

    public void ChangeLanguajeDrunk()
    {
        if (idCurrentLanguage < 2)//Si estamos en castellano o ingles
        {
            ChangeLanguage(2);
        }
        else //Si estamos en ucraniano
        {
            ChangeLanguage(0);
        }
    }

    /// <summary>
    /// Metodo para crear un subtitulo que mezcle dos idiomas aleatoriamente
    /// </summary>
    /// <param name="id"> identificador del subtitulos</param>
    /// <returns></returns>

    public Subtitles MixLanguagesInSubtitles(int id)
    {
        currentLanguajeSub = new Subtitles(GameSubtitles.Single(x => x.Id == id));

        if (idCurrentLanguage < 2)//Si estamos en castellano o ingles
        {
            //se mezclara con el ukraniano
            wrongLanguajeSub = languagesGame[2].Subtitles.Single(x => x.Id == id);
        }
        else //Si estamos en ucraniano
        {
            //se mezclara con el español
            wrongLanguajeSub = languagesGame[0].Subtitles.Single(x => x.Id == id);
        }

        //por cada linea de texto del subtitulo
        for (int i = 0; i < currentLanguajeSub.SubList.Count; i++)
        {
            ////obtiene las listas de palabras
            wordsRightIdiom = currentLanguajeSub.SubList[i].Txt.Split(' ');
            wordsWrongIdiom = wrongLanguajeSub.SubList[i].Txt.Split(' ');

            currentLanguajeSub.SubList[i].Txt = RandomMixTwoTexts();
        }

        return currentLanguajeSub;
    }



    /// <summary>
    /// Mezcla dos textos aleatoriamente
    /// </summary>
    /// <returns>Retorna el texto mezclado</returns>

    private string RandomMixTwoTexts()
    {
        //obitiene el numero de palabras por lista
        int numWordsRightIdiom = wordsRightIdiom.Length;
        int numWordsWrongIdiom = wordsWrongIdiom.Length;

        string mixSubtitleTxt = "";

        //por cada palabra correcta
        for (int j = 0; j < numWordsRightIdiom; j++)
        {
            // hay un porcentaje de que cambie por una incorrecta
            int n = Random.Range(0, 4);

            if (n == 1)
            {
                wordsRightIdiom[j] = "\"" + wordsWrongIdiom[Random.Range(0, numWordsWrongIdiom)] + "\"...";
            }

            //y construye el nuevo subtitulo
            mixSubtitleTxt += wordsRightIdiom[j] + " ";
        }

        return mixSubtitleTxt;
    }

    /// <summary>
    /// Metodo para conseguir la informacion de los textos
    /// </summary>

    private void GetInfoLevel()
    {

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(this.file.text);
        XmlNodeList subtitlesXml = xmlDoc.GetElementsByTagName("languajes");

        List<Subtitles> subtitlesListTemp = new List<Subtitles>();
        List<GameText> gameTextsListTemp = new List<GameText>();


        foreach (XmlNode subs in subtitlesXml)
        {
            languagesGame = new List<LanguageTexts>();

            foreach (XmlNode languages in subs)
            {
                foreach (XmlNode content in languages)
                {
                    if (content.Name == "subtitles")
                    {
                        subtitlesListTemp = new List<Subtitles>();

                        foreach (XmlNode subtitles in content)
                        {
                            subtitlesListTemp.Add(GetInfoSubtitle(subtitles));
                        }
                    }
                    else if (content.Name == "texts")
                    {
                        gameTextsListTemp = new List<GameText>();

                        foreach (XmlNode gameText in content)
                        {
                            gameTextsListTemp.Add(GetInfoGameTexts(gameText));
                        }
                    }
                }

                languagesGame.Add(new LanguageTexts(subtitlesListTemp, gameTextsListTemp));
            }
        }
    }

    private GameText GetInfoGameTexts(XmlNode gameText)
    {
        int id = 0;
        string contentTextTemp = "";

        foreach (XmlNode textPart in gameText)
        {
            if (textPart.Name == "ID")
                id = int.Parse(textPart.InnerText);
            else if (textPart.Name == "content")
            {
                contentTextTemp = textPart.InnerText;
            }
        }

        return new GameText(id, contentTextTemp);
    }

    private Subtitles GetInfoSubtitle(XmlNode subtitles)
    {
        int id = 0;
        List<Subtitle> subListTemp = new List<Subtitle>();

        foreach (XmlNode subtitlePart in subtitles)
        {
            if (subtitlePart.Name == "ID")
                id = int.Parse(subtitlePart.InnerText);

            else if (subtitlePart.Name == "content")     
                subListTemp = GetInfoTexts(subtitlePart);         
        }
        return new Subtitles(id, subListTemp);
    }

    private List<Subtitle> GetInfoTexts(XmlNode subtitleTexts)
    {
        List<Subtitle> subListTemp = new List<Subtitle>();

        foreach (XmlNode subText in subtitleTexts)
        {
            subListTemp.Add(GetInfoText(subText));
        }

        return subListTemp;
    }

    private Subtitle GetInfoText(XmlNode subtitleText)
    {
        float duration = 0;
        string txt = "";

        foreach (XmlNode part in subtitleText)
        {
            if (part.Name == "duration")
                duration = float.Parse(part.InnerText);

            else if (part.Name == "text")
                txt = part.InnerText;
        }

        return new Subtitle (txt, duration);
    }
}
