using AutoMapper;
using EventMS.Application.DTOs.UsersDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Infrastructure.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ClaimsPrincipal, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.NameIdentifier).Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.GivenName).Value))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.Surname).Value))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.Email).Value))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.FindFirst("nickname").Value))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.Role).Value));
        }

        //    public UserProfile()
        //    {
        //        CreateMap<ClaimsPrincipal, UserDto>()
        //            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => GetClaimValue(src, ClaimTypes.NameIdentifier)))
        //            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => GetClaimValue(src, ClaimTypes.GivenName)))
        //            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => GetClaimValue(src, ClaimTypes.Surname)))
        //            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => GetClaimValue(src, ClaimTypes.Email)))
        //            .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => GetClaimValue(src, "nickname")))
        //            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => GetClaimValue(src, ClaimTypes.Role)));
        //    }

        //    private string GetClaimValue(ClaimsPrincipal src, string claimType)
        //    {
        //        var claim = src.FindFirst(claimType);
        //        return claim != null ? claim.Value : null;
        //    }
        //}
    }
}