using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IAuthRepositoryDAL
    {
        PersonDAL Login(LoginDAL l);
        void Register(PersonDAL p);
        string GetJwtToken(LoginDAL l);

        Task CallApiWithJwtToken(string token);

        public string JwtToken { get; set; }
    }
}
