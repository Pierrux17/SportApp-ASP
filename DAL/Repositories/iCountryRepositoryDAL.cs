using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ICountryRepositoryDAL
    {
        void Create(CountryDAL c);
        IEnumerable<CountryDAL> GetAll();
        CountryDAL GetById(int id);
        void Update(CountryDAL c);
        void Delete(CountryDAL c);

        Task CallApiWithJwtToken(string token);

    }
}
