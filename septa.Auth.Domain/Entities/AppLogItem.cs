using septa.Auth.Domain.Interface;
using System;

namespace septa.Auth.Domain.Entities
{
    public class AppLogItem : Entitiy<int>, IAuditableEntity
    {
        public DateTimeOffset? CreatedDateTime { get; set; }

        public int EventId { get; set; }

        public string Url { get; set; }

        public string LogLevel { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string StateJson { get; set; }
    }


}
