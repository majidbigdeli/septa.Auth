﻿using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface
{
    public interface IEmailSender
    {
        #region BaseClass

        Task SendEmailAsync(string email, string subject, string message);

        #endregion

        #region CustomMethods

        Task SendEmailAsync<T>(string email, string subject, string viewNameOrPath, T model);

        #endregion
    }
}
