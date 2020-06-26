using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface
{
    public interface IIdentityResourceDataSeeder
    {
        Task CreateStandardResourcesAsync();
    }

}
