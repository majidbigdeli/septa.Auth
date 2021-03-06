﻿using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace septa.Auth.Domain.Services
{
    public class ConfirmEmailDataProtectionTokenProviderOptions : DataProtectionTokenProviderOptions
    { }

    public class ConfirmEmailDataProtectorTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        //public ConfirmEmailDataProtectorTokenProvider(
        //    IDataProtectionProvider dataProtectionProvider,
        //    IOptions<ConfirmEmailDataProtectionTokenProviderOptions> options)
        //    : base(dataProtectionProvider, options,)
        //{
        //    // NOTE: DataProtectionTokenProviderOptions.TokenLifespan is set to TimeSpan.FromDays(1)
        //    // which is low for the `ConfirmEmail` task.
        //}
        public ConfirmEmailDataProtectorTokenProvider
            (IDataProtectionProvider dataProtectionProvider,
            IOptions<ConfirmEmailDataProtectionTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
        {
        }
    }

}
