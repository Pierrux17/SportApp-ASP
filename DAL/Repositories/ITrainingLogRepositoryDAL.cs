using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ITrainingLogRepositoryDAL
    {
        void Create(TrainingLogDAL t);
        IEnumerable<TrainingLogDAL> GetAll();
        TrainingLogDAL GetById(int id);
        void Update(TrainingLogDAL t);
        void Delete(TrainingLogDAL t);
        IEnumerable<TrainingLogDAL> GetByIdPerson(int id);

        Task CallApiWithJwtToken(string token);
    }
}
