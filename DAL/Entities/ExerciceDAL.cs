using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ExerciceDAL
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public int Id_type_exercice { get; set; }

        //public TypeExerciceDAL TypeExercice { get; set; }
    }
}
