using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IPerformanceRepositoryDAL
    {
        void Create(PerformanceDAL p);
        IEnumerable<PerformanceDAL> GetAll();
        PerformanceDAL GetById(int id);
        void Update(PerformanceDAL p);
        void Delete(PerformanceDAL p);

        Task CallApiWithJwtToken(string token);
    }
}
