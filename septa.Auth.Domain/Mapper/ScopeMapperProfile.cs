using AutoMapper;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace septa.Auth.Domain.Mapper
{
    public class ScopeMapperProfile : Profile
    {
        /// <summary>
        /// <see cref="ScopeMapperProfile"/>
        /// </summary>
        public ScopeMapperProfile()
        {
            CreateMap<Entities.ApiScopeProperty, KeyValuePair<string, string>>()
                .ReverseMap();

            CreateMap<Entities.ApiScopeClaim, string>()
               .ConstructUsing(x => x.Type)
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            CreateMap<Entities.ApiScope, ApiScope>(MemberList.Destination)
                .ConstructUsing(src => new ApiScope())
                .ForMember(x => x.Properties, opts => opts.MapFrom(x => x.Properties))
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(x => x.UserClaims))
                .ReverseMap();
        }
    }
}
