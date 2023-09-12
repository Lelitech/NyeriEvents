using AutoMapper;
using NyeriEvents.Entities;
using NyeriEvents.Requests;

namespace NyeriEvents.Profiles
{
    public class NyeriProfiles : Profile
    {
        public NyeriProfiles()
        {
            CreateMap<BookEvent, Event>().ReverseMap();
            CreateMap<AddUser,  User>().ReverseMap();
            CreateMap<AddEvent, Event>().ReverseMap();
        }
    }
}
