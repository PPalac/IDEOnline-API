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

        public IDEService()
        {
            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "IDEOnline", "Miscs");
        }

        public async Task<string> CompileAsync(string code)
        {
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "Program.cs"), code);

            var compiler = new ProcessHelper();
            var result = await compiler.CompileCodeAsync();

            return result;
        }

        public async Task<string> RunAsync()
        {
            var runner = new ProcessHelper();
            var result = await runner.RunAppAsync();

            return result;
        }
    }
}
