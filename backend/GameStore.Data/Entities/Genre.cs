namespace GameStore.Data.Entities;

public sealed class Genre : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Game> Games { get; set; }
        = new List<Game>();
}