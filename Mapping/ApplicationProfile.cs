using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Mapping
{
  public class ApplicationProfile : Profile
  {
    public ApplicationProfile()
    {
      CreateMap<Domain.AppLogic.Profile, Models.DTOs.ProfileDto>().ReverseMap()
        //.ForMember(destinationMember: dest => dest.AvatarUrl, memberOptions: op => 
        //op.MapFrom(mapExpression: src => src.AvatarUrl == ...))
        ;
      
    }
  }
}
