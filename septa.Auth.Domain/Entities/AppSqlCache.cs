using septa.Auth.Domain.Interface;
using System;

namespace septa.Auth.Domain.Entities
{
    public class AppSqlCache : Entitiy<string>, IAuditableEntity
    {
        public byte[] Value { get; set; }
        public DateTimeOffset ExpiresAtTime { get; set; }
        public long? SlidingExpirationInSeconds { get; set; }
        public DateTimeOffset? AbsoluteExpiration { get; set; }
    }


}
