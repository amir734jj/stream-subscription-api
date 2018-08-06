using AutoMapper;
using Models.Models;

namespace Models.Profiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, User>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}