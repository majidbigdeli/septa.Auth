﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Hellper;
using septa.Auth.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Services
{
    public class CustomUserValidator : UserValidator<User>
    {
        private readonly ISet<string> _emailsBanList;

        public CustomUserValidator(
            IdentityErrorDescriber errors,// How to use CustomIdentityErrorDescriber
            IOptionsSnapshot<SiteSettings> configurationRoot
            ) : base(errors)
        {
            configurationRoot.CheckArgumentIsNull(nameof(configurationRoot));
            _emailsBanList = new HashSet<string>(configurationRoot.Value.EmailsBanList, StringComparer.OrdinalIgnoreCase);

            if (!_emailsBanList.Any())
            {
                throw new InvalidOperationException("Please fill the emails ban list in the appsettings.json file.");
            }
        }

        public override async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            // First use the built-in validator
            var result = await base.ValidateAsync(manager, user);
            var errors = new List<IdentityError>();

            // Extending the built-in validator
            validateEmail(user, errors);
            validateUserName(user, errors);

            return !errors.Any() ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }


        private void validateEmail(User user, List<IdentityError> errors)
        {
            var userEmail = user?.Email;
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                //if (string.IsNullOrWhiteSpace(userEmail)) {
                //    errors.Add(new IdentityError {
                //        Code = "EmailIsNotSet",
                //        Description = "لطفا اطلاعات ایمیل را تکمیل کنید."
                //    });
                //}
                return; // base.ValidateAsync() will cover this case
            }



            if (_emailsBanList.Any(email => userEmail.EndsWith(email, StringComparison.OrdinalIgnoreCase)))
            {
                errors.Add(new IdentityError
                {
                    Code = "BadEmailDomainError",
                    Description = "لطفا یک ایمیل پروایدر معتبر را وارد نمائید."
                });
            }
        }

        private static void validateUserName(User user, List<IdentityError> errors)
        {
            var userName = user?.UserName;
            if (string.IsNullOrWhiteSpace(userName))
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    errors.Add(new IdentityError
                    {
                        Code = "UserIsNotSet",
                        Description = "لطفا اطلاعات کاربری را تکمیل کنید."
                    });
                }
                return;  // base.ValidateAsync() will cover this case
            }

            //if (!Regex.IsMatch(userName, @"^(\+98|0)?9\d{9}$"))
            //{
            //    errors.Add(new IdentityError
            //    {
            //        Code = "BadUserNameError",
            //        Description = "شماره موبایل درست نمی باشد"
            //    });
            //}

            //if (userName.IsNumeric() || userName.ContainsNumber())
            //{
            //    errors.Add(new IdentityError
            //    {
            //        Code = "BadUserNameError",
            //        Description = "نام کاربری وارد شده نمی‌تواند حاوی اعداد باشد."
            //    });
            //}

            if (userName.HasConsecutiveChars())
            {
                errors.Add(new IdentityError
                {
                    Code = "BadUserNameError",
                    Description = "نام کاربری وارد شده معتبر نیست."
                });
            }
        }
    }



}
