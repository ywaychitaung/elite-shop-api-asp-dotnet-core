using AutoMapper;
using elite_shop.Constants;
using elite_shop.Models.Domains;
using elite_shop.Models.DTOs.Requests;

namespace elite_shop.Mapper;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.Email, opt => opt.Ignore()) // We'll handle encryption separately
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // We'll handle hashing separately
            .ForMember(dest => dest.SaltKey, opt => opt.Ignore()) // Salt generated separately
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)) // Default value
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)) // Default value
            .ForMember(dest => dest.LastLoginAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => AppConstants.Roles.CustomerRoleId));
    }
}
