using AutoMapper;
using Models.Models;

namespace Models.Profiles
{
    public class StreamingSubscriptionMappingProfile : Profile
    {
        public StreamingSubscriptionMappingProfile()
        {
            CreateMap<StreamingSubscription, StreamingSubscription>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}