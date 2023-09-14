using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IExerciceRepositoryDAL
    {
        void Create(ExerciceDAL e);
        IEnumerable<ExerciceDAL> GetAll();
        ExerciceDAL GetById(int id);
        void Update(ExerciceDAL e);
        void Delete(ExerciceDAL e);

        Task CallApiWithJwtToken(string token);

    }
}
