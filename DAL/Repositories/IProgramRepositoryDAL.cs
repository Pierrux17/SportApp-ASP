using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IProgramRepositoryDAL
    {
        void Create(ProgramDAL p);
        IEnumerable<ProgramDAL> GetAll();
        ProgramDAL GetById(int id);
        void Update(ProgramDAL p);
        void Delete(ProgramDAL p);
        ProgramDAL GetLastProgramCreated();

        Task CallApiWithJwtToken(string token);

    }
}
