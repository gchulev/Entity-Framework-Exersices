namespace MusicHub.Data.Models
{
    public class Producer
    {
        public Producer()
        {
            this.Albums = new HashSet<Album>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Pseudonym { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public ICollection<Album> Albums { get; set; }
    }
}
