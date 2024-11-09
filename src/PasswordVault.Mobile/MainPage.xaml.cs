using Microsoft.AspNetCore.SignalR.Client;
using PasswordVault.Mobile.ViewModel;

namespace PasswordVault.Mobile
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {

            InitializeComponent();
            
            

            BindingContext = new MainViewModel();
        }


        
    }

}
