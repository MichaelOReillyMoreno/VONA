
public class Subtitle
{
    public string Txt { get; set; }
    public float Duration { get; set; }

    public Subtitle(string txt, float duration)
    {
        this.Txt = txt;
        this.Duration = duration;
    }

    public Subtitle(Subtitle sub)
    {
        Txt = sub.Txt;
        Duration = sub.Duration;
    }
}