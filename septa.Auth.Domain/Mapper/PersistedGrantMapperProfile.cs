using AutoMapper;
using IdentityServer4.Models;

namespace septa.Auth.Domain.Mapper
{
    public class PersistedGrantMapperProfile : Profile
    {
        /// <summary>
        /// <see cref="PersistedGrantMapperProfile">
        /// </see>
        /// </summary>
        public PersistedGrantMapperProfile()
        {
            CreateMap<Entities.PersistedGrant, PersistedGrant>(MemberList.Destination)
                .ReverseMap();
        }
    }
}
