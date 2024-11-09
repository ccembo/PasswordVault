
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Plugin.Maui.Biometric;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;
using System.Text;
using System.IO;


namespace PasswordVault.Mobile.ViewModel
{
    

public partial class MainViewModel : ObservableObject
    {
        private readonly HubConnection _KeyExchangeConnection;
        public MainViewModel()
        {
            items = new ObservableCollection<string>();

            ServerConfiguration config = new ServerConfiguration();
            

            var loadedData = ReadJsonFromFileAsync<ServerConfiguration>("DefaultConfig.json");

            if (loadedData.Result != null)
            {
                // Do something with the data
                config = loadedData.Result;
                Console.WriteLine("Loaded data from file");
            }
            else
            {
                var writtendata = WriteJsonToFileAsync<ServerConfiguration>("DefaultConfig.json", config);

            
            }


            _KeyExchangeConnection = new HubConnectionBuilder()
                .WithUrl(config.Server)
                .Build();
            _KeyExchangeConnection.On<string, string>("KeyExchangeRequest", (user, key) =>
            {
                // Do something with the key
                //DisplayAlert("Key Exchange", $"User: {user} key received:{key}.", "Ok"); // Fix the error here
                //Task.Run(async () => await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?Test={user + key}"));
                //Shell.Current.Navigation.PushAsync(new DetailsPage(new DetailViewModel()));
                items.Add(user +":"+ key);



            });
            //Start listening for key exchange
            Task.Run(async () => await _KeyExchangeConnection.StartAsync());

            items.Add("Item 1");

        }

        IConnectivity connectivity;

        [ObservableProperty]
        string texto;

        [ObservableProperty]
        ObservableCollection<string> items;

        [RelayCommand]
        void Add()
        {
            items.Add(texto);
            texto = string.Empty;
        }

        [RelayCommand]
        void Delete(string s)
        {
            if (items.Contains(s))
            {
                items.Remove(s);
            }
        }

        [RelayCommand]
        async Task Tab(string s)
        {
            //KeyExchange.KeyExchange.test();

            var result = await BiometricAuthenticationService.Default.AuthenticateAsync(
                new AuthenticationRequest()
                {
                    Title = "Please authenticate to transmit the user key",
                    NegativeText = "Cancel authentication"
                }, CancellationToken.None);

            if (result.Status == BiometricResponseStatus.Success)
            {
                //await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?Text={s}");
                await _KeyExchangeConnection.InvokeCoreAsync("SendKey", args: new[] { "user", "1234567" });
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Couldn't get authenticated.", "Ok"); // Fix the error here
            }
        }

        ///Read data from the default.json file from the apps location.
        ///
        public async Task<T> ReadJsonFromFileAsync<T>(string filename)
        {
            //string fullPath = Path.Combine(FileSystem.Current.AppDataDirectory, filename);
            string fullPath = Path.Combine(FileSystem.Current.CacheDirectory, filename);

            if (!File.Exists(fullPath))
                return default;


            string json = File.ReadAllText(fullPath);
                //string json = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<T>(json);
            
        }

        //Write data to the default.json file from the apps location.
        public async Task WriteJsonToFileAsync<T>(string filename, T data)
        {
            string json = JsonSerializer.Serialize(data);
            string fullPath = Path.Combine(FileSystem.Current.CacheDirectory, filename);

            using (var writer = new StreamWriter(fullPath, false, Encoding.UTF8))
            {
                await writer.WriteAsync(json);
            }
        }
    }
}
