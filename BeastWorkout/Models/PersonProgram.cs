using System.ComponentModel;

namespace BeastWorkout.Models
{
    public class PersonProgram
    {
        public int Id_person { get; set; }
        public int Id_program { get; set; }

        public virtual Person? Person { get; set; }
        public virtual Program? Program { get; set; }

        public virtual List<ProgramTraining>? ProgramTrainings { get; set; }
    }
}
