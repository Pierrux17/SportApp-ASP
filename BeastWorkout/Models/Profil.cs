namespace BeastWorkout.Models
{
    public class Profil
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public int Total_xp { get; set; }
        public int Id_person { get; set; }

        public virtual Person? Person { get; set; }
    }
}
