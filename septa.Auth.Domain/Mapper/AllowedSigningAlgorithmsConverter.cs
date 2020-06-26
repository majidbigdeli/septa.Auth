using AutoMapper;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace septa.Auth.Domain.Mapper
{
    public class AllowedSigningAlgorithmsConverter :
        IValueConverter<ICollection<string>, string>,
        IValueConverter<string, ICollection<string>>
    {
        public static AllowedSigningAlgorithmsConverter Converter = new AllowedSigningAlgorithmsConverter();

        public string Convert(ICollection<string> sourceMember, ResolutionContext context)
        {
            if (sourceMember == null || !sourceMember.Any())
            {
                return null;
            }
            return sourceMember.Aggregate((x, y) => $"{x},{y}");
        }

        public ICollection<string> Convert(string sourceMember, ResolutionContext context)
        {
            var list = new HashSet<string>();
            if (!String.IsNullOrWhiteSpace(sourceMember))
            {
                sourceMember = sourceMember.Trim();
                foreach (var item in sourceMember.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct())
                {
                    list.Add(item);
                }
            }
            return list;
        }
    }

    public static class ScopeMappers
    {
        static ScopeMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ScopeMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ApiScope ToModel(this Entities.ApiScope entity)
        {
            return entity == null ? null : Mapper.Map<ApiScope>(entity);
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Entities.ApiScope ToEntity(this ApiScope model)
        {
            return model == null ? null : Mapper.Map<Entities.ApiScope>(model);
        }
    }

}
