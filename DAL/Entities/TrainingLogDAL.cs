using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class TrainingLogDAL
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Id_person { get; set; }
        public int Id_training { get; set; }
    }
}
