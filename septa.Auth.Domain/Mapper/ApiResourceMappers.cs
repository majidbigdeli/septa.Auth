using AutoMapper;
using IdentityServer4.Models;

namespace septa.Auth.Domain.Mapper
{
    public static class ApiResourceMappers
    {
        static ApiResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ApiResource ToModel(this Entities.ApiResource entity)
        {
            return entity == null ? null : Mapper.Map<ApiResource>(entity);
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Entities.ApiResource ToEntity(this ApiResource model)
        {
            return model == null ? null : Mapper.Map<Entities.ApiResource>(model);
        }
    }
}
