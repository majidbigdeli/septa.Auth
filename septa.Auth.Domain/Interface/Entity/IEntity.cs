using System;
using System.Collections.Generic;
using System.Text;

namespace septa.Auth.Domain.Interface.Entity
{
    public interface IEntity<TKey> 
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        TKey Id { get; }
    }
}
