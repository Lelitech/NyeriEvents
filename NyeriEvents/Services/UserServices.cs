using Microsoft.EntityFrameworkCore;
using nyerievents.services.iservices;
using NyeriEvents.Data;
using NyeriEvents.Entities;
using NyeriEvents.Requests;

namespace NyeriEvents.Services
{
    public class UserServices : IUserService
    {
        private readonly EventDbContext _context;

        public UserServices(EventDbContext context)
        {
            _context = context;
        }
        public async Task<string> BookAnEvent(BookEvent bookEvent)
        {
            var User = await _context.Users.Where(u => u.Id == bookEvent.UserId).FirstOrDefaultAsync();
            var Event = await _context.Events.Where(e => e.Id == bookEvent.EventId).FirstOrDefaultAsync();
            if (User != null && Event != null)
            {
                User.Events.Add(Event);
                await _context.SaveChangesAsync();
                return ("Event booked successfully");
            }
            throw new Exception("Invalid Input!");
        }

        public async Task<string> DeleteUser(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return ("User deleted successfully");
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();

        }

        public async Task<string> RegisterAUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return ("User registered successfully");


        }


        public async Task<User> GetAUserById(Guid id)
        {
            return await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<string> UpdateUser(Guid id, User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return "User Updated Successfully";
        }
    }
}
