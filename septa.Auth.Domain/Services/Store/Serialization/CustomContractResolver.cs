using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace septa.Auth.Domain.Services.Store.Serialization
{
    public class CustomContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = base.CreateProperties(type, memberSerialization);
            return props.Where(p => p.Writable).ToList();
        }

    }
}
