using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ITrainingRepositoryDAL
    {
        void Create(TrainingDAL t);
        IEnumerable<TrainingDAL> GetAll();
        TrainingDAL GetById(int id);
        void Update(TrainingDAL t);
        void Delete(TrainingDAL t);
        TrainingDAL GetLastTraininCreated();

        Task CallApiWithJwtToken(string token);

    }
}
