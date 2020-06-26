using System.Collections.Generic;
using System.Reflection;

namespace septa.Auth.Domain.ReflectionToolkit
{
    /// <summary>
    /// Keeps a mapping between a string and a PropertyInfo instance.
    /// Simply wraps an IDictionary and exposes the relevant operations.
    /// Putting all this in a separate class makes the calling code more
    /// readable.
    /// </summary>
    internal class PropertyInfoCache
    {
        private readonly IDictionary<string, PropertyInfo> propertyInfoCache;

        public PropertyInfoCache()
        {
            propertyInfoCache = new Dictionary<string, PropertyInfo>();
        }

        public bool ContainsKey(string key)
        {
            return propertyInfoCache.ContainsKey(key);
        }

        public void Add(string key, PropertyInfo value)
        {
            propertyInfoCache.Add(key, value);
        }

        public PropertyInfo this[string key]
        {
            get { return propertyInfoCache[key]; }
            set { propertyInfoCache[key] = value; }
        }
    }
}
