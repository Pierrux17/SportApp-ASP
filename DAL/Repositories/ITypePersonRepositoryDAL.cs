using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ITypePersonRepositoryDAL
    {
        void Create(TypePersonDAL t);
        IEnumerable<TypePersonDAL> GetAll();
        TypePersonDAL GetById(int id);
        void Update(TypePersonDAL t);
        void Delete(TypePersonDAL t);

        Task CallApiWithJwtToken(string token);

    }
}
