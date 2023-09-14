using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ISortExerciceRepositoryDAL
    {
        void Create(SortExerciceDAL s);
        IEnumerable<SortExerciceDAL> GetAll();
        SortExerciceDAL GetById(int id);
        void Update(SortExerciceDAL s);
        void Delete(SortExerciceDAL s);

        Task CallApiWithJwtToken(string token);

    }
}
