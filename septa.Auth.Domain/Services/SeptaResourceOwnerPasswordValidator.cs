﻿using IdentityServer4.AspNetIdentity;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Services
{
    public class SeptaResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        protected IApplicationSignInManager SignInManager { get; }
        protected IEventService Events { get; }
        protected IApplicationUserManager UserManager { get; }
        protected ILogger<ResourceOwnerPasswordValidator<IdentityUser>> Logger { get; }

        public SeptaResourceOwnerPasswordValidator(
            IApplicationUserManager userManager,
            IApplicationSignInManager signInManager,
            IEventService events,
            ILogger<ResourceOwnerPasswordValidator<IdentityUser>> logger
            )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Events = events;
            Logger = logger;
        }

        ///// <summary>
        ///// https://github.com/IdentityServer/IdentityServer4/blob/master/src/AspNetIdentity/src/ResourceOwnerPasswordValidator.cs#L53
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //[UnitOfWork]
        public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            await ReplaceEmailToUsernameOfInputIfNeeds(context);
            var user = await UserManager.FindByNameAsync(context.UserName);
            string errorDescription;
            if (user != null)
            {
                var result = await SignInManager.CheckPasswordSignInAsync(user, context.Password, true);
                if (result.Succeeded)
                {
                    var sub = await UserManager.GetUserIdAsync(user);

                    Logger.LogInformation("Credentials validated for username: {username}", context.UserName);
                    await Events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, sub, context.UserName, interactive: false));

                    var additionalClaims = new List<Claim>();

                    await AddCustomClaimsAsync(additionalClaims, user, context);
                    context.Result = new GrantValidationResult(sub, "pwd", (IEnumerable<Claim>)additionalClaims.ToArray(), "local", (Dictionary<string, object>)null);


                    return;
                }
                else if (result.IsLockedOut)
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: locked out", context.UserName);
                    await Events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "locked out", interactive: false));
                    errorDescription = "UserLockedOut";
                }
                else if (result.IsNotAllowed)
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.UserName);
                    await Events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "not allowed", interactive: false));
                    errorDescription = "LoginIsNotAllowed";
                }
                else
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
                    await Events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials", interactive: false));
                    errorDescription = "InvalidUserNameOrPassword";
                }
            }
            else
            {
                Logger.LogInformation("No user found matching username: {username}", context.UserName);
                await Events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", interactive: false));
                errorDescription = "InvalidUsername";
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, errorDescription);
        }


        protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(
  ResourceOwnerPasswordValidationContext context)
        {
            if (!ValidationHelper.IsValidEmailAddress(context.UserName))
                return;
            if (await this.UserManager.FindByNameAsync(context.UserName).ConfigureAwait(false) != null)
                return;
            var identityUser = await UserManager.FindByEmailAsync(context.UserName).ConfigureAwait(false);
            if (identityUser == null)
                return;
            context.UserName = identityUser.UserName;
        }

        protected virtual Task AddCustomClaimsAsync(
          List<Claim> customClaims,
          User user,
          ResourceOwnerPasswordValidationContext context)
        {
            //user.TenantId.HasValue
            if (false)
            {
                //List<Claim> claimList = customClaims;
                //string tenantId1 = AbpClaimTypes.TenantId;
                //Guid? tenantId2 = user.TenantId;
                //ref Guid? local = ref tenantId2;
                //string str = local.HasValue ? local.GetValueOrDefault().ToString() : (string)null;
                //Claim claim = new Claim(tenantId1, str);
                //claimList.Add(claim);
            }
            return Task.CompletedTask;
        }



    }

    public class ValidationHelper
    {
        private const string EmailRegEx = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        public static bool IsValidEmailAddress(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            /*RFC 2822 (simplified)*/
            return Regex.IsMatch(email, EmailRegEx, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }

}

