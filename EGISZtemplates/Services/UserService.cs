using EGISZtemplates.Data;
using EGISZtemplates.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EGISZtemplates.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);
            return user;
        }

        /*
        public async Task RegisterAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        */
    }
}
