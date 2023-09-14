using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class PerformanceDAL
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }
        public int Id_profil { get; set; }
        public int Id_exercice { get; set; }
    }
}
