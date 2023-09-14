namespace BeastWorkout.Models
{
    public class ProgramTraining
    {
        public int Id_program { get; set; }
        public int Id_training { get; set; }

        public virtual Program? Program { get; set; }
        public virtual Training? Training { get; set; }
        public virtual IEnumerable<TrainingExercice>? Exercices { get; set; }
    }
}
