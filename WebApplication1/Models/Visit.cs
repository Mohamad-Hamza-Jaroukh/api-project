namespace Portfolio.API.Models
{
    public class Visit
    {
        public int Id { get; set; }

        public DateTime VisitDate { get; set; } = DateTime.UtcNow;
    }
}
