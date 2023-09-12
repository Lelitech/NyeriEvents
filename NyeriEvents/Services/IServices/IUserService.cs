using NyeriEvents.Entities;
using NyeriEvents.Requests;
using System.Diagnostics.Eventing.Reader;

namespace nyerievents.services.iservices
{
    public interface IUserService
    {
        Task<string> RegisterAUser(User user);

        Task<string> BookAnEvent(BookEvent bookEvent);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetAUserById(Guid id);
        Task<string>UpdateUser(Guid id, User user);
        Task<string> DeleteUser(User user);

    }
}
