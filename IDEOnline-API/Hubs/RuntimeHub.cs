using IDEOnlineAPI.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Hubs
{
    /// <summary>
    /// SignalR hub used when user start compiled applicaiton.
    /// </summary>
    public class RuntimeHub : Hub
    {
        private IIDEService ideService;

        /// <summary>
        /// RuntimeHub contructor.
        /// </summary>
        /// <param name="ideService"></param>
        public RuntimeHub(IIDEService ideService)
        {
            this.ideService = ideService;
            ideService.OnOutputRecived += IDEService_OnOutputRecivedAsync;
            ideService.OnStandardInputRequest += IDEService_OnInputRequestAsync;
        }

        /// <summary>
        /// Method to invoke frontend function while input is needed.
        /// </summary>
        /// <returns></returns>
        public async Task InputRequest()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("RequestInput");
        }

        /// <summary>
        /// Method for sending output from console application.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public async Task SendOutput(string output)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("Output", output);
        }

        /// <summary>
        /// Method invoked from frontend. Starts compiled application named as given ID param
        /// </summary>
        /// <param name="ID">Unique application name. Generated during compile process.</param>
        /// <returns></returns>
        public async Task Run(string ID)
        {
            await ideService.RunAsync(ID);
        }

        /// <summary>
        /// Method invoked from frontend. Passing values to standard input of running application.
        /// </summary>
        /// <param name="input">Input value</param>
        /// <returns></returns>
        public async Task Input(string input)
        {
            await ideService.PassInputAsync(input);
        }

        /// <summary>
        /// Invoked from frontend to kill process of given ID.
        /// </summary>
        /// <param name="ID"></param>
        public void Kill(string ID)
        {
            ideService.Kill(ID);
        }

        /// <summary>
        /// Method called when output recived from running application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="output">Output value</param>
        private async void IDEService_OnOutputRecivedAsync(object sender, string output)
        {
            await SendOutput(output);
        }

        /// <summary>
        /// Method called when input values recived from frontend.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="input">Input value</param>
        private async void IDEService_OnInputRequestAsync(object sender, string input)
        {
            await InputRequest();
        }
    }
}
