namespace BeastWorkout.Models
{
    public class Performance
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }
        public int Id_profil { get; set; }
        public int Id_exercice { get; set; }
        public virtual Profil? Profil { get; set; }
        public virtual Exercice? Exercice { get; set; }
    }
}
