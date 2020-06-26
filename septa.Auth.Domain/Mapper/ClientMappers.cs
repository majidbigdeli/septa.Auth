using AutoMapper;
using IdentityServer4.Models;

namespace septa.Auth.Domain.Mapper
{
    public static class ClientMappers
    {
        static ClientMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClientMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Client ToModel(this Entities.Client entity)
        {
            return Mapper.Map<Client>(entity);
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Entities.Client ToEntity(this Client model)
        {
            return Mapper.Map<Entities.Client>(model);
        }
    }
}
