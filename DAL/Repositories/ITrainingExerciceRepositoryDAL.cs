using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ITrainingExerciceRepositoryDAL
    {
        void Create(TrainingExerciceDAL t);
        IEnumerable<TrainingExerciceDAL> GetAll();
        void Delete(TrainingExerciceDAL t);
        IEnumerable<TrainingExerciceDAL> GetByIdTraining(int id);
        TrainingExerciceDAL GetById(int id_training, int id_exercice);
        void Update(TrainingExerciceDAL t);
        Task CallApiWithJwtToken(string token);

    }
}
