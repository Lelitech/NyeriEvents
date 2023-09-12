using Microsoft.EntityFrameworkCore;
using NyeriEvents.Data;
using NyeriEvents.Entities;
using NyeriEvents.Services.IServices;

namespace NyeriEvents.Services
{
    public class EventService : IEventService
    {
        private readonly EventDbContext _context;

        public EventService(EventDbContext context)
        {
            _context = context;

        }
        public async Task<string> AddEvent(Event Event)
        {
            _context.Events.Add(Event);
            await _context.SaveChangesAsync();
            return ("Event Added Successfully");
        }

        public async Task<string> DeleteEvent(Event Event)
        {
            _context.Events.Remove(Event);
            await _context.SaveChangesAsync();
            return ("Event Deleted Successfully");
        }

        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsers(Guid id)
        {
            var Event = await _context.Events.Where(e => e.Id == id).FirstOrDefaultAsync();
            var Users = Event.Users.ToList();
            return Users;
        }

        public async Task<Event> GetOneEventbyId(Guid id)
        {
            return await _context.Events.Where(e => e.Id == id).Include(e => e.Users).FirstOrDefaultAsync();
                
        }

        public async Task<IEnumerable<Event>> GetEventInLocation(string? location)
        {
            var query = _context.Events.AsQueryable<Event>();
            query = query.Where(e => e.Location.ToLower().Contains(location.ToLower()));
            return await query.ToListAsync();
        }

        public async Task<string> UpdateEvent(Event Event)
        {
            _context.Events.Update(Event);
            await _context.SaveChangesAsync();
            return ("Event Updated Successfully");
        }


    }
}
