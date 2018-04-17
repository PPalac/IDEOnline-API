using IDEOnlineAPI.Helpers;
using IDEOnlineAPI.Hubs;
using IDEOnlineAPI.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Services
{
    /// <summary>
    /// Service to manage compiling and running application.
    /// </summary>
    public class IDEService : IIDEService
    {
        private IHubContext<RuntimeHub> hubContext;
        private string directory;
        private IDEHelper runHelper;

        /// <summary>
        /// IDEService constructor.
        /// </summary>
        public IDEService(IHubContext<RuntimeHub> hubContext)
        {
            this.hubContext = hubContext;
            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "IDEOnline", "Miscs");
            
        }

        /// <summary>
        /// Saving given code in file named {ID}.cs compile it.
        /// </summary>
        /// <param name="code">Code to save and compile</param>
        /// <param name="ID">Unique name of file to save code.</param>
        /// <returns>'Compile Succesful' while compile complete, compiler output otherwise.</returns>
        public async Task<string> CompileAsync(string code, string ID)
        {
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, $"{ID}.cs"), code);

            var compiler = new IDEHelper();
            var result = await compiler.CompileCodeAsync(ID);

            return result;
        }

        /// <summary>
        /// Runs application of given ID.
        /// </summary>
        /// <param name="ID">Name of process to run</param>
        /// <returns>0 when ends.</returns>
        public async Task<int> RunAsync(string ID)
        {
            runHelper = new IDEHelper(hubContext);

            var result = await runHelper.RunAppAsync(ID);

            return result;
        }

        /// <summary>
        /// Kill running process of given ID
        /// </summary>
        /// <param name="ID">Name of running process</param>
        public void Kill(string ID)
        {
            runHelper = new IDEHelper();
            runHelper.KillRunningProcess(ID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public void PassInputAsync(string input, string id)
        {
            runHelper = new IDEHelper();
            runHelper.PassInputAsync(input, id);
        }
    }
}
