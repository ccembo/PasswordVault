using Microsoft.AspNetCore.SignalR;

namespace SignalRKeyExchange.Hubs
{
    public interface IKeyRepository
    {
        void Add(string name, string message);
        void Remove(string name);
        string Get(string name);
    }

    public class keyRepository : IKeyRepository
    {
        Dictionary<string, string> _store = new Dictionary<string, string>();
        public void Add(string name, string message)
        {
            _store.Add(name, message);
        }

        public void Remove(string name)
        {
            _store.Remove(name);
        }
        public string Get(string name)
        {
            return _store.ContainsKey(name) ? _store[name] : null;
        }
    }
    public class KeyExchangeHub : Hub
    {      
        IKeyRepository _keyRepository;
        public KeyExchangeHub(IKeyRepository keyRepository)
        {
            _keyRepository = keyRepository;
        }  
        public async Task SendKey(string user, string key, IKeyRepository keyRepository)
        {
            keyRepository.Add(user, key);
            await Clients.All.SendAsync("KeyExchangeReceived", user, key);
        }
        public async Task RequestKey(string user, string key)
        {
            await Clients.All.SendAsync("KeyExchangeRequest", user, key);

        }
    }
}