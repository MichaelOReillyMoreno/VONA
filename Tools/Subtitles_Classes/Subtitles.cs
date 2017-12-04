using System.Collections.Generic;

public class Subtitles
{
    public int Id { get; set; }
    public List<Subtitle> SubList { get; set; }

    public Subtitles(int Id, List<Subtitle> SubList)
    {
        this.Id = Id;
        this.SubList = SubList;
    }

    public Subtitles(Subtitles sub)
    {
        this.Id = sub.Id;
        this.SubList = new List<Subtitle>();

        for (int i = 0; i < sub.SubList.Count; i++)
        {
            this.SubList.Add(new Subtitle(sub.SubList[i]));
        }
    }
}