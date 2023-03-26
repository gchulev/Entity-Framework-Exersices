namespace Footballers.Data.Models
{
    public class TeamFootballer
    {
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public  int FootballerId { get; set; }
        public Footballer Footballer { get; set; } = null!;
    }
}
