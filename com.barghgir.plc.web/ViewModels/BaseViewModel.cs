using com.barghgir.plc.data.Models;
using com.barghgir.plc.web.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        private readonly IConnectivity connectivity;

        public BaseViewModel(IConnectivity connectivity)
        {
            this.connectivity = connectivity;

            Task.Run(() => CheckInternet());

            isAdmin = false;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UpdateAdminToolVisibilityCommand))]
        bool isAdmin;

        [RelayCommand]
        public async Task UpdateAdminToolVisibilityAsync()
        {
            await Task.Run(async delegate
            {
                await Task.Delay(1000);
                //IsAdmin = !IsAdmin;
            });
        }

        public bool IsNotBusy => !IsBusy;

        public async Task CheckInternet()
        {
            if (connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet", $"Check connection", "OK");
                return;
            }
        }

        [ObservableProperty]
        bool isSignedIn;

        [RelayCommand]
        async Task GoToSignInAsync()
        {
            Console.WriteLine($"Going to sign-in...");

            await Shell.Current.GoToAsync(nameof(SignInPage), true);
        }
    }
}