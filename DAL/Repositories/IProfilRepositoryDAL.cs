using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IProfilRepositoryDAL
    {
        void Create(ProfilDAL p);
        IEnumerable<ProfilDAL> GetAll();
        ProfilDAL GetById(int id);
        void Update(ProfilDAL p);
        void Delete(ProfilDAL p);
        ProfilDAL GetByIdPerson(int id);

        Task CallApiWithJwtToken(string token);
    }
}
