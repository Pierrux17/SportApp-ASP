using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ExerciceLogDAL
    {
        public int Id { get; set; }
        public string? Reps { get; set; }
        public string? Weight { get; set; }
        public string? Distance { get; set; }
        public string? Time { get; set; }
        public string? Comment { get; set; }
        public int Id_training_log { get; set; }
        public int Id_exercice { get; set; }
    }
}
