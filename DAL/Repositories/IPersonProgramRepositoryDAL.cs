using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IPersonProgramRepositoryDAL
    {
        void Create(PersonProgramDAL p);
        IEnumerable<PersonProgramDAL> GetAll();
        void Delete(PersonProgramDAL p);
        //void Delete(int id_person, int id_program);

        IEnumerable<PersonProgramDAL> GetByIdPerson(int id);
        PersonProgramDAL GetById(int id_person, int id_program);

        Task CallApiWithJwtToken(string token);

    }
}
