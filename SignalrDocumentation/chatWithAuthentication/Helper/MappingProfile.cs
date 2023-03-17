using AutoMapper;
using ChatWithAuth.DAL.Data;
using chatWithAuthentication.Models;

namespace chatWithAuthentication.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, MessageViewModel>()
                .ForMember(d => d.SenderName, s => s.MapFrom(o => o.SenderUser.UserName))
                .ForMember(d => d.RecieverName, s => s.MapFrom(o => o.ReceiverUser.UserName));
        }
    }
}
