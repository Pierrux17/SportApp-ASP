using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ITypeExerciceRepositoryDAL
    {
        void Create(TypeExerciceDAL t);
        IEnumerable<TypeExerciceDAL> GetAll();
        TypeExerciceDAL GetById(int id);
        void Update(TypeExerciceDAL t);
        void Delete(TypeExerciceDAL t);

        Task CallApiWithJwtToken(string token);


    }
}
