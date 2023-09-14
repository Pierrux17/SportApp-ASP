namespace BeastWorkout.Models
{
    public class TypeExercice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }
        public int Id_sort_exercice { get; set; }

        public virtual SortExercice? SortExercice { get; set; }
    }
}
