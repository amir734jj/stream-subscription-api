using AutoMapper;
using Models.Models;
using Models.ViewModels.Streams;

namespace Models.Profiles
{
    public class AddStreamViewModelProfile : Profile
    {
        public AddStreamViewModelProfile()
        {
            CreateMap<AddStreamViewModel, Stream>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore());
        }
    }
}