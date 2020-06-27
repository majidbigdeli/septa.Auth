using JetBrains.Annotations;
using septa.Auth.Domain.Enums;
using septa.Auth.Domain.Hellper;
using septa.Auth.Domain.Interface;
using System;

namespace septa.Auth.Domain.Entities
{
    public class IdentityClaimType : Entitiy<Guid>, IAuditableEntity
    {
        public virtual string Name { get; protected set; }

        public virtual bool Required { get; set; }

        public virtual bool IsStatic { get; protected set; }

        public virtual string Regex { get; set; }

        public virtual string RegexDescription { get; set; }

        public virtual string Description { get; set; }

        public virtual IdentityClaimValueType ValueType { get; protected set; }

        public virtual string ConcurrencyStamp { get; set; }



        protected IdentityClaimType()
        {

        }

        public IdentityClaimType(
            Guid id,
            [NotNull] string name,
            bool required = false,
            bool isStatic = false,
            [CanBeNull] string regex = null,
            [CanBeNull] string regexDescription = null,
            [CanBeNull] string description = null,
            IdentityClaimValueType valueType = IdentityClaimValueType.String)
        {
            Id = id;
            Name = Check.NotNull(name, nameof(name));
            Required = required;
            IsStatic = isStatic;
            Regex = regex;
            RegexDescription = regexDescription;
            Description = description;
            ValueType = valueType;
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

    }
}
