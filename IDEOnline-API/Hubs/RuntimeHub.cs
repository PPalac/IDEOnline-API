using IDEOnlineAPI.Models;
using IDEOnlineAPI.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Hubs
{
    /// <summary>
    /// SignalR hub used when user start compiled applicaiton.
    /// </summary>
    public class RuntimeHub : Hub
    {
        private IIDEService ideService;

        public static List<Connections> connections = new List<Connections>();

        /// <summary>
        /// RuntimeHub contructor.
        /// </summary>
        /// <param name="ideService"></param>
        public RuntimeHub(IIDEService ideService)
        {
            this.ideService = ideService;
        }

        /// <summary>
        /// Method for sending output from console application.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public async Task SendOutput(string output, string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("Output", output);
        }

        /// <summary>
        /// Method invoked from frontend. Starts compiled application named as given ID param
        /// </summary>
        /// <param name="ID">Unique application name. Generated during compile process.</param>
        /// <returns></returns>
        public async Task Run(string ID)
        {
            connections.Add(new Connections { ConnectionId = Context.ConnectionId, ProcessId = ID });
            await ideService.RunAsync(ID);
        }

        /// <summary>
        /// Method invoked from frontend. Passing values to standard input of running application.
        /// </summary>
        /// <param name="input">Input value</param>
        /// <param name="id">Id of running process</param>
        /// <returns></returns>
        public void Input(string input, string id)
        {
            ideService.PassInputAsync(input, id);
        }

        /// <summary>
        /// Invoked from frontend to kill process of given ID.
        /// </summary>
        /// <param name="ID"></param>
        public void Kill(string ID)
        {
            ideService.Kill(ID);
        }
    }
}
