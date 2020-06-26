using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using septa.Auth.Domain.Contexts;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Hellper;
using septa.Auth.Domain.Interface;
using System.Security.Claims;

namespace septa.Auth.Domain.Services
{
    public class ApplicationRoleStore :
    RoleStore<Role, ApplicationDbContext, long, UserRole, RoleClaim>,
    IApplicationRoleStore
    {
        private readonly IUnitOfWork _uow;
        private readonly IdentityErrorDescriber _describer;

        public ApplicationRoleStore(
            IUnitOfWork uow,
            IdentityErrorDescriber describer)
            : base((ApplicationDbContext)uow, describer)
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _describer = describer;
            _describer.CheckArgumentIsNull(nameof(_describer));
        }


        #region BaseClass

        protected override RoleClaim CreateRoleClaim(Role role, Claim claim)
        {
            return new RoleClaim
            {
                RoleId = role.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };
        }

        #endregion

        #region CustomMethods

        #endregion
    }



}
