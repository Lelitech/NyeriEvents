using NyeriEvents.Entities;

namespace NyeriEvents.Services.IServices
{
    public interface IEventService
    {
        Task<string> AddEvent(Event Event);
        Task<string> UpdateEvent(Event Event);
        Task<string> DeleteEvent(Event Event);
        Task<IEnumerable<Event>> GetAllEvents();
        Task<Event> GetOneEventbyId(Guid id);
        Task<IEnumerable<Event>> GetEventInLocation(string? location);

        Task<IEnumerable<User>> GetAllUsers(Guid id);



    }
}
