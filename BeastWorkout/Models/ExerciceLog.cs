namespace BeastWorkout.Models
{
    public class ExerciceLog
    {
        public int Id { get; set; }
        public string? Reps { get; set; }
        public string? Weight { get; set; }
        public string? Distance { get; set; }
        public string? Time { get; set; }
        public string? Comment { get; set; }
        public int Id_training_log { get; set; }
        public int Id_exercice { get; set; }

        public virtual TrainingLog? TrainingLog { get; set; }
        public virtual Exercice? Exercice { get; set; }
    }
}
