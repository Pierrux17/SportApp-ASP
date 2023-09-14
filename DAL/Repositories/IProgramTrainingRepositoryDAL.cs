using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IProgramTrainingRepositoryDAL
    {
        void Create(ProgramTrainingDAL p);
        IEnumerable<ProgramTrainingDAL> GetAll();
        void Delete(ProgramTrainingDAL p);
        IEnumerable<ProgramTrainingDAL> GetByIdProgram(int id);

        ProgramTrainingDAL GetById(int id_program, int id_training);

        Task CallApiWithJwtToken(string token);

    }
}
