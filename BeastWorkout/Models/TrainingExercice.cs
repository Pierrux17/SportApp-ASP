namespace BeastWorkout.Models
{
    public class TrainingExercice
    {
        public int Id_training { get; set; }
        public int Id_exercice { get; set; }
        public string? Serie { get; set; }
        public string? Reps { get; set; }
        public string? Rest { get; set; }
        public string? Weight { get; set; }
        public string? Rpe { get; set; }
        public string? Distance { get; set; }
        public string? Time { get; set; }
        public int Cpt { get; set; }
        public virtual Training? Training { get; set; }
        public virtual Exercice? Exercice { get; set; }
    }
}
