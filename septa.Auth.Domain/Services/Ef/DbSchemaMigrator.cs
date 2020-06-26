using Microsoft.EntityFrameworkCore;
using septa.Auth.Domain.Contexts;
using septa.Auth.Domain.Interface.Service.Ef;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Services.Ef
{
    public class DbSchemaMigrator : IDbSchemaMigrator
    {
        private readonly ApplicationDbContext _dbContext;

        public DbSchemaMigrator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task MigrateAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}
