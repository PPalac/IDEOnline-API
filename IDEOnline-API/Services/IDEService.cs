using IDEOnlineAPI.Helpers;
using IDEOnlineAPI.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Services
{
    public class IDEService : IIDEService
    {
        private string directory;
        private IDEHelper runHelper;

        public event EventHandler<string> OnOutputRecived;
        public event EventHandler<string> OnStandardInputRequest;

        public IDEService()
        {
            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "IDEOnline", "Miscs");
            
        }

        public async Task<string> CompileAsync(string code, string ID)
        {
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, $"{ID}.cs"), code);

            var compiler = new IDEHelper();
            var result = await compiler.CompileCodeAsync(ID);

            return result;
        }

        public async Task<int> RunAsync(string ID)
        {
            runHelper = new IDEHelper();
            runHelper.OnOutputRecived += OnOutputRecived;
            runHelper.OnStandardInputRequest += OnStandardInputRequest;

            var result = await runHelper.RunAppAsync(ID);

            return result;
        }

        public async Task InputRecivedAsync(string input, string ID)
        {
            runHelper = new IDEHelper();
            await runHelper.InputRecived(input, ID);
        }
    }
}
