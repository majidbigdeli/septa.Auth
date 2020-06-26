using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface.Service.Ef
{
    public interface IDbSchemaMigrator
    {
        Task MigrateAsync();

    }
}
