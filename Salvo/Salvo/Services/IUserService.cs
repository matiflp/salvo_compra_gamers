using Salvo.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterModel model);
        UserManagerResponse ConfirmEmail(long userId, string token);
        Task<UserManagerResponse> ForgetPasswordAsync(string email);
        UserManagerResponse ResetPassword(ResetPasswordModel model);
    }
}
