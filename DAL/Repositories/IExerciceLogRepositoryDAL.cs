using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IExerciceLogRepositoryDAL
    {
        void Create(ExerciceLogDAL e);
        IEnumerable<ExerciceLogDAL> GetAll();
        ExerciceLogDAL GetById(int id);
        void Update(ExerciceLogDAL e);
        void Delete(ExerciceLogDAL e);
        IEnumerable<ExerciceLogDAL> GetByIdTrainingLog(int id);

        Task CallApiWithJwtToken(string token);
    }
}
