using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IPersonRepositoryDAL
    {
        void Create(PersonDAL p);
        IEnumerable<PersonDAL> GetAll();
        PersonDAL GetById(int id);
        void Update(PersonDAL p);
        void Delete(PersonDAL p);

        Task CallApiWithJwtToken(string token);

    }
}
