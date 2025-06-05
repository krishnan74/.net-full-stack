using AutoMapper;
using Notify.Models;
using Notify.Models.DTOs;
namespace Notify.Misc
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<EmployeeAddRequestDTO, User>()
            .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<User, EmployeeAddRequestDTO>()
            .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Username));

            CreateMap<HRAdminAddRequestDTO, User>()
            .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<User, HRAdminAddRequestDTO>()
            .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Username));
   
        }
    }
}