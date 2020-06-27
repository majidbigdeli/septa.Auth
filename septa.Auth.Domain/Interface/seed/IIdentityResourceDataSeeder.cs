using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface.seed
{
    public interface IIdentityResourceDataSeeder
    {
        Task CreateStandardResourcesAsync();
    }
}
