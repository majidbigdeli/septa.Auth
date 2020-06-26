using JetBrains.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface.Repository
{
    public interface IBasicRepository<TEntity> where TEntity : class
    {
        [NotNull]
        Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        [NotNull]
        Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        Task DeleteAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    }
}
