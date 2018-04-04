using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Helpers
{
    public class ProcessHelper
    {
        private Process process;
        private ProcessStartInfo startInfo;
        private string directory;

        public ProcessHelper()
        {
            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "IDEOnline", "Miscs");
        }

        public async Task<string> CompileCodeAsync()
        {
            startInfo = new ProcessStartInfo
            {
                FileName = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\Roslyn\csc.exe",
                WorkingDirectory = directory,
                Arguments = $"Program.cs",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();

            return output;
        }

        public async Task<string> RunAppAsync()
        {
            startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(directory, "Program"),
                WorkingDirectory = directory,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();

            return output;
        }
    }
}
