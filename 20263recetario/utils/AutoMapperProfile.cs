using _20263recetario.DTOs.Categories;
using _20263recetario.DTOs.Identity;
using _20263recetario.Models;
using AutoMapper;


namespace _20263recetario.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserCredentialsDto, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Email));

            CreateMap<RegisterUserDto, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Email));

            CreateMap<Category, CategoryDtos>();
            CreateMap<CategoryCreateDtos, Category>();
            CreateMap<CategoryUpdateDtos, Category>();
        }
    }
}
