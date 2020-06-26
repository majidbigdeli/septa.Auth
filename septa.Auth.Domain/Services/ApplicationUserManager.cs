using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using septa.Auth.Domain.Contexts;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Hellper;
using septa.Auth.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Services
{
    public class ApplicationUserManager :
    UserManager<User>,
    IApplicationUserManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _uow;
        private readonly IUsedPasswordsService _usedPasswordsService;
        private readonly IdentityErrorDescriber _errors;
        private readonly ILookupNormalizer _keyNormalizer;
        private readonly ILogger<ApplicationUserManager> _logger;
        private readonly IOptions<IdentityOptions> _optionsAccessor;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IServiceProvider _services;
        private readonly DbSet<User> _users;
        private readonly DbSet<Role> _roles;
        private readonly DbSet<UserRole> _userRoles;

        private readonly IApplicationUserStore _userStore;
        private readonly IEnumerable<IUserValidator<User>> _userValidators;
        private User _currentUserInScope;

        public ApplicationUserManager(
            IApplicationUserStore store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<ApplicationUserManager> logger,
            IHttpContextAccessor contextAccessor,
            IUnitOfWork uow,
            IUsedPasswordsService usedPasswordsService)
            : base((UserStore<User, Role, ApplicationDbContext, long, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>)store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _userStore = store;
            _userStore.CheckArgumentIsNull(nameof(_userStore));

            _optionsAccessor = optionsAccessor;
            _optionsAccessor.CheckArgumentIsNull(nameof(_optionsAccessor));

            _passwordHasher = passwordHasher;
            _passwordHasher.CheckArgumentIsNull(nameof(_passwordHasher));

            _userValidators = userValidators;
            _userValidators.CheckArgumentIsNull(nameof(_userValidators));

            _passwordValidators = passwordValidators;
            _passwordValidators.CheckArgumentIsNull(nameof(_passwordValidators));

            _keyNormalizer = keyNormalizer;
            _keyNormalizer.CheckArgumentIsNull(nameof(_keyNormalizer));

            _errors = errors;
            _errors.CheckArgumentIsNull(nameof(_errors));

            _services = services;
            _services.CheckArgumentIsNull(nameof(_services));

            _logger = logger;
            _logger.CheckArgumentIsNull(nameof(_logger));

            _contextAccessor = contextAccessor;
            _contextAccessor.CheckArgumentIsNull(nameof(_contextAccessor));

            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _usedPasswordsService = usedPasswordsService;
            _usedPasswordsService.CheckArgumentIsNull(nameof(_usedPasswordsService));

            _users = uow.Set<User>();
            _roles = uow.Set<Role>();
            _userRoles = uow.Set<UserRole>();
        }

        #region BaseClass

        string IApplicationUserManager.CreateTwoFactorRecoveryCode()
        {
            return base.CreateTwoFactorRecoveryCode();
        }

        Task<PasswordVerificationResult> IApplicationUserManager.VerifyPasswordAsync(IUserPasswordStore<User> store, User user, string password)
        {
            return base.VerifyPasswordAsync(store, user, password);
        }

        public override async Task<IdentityResult> CreateAsync(User user)
        {
            var result = await base.CreateAsync(user);
            if (result.Succeeded)
            {
                await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
            }
            return result;
        }

        public async Task<IdentityResult> CreateUserAsync(User user)
        {

            var result = await base.CreateAsync(user);
            return result;
        }



        public override async Task<IdentityResult> CreateAsync(User user, string password)
        {
            var result = await base.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
            }
            return result;
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string newPassword)
        {

            IdentityResult identityResult = await this.UpdatePasswordHash(user, newPassword, true);
            if (!identityResult.Succeeded)
                return identityResult;
            return await this.UpdateUserAsync(user);
        }


        public override async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            var result = await base.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {
                await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
            }
            return result;
        }

        public override async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
        {
            var result = await base.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
            }
            return result;
        }

        #endregion

        #region CustomMethods


        public async Task<bool> SetUserRole(long userId, List<long> roleIds)
        {

            var userRoles = await (from ur in _userRoles
                                   where ur.UserId.Equals(userId)
                                   select ur).AsNoTracking().ToListAsync();
            _uow.RemoveRange<UserRole>(userRoles);

            await _uow.SaveChangesAsync();

            userRoles = new List<UserRole>();

            foreach (var item in roleIds)
            {
                userRoles.Add(new UserRole()
                {
                    RoleId = item,
                    UserId = userId
                });
            }
            _uow.AddRange<UserRole>(userRoles);

            var result = await _uow.SaveChangesAsync();
            return result > 0;
        }

        public User FindById(long userId)
        {
            return _users.Find(userId);
        }

        public Task<User> FindByIdIncludeUserRolesAsync(int userId)
        {
            return _users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return Users.ToListAsync();
        }

        public User GetCurrentUser()
        {
            if (_currentUserInScope != null)
            {
                return _currentUserInScope;
            }

            var currentUserId = GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return null;
            }

            var userId = int.Parse(currentUserId);
            return _currentUserInScope = FindById(userId);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            return _currentUserInScope ??
                (_currentUserInScope = await GetUserAsync(_contextAccessor.HttpContext.User));
        }

        public string GetCurrentUserId()
        {
            return _contextAccessor.HttpContext.User.Identity.GetUserId();
        }

        public int? CurrentUserId
        {
            get
            {
                var userId = _contextAccessor.HttpContext.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                int result;
                return !int.TryParse(userId, out result) ? (int?)null : result;
            }
        }

        IPasswordHasher<User> IApplicationUserManager.PasswordHasher { get => base.PasswordHasher; set => base.PasswordHasher = value; }

        IList<IUserValidator<User>> IApplicationUserManager.UserValidators => base.UserValidators;

        IList<IPasswordValidator<User>> IApplicationUserManager.PasswordValidators => base.PasswordValidators;

        IQueryable<User> IApplicationUserManager.Users => base.Users;

        public string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext.User.Identity.GetUserName();
        }

        public async Task<bool> HasPasswordAsync(int userId)
        {
            var user = await FindByIdAsync(userId.ToString());
            return user?.PasswordHash != null;
        }

        public async Task<bool> HasPhoneNumberAsync(int userId)
        {
            var user = await FindByIdAsync(userId.ToString());
            return user?.PhoneNumber != null;
        }

        public async Task<byte[]> GetEmailImageAsync(int? userId)
        {
            if (userId == null)
                return "?".TextToImage(new TextToImageOptions());

            var user = await FindByIdAsync(userId.Value.ToString());
            if (user == null)
                return "?".TextToImage(new TextToImageOptions());

            if (!user.IsEmailPublic)
                return "?".TextToImage(new TextToImageOptions());

            return user.Email.TextToImage(new TextToImageOptions());
        }



        public async Task<IdentityResult> UpdateUserAndSecurityStampAsync(int userId, Action<User> action)
        {
            var user = await FindByIdIncludeUserRolesAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }

            action(user);

            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UpdateSecurityStampAsync(user);
        }

        public async Task<IdentityResult> AddOrUpdateUserRolesAsync(int userId, IList<long> selectedRoleIds, Action<User> action = null)
        {
            var user = await FindByIdIncludeUserRolesAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }

            var currentUserRoleIds = user.Roles.Select(x => x.RoleId).ToList();

            if (selectedRoleIds == null)
            {
                selectedRoleIds = new List<long>();
            }
            var newRolesToAdd = selectedRoleIds.Except(currentUserRoleIds).ToList();
            foreach (var roleId in newRolesToAdd)
            {
                user.Roles.Add(new UserRole { RoleId = roleId, UserId = user.Id });
            }

            var removedRoles = currentUserRoleIds.Except(selectedRoleIds).ToList();
            foreach (var roleId in removedRoles)
            {
                var userRole = user.Roles.FirstOrDefault(ur => ur.RoleId == roleId);
                if (userRole != null)
                {
                    user.Roles.Remove(userRole);
                }
            }

            action?.Invoke(user);

            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UpdateSecurityStampAsync(user);
        }


        #endregion
    }

}
