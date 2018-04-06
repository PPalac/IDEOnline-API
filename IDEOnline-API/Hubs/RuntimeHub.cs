using IDEOnlineAPI.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Hubs
{
    public class RuntimeHub : Hub
    {
        private IIDEService ideService;

        public RuntimeHub(IIDEService ideService)
        {
            this.ideService = ideService;
            ideService.OnOutputRecived += IDEService_OnOutputRecivedAsync;
            ideService.OnStandardInputRequest += IDEService_OnInputRequestAsync;
        }
        public async Task InputRequest()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("RequestInput");
        }

        public async Task SendOutput(string output)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("Output", output);
        }

        public async Task Run(string ID)
        {
            await ideService.RunAsync(ID);
        }

        public async Task Input(string input, string ID)
        {
            await ideService.InputRecivedAsync(input, ID);
        }

        private async void IDEService_OnOutputRecivedAsync(object sender, string output)
        {
            await SendOutput(output);
        }

        private async void IDEService_OnInputRequestAsync(object sender, string input)
        {
            await InputRequest();
        }
    }
}
