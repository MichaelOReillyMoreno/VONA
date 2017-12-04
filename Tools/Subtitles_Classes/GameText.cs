
public class GameText
{
    public int Id { get; set; }
    public string Content { get; set; }

    public GameText(int Id, string Content)
    {
        this.Id = Id;
        this.Content = Content;
    }
}