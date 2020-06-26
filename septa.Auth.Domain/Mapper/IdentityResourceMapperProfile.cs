using AutoMapper;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace septa.Auth.Domain.Mapper
{
    public class IdentityResourceMapperProfile : Profile
    {
        /// <summary>
        /// <see cref="IdentityResourceMapperProfile"/>
        /// </summary>
        public IdentityResourceMapperProfile()
        {
            CreateMap<Entities.IdentityResourceProperty, KeyValuePair<string, string>>()
                .ReverseMap();

            CreateMap<Entities.IdentityResource, IdentityResource>(MemberList.Destination)
                .ConstructUsing(src => new IdentityResource())
                .ReverseMap();

            CreateMap<Entities.IdentityResourceClaim, string>()
               .ConstructUsing(x => x.Type)
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
        }
    }
}
