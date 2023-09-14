namespace BeastWorkout.Models
{
    public class Program
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Nbtrainingperweek { get; set; }
        public int? Duration { get; set; }
        public string? Objectif { get; set; }
        public int Id_type_program { get; set; }
        public bool Is_my_Program { get; set; }
        public int Created_by { get; set; }

        public virtual TypeProgram? TypeProgram { get; set; }
        public virtual IEnumerable<ProgramTraining>? Trainings { get; set; }

    }
}
