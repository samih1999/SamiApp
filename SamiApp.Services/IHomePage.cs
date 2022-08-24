using SamiApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamiApp.Services
{
  public  interface IHomePage
    {

        Task CreateAsync(homePage newHome);
        Task UpdateAsync(homePage home);
        homePage GetById(int Hid);
        IEnumerable<homePage> GetAll();

    }
}
