using IDEOnlineAPI.Helpers;
using IDEOnlineAPI.Services.Interfaces;
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
        private string directory;
        private IDEHelper runHelper;

        /// <summary>
        /// EventHandler invoked when output from application is recived.
        /// </summary>
        public event EventHandler<string> OnOutputRecived;

        /// <summary>
        /// EventHandler invoked when input of application is needed. 
        /// </summary>
        public event EventHandler<string> OnStandardInputRequest;

        /// <summary>
        /// IDEService constructor.
        /// </summary>
        public IDEService()
        {
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
        /// <param name="ID"></param>
        /// <returns>0 when ends.</returns>
        public async Task<int> RunAsync(string ID)
        {
            runHelper = new IDEHelper();
            runHelper.OnOutputRecived += OnOutputRecived;
            runHelper.OnStandardInputRequest += OnStandardInputRequest;

            var result = await runHelper.RunAppAsync(ID);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task PassInputAsync(string input)
        {
            runHelper = new IDEHelper();
            await runHelper.PassInputAsync(input);
        }
    }
}
