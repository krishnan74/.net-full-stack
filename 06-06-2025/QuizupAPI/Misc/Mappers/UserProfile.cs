using AutoMapper;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Teacher;
using QuizupAPI.Models.DTOs.Student;

namespace QuizupAPI.Misc
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<StudentAddRequestDTO, User>()
            .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Email))
            .ForMember(dest => dest.HashedPassword, opt => opt.Ignore());

            CreateMap<User, StudentAddRequestDTO>()
            .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Username));

            CreateMap<TeacherAddRequestDTO, User>()
            .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Email))
            .ForMember(dest => dest.HashedPassword, opt => opt.Ignore());

            CreateMap<User, TeacherAddRequestDTO>()
            .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Username));
   
        }
    }
}