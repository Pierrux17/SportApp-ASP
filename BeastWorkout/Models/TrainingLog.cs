namespace BeastWorkout.Models
{
    public class TrainingLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Id_person { get; set; }
        public int Id_training { get; set; }

        public virtual Person? Person { get; set; }
        public virtual Training? Training { get; set; }
        public virtual IEnumerable<ExerciceLog>? ExerciceLogs { get; set; }
        public virtual Program? Program { get; set; }
        public string FormattedDate => Date.ToString("dd-MM-yy"); //Formatte la date
    }
}
