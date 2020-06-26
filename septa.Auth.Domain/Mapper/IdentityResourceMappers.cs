using AutoMapper;
using IdentityServer4.Models;

namespace septa.Auth.Domain.Mapper
{
    public static class IdentityResourceMappers
    {
        static IdentityResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static IdentityResource ToModel(this Entities.IdentityResource entity)
        {
            return entity == null ? null : Mapper.Map<IdentityResource>(entity);
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Entities.IdentityResource ToEntity(this IdentityResource model)
        {
            return model == null ? null : Mapper.Map<Entities.IdentityResource>(model);
        }
    }
}
