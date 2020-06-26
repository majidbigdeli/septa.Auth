using System;
using System.Collections.Generic;
using System.Linq;

namespace septa.Auth.Domain.Hellper
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            if (source != null)
                return source.Count <= 0;
            return true;
        }

        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            Check.NotNull<ICollection<T>>(source, nameof(source));
            if (source.Contains(item))
                return false;
            source.Add(item);
            return true;
        }

        public static IEnumerable<T> AddIfNotContains<T>(
          this ICollection<T> source,
          IEnumerable<T> items)
        {
            Check.NotNull<ICollection<T>>(source, nameof(source));
            List<T> objList = new List<T>();
            foreach (T obj in items)
            {
                if (!source.Contains(obj))
                {
                    source.Add(obj);
                    objList.Add(obj);
                }
            }
            return (IEnumerable<T>)objList;
        }

        public static bool AddIfNotContains<T>(
          this ICollection<T> source,
          Func<T, bool> predicate,
          Func<T> itemFactory)
        {
            Check.NotNull<ICollection<T>>(source, nameof(source));
            Check.NotNull<Func<T, bool>>(predicate, nameof(predicate));
            Check.NotNull<Func<T>>(itemFactory, nameof(itemFactory));
            if (source.Any<T>(predicate))
                return false;
            source.Add(itemFactory());
            return true;
        }

        public static IList<T> RemoveAll<T>(this ICollection<T> source, Func<T, bool> predicate)
        {
            List<T> list = source.Where<T>(predicate).ToList<T>();
            foreach (T obj in list)
                source.Remove(obj);
            return (IList<T>)list;
        }
    }

}
