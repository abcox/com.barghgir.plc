using com.barghgir.plc.common.Encryption;
using com.barghgir.plc.infra.common.Encryption;
using com.barghgir.plc.web.Services;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.ViewModels
{
    public partial class SignInViewModel : BaseViewModel
    {
        private readonly MemberService memberService;

        public SignInViewModel(
            MemberService memberService,
            IConnectivity connectivity
            ): base(connectivity)
        {
            this.memberService = memberService;
        }

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;

        [RelayCommand]
        async Task SignIn()
        {
            if (IsBusy) return;

            if (string.IsNullOrEmpty(password))
            {
                // todo: implement toaster/notification system
                await Shell.Current.DisplayAlert("Error!", $"Password required", "OK");
                return;
            }
            //await Shell.Current.DisplayAlert("SignIn", $"Check username and password [{protectedPassword}]", "OK");
            var token = await memberService.SignIn(username.Trim(), password.Trim());
            var url = memberService.lastUrl;
            var isValidToken = JwtTokenHelper.IsTokenValid(token);
            if (!isValidToken)
            {
                await Shell.Current.DisplayAlert("SignIn", $"Sign-in failed. Email support. isValidToken:{isValidToken}; url:{url}; token:{token}", "OK");
                return;
            }
            Password = string.Empty;    // todo: test this...  put some unit testing on presentation!
            Username = string.Empty;
            var isAdmin = JwtTokenHelper.IsAdmin(token);
            await Shell.Current.GoToAsync("..", true,
                new Dictionary<string, object> {
                    { "IsSignedIn", true },
                    { "IsAdmin", isAdmin }
                }); // todo: best to make state model for passing (i.e. StateViewModel)
        }
    }
}
