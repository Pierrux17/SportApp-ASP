using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ITypeProgramRepositoryDAL
    {
        void Create(TypeProgramDAL t);
        IEnumerable<TypeProgramDAL> GetAll();
        TypeProgramDAL GetById(int id);
        void Update(TypeProgramDAL t);
        void Delete(TypeProgramDAL t);

        Task CallApiWithJwtToken(string token);

    }
}
