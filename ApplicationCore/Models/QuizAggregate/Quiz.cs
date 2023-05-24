using ApplicationCore.Interfaces.Repository;

namespace ApplicationCore.Models;

public class Quiz : IIdentity<int>
{
    public int Id { get; set; }

    public string Title { get; set; }

    public List<QuizItem> Items { get; set; }

    public Quiz(int id, List<QuizItem> items, string title)
    {
        Id = id;
        Items = items;
        Title = title;
    }

    public Quiz()
    { }
}