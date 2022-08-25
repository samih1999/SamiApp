using SamiApp.Data;
using SamiApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SamiApp.Services.Implementation
{
    public class RegService : IRegisterService
    {
        private readonly ApplicationDbContext _context;

        public RegService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Register reg)
        {
            await _context.registers.AddAsync(reg);
            await _context.SaveChangesAsync();
        }

        public Register GetById(string regemail)=> _context.registers.Where(e => e.Email == regemail).FirstOrDefault();

        public bool CheckEmail(string email)
        {
            var e = _context.logins.Where(x => x.Username == email).FirstOrDefault();
            if (e.Username != null)
                return true;
            return false;
        }

        public bool CheckPass(byte[] Pass)
        {
            var e = _context.registers.Where(x => x.PasswordHash == Pass).FirstOrDefault();
            if (e == null)
                return false;
            return true;
        }
        public Register logcheck(string username, string password)
        {
            var user = GetById(username);
            if (user == null) return null;

                var hmac = new HMACSHA512(user.PasswordSalt);
           
            
            //var e = _context.logins.Where(x => x.Password == Pass).FirstOrDefault();
            var e = _context.registers.Where(x => x.Email == username && x.PasswordHash == hmac.ComputeHash(Encoding.UTF8.GetBytes(password))).FirstOrDefault(); 
          
            if (e != null)
                return e;
            return e;
        }

        public string GetRole(string email)
        {
            var user = GetById(email);
            if (user !=null)
            return user.Role;

            return "usernotfound";
        }
    }
}
