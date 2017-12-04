using System.Collections.Generic;

public class LanguageTexts
{
    public List<Subtitles> Subtitles { get; set; }
    public List<GameText> Texts { get; set; }

    public LanguageTexts(List<Subtitles> Subtitles, List<GameText> Texts)
    {
        this.Subtitles = Subtitles;
        this.Texts = Texts;
    }
}