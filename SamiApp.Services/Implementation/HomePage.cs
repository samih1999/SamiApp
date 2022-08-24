using Microsoft.EntityFrameworkCore;
using SamiApp.Data;
using SamiApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamiApp.Services.Implementation
{
   public class HomePage : IHomePage
    {
        private readonly ApplicationDbContext _context;

        public HomePage(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(homePage newHome)
        {
            await _context.homePages.AddAsync(newHome);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<homePage> GetAll() => _context.homePages.AsNoTracking().OrderBy(emp => emp.Email);

        public homePage GetById(int Hid)
        
          => _context.homePages.Where(e => e.Id == Hid).FirstOrDefault();
        

        public async Task UpdateAsync(homePage home)
        {
            _context.Update(home);
            await _context.SaveChangesAsync();
        }

    }
}
