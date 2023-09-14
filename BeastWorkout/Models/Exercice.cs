namespace BeastWorkout.Models
{
    public class Exercice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public int Id_type_exercice { get; set; }

        public TypeExercice? TypeExercice { get; set; }
    }
}
