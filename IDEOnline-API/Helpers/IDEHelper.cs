using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Helpers
{
    public class IDEHelper
    {
        private Process process;
        private ProcessStartInfo startInfo;
        private string directory;
        private bool waitingForInput;

        public event EventHandler<string> OnOutputRecived;
        public event EventHandler<string> OnStandardInputRequest;

        public IDEHelper()
        {
            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "IDEOnline", "Miscs");
        }

        public async Task<string> CompileCodeAsync(string ID)
        {
            startInfo = new ProcessStartInfo
            {
                FileName = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\Roslyn\csc.exe",
                WorkingDirectory = directory,
                Arguments = $"{ID}.cs",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();
            var exitCode = process.ExitCode;

            if (exitCode == 0)
                return "Compile Succeded!";

            return output;
        }

        public async Task<int> RunAppAsync(string ID)
        {
            startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(directory, $"{ID}"),
                WorkingDirectory = directory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false
            };

            process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            await ScanOutput();

            return 0;
        }            

        public async Task InputRecived(string input, string ID)
        {
            startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(directory, $"{ID}"),
                RedirectStandardInput = true                
            };
            process.Start();

            await process.StandardInput.WriteLineAsync(input);
        }

        private async Task ScanOutput()
        {

            while (true)
            {
                OnOutputRecived.Invoke(this, process.StandardOutput.ReadLine());
                if (process.StandardOutput.EndOfStream)
                {
                    break;
                }
            }
        }
    }
}
