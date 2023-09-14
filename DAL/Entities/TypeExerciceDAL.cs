using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class TypeExerciceDAL
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }
        public int Id_sort_exercice { get; set; }
    }
}
