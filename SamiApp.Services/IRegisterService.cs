using SamiApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamiApp.Services
{
   public interface IRegisterService
    {
        bool CheckEmail(string email);
        bool CheckPass(byte[] Pass);
        Register logcheck(string username, string password);
        Task CreateAsync(Register reg);
        Register GetById(string regmail);
    }
}
